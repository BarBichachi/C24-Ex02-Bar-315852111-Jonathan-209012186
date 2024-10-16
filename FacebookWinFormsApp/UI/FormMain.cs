﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BasicFacebookFeatures.Logic;
using BasicFacebookFeatures.UI.Strategy;

namespace BasicFacebookFeatures.UI
{
    public partial class FormMain : Form
    {
        private readonly AppLogic r_AppLogic = AppLogic.Instance;
        private Dictionary<TabPage, Func<ILayoutPopulationStrategy>> m_TabPopulationStrategies;

        private const int k_ColumnsForProfile = 3;
        private const int k_BoxesForProfile = 9;
        private const int k_ColumnsForTab = 5;
        private const int k_BoxesForTab = 25;

        public FormMain()
        {
            InitializeComponent();
            initializeTabStrategies();

            r_AppLogic.TimerElapsed += OnTimerElapsed;
            r_AppLogic.TimeUpdated += OnTimeUpdated;
            this.Shown += OnFormShown;
        }

        private void initializeTabStrategies()
        {
            m_TabPopulationStrategies = new Dictionary<TabPage, Func<ILayoutPopulationStrategy>>
            {
                { tabPageProfileLikedPages, () => new LikedPagesPopulationStrategy() },
                { tabPageProfileFavoriteTeams, () => new FavoriteTeamsPopulationStrategy() },
                { tabPageProfileFriends, () => new FriendsPopulationStrategy() },
                { tabPageProfileCheckins, () => new CheckInsPopulationStrategy() },
                { tabPageProfilePhotos, () => new PhotosPopulationStrategy() }
            };
        }
        private void OnFormShown(object sender, EventArgs e)
        {
            Application.DoEvents();
            resetScrollPosition();
            new Thread(initiateProfilePage).Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            r_AppLogic.ResetAppSettings();
            base.OnClosing(e);
        }

        private void initiateProfilePage()
        {
            try
            {
                new Thread(() =>
                {
                    initiateProfileElements();
                    initiateProfileAboutPanel();
                    initiateProfileFlowLayoutPanels();

                    this.Invoke(new Action(() => loadingFinished()));
                }).Start();
            }
            catch (Exception e)
            {
                this.Invoke(new Action(() =>
                {
                    throw new ApplicationException("Initiating the profile page has failed! >\n" + e.Message, e);
                }));
            }
        }

        private void initiateProfileElements()
        {
            Image coverImage = r_AppLogic.UserCoverImage;
            Image profileImage = r_AppLogic.UserProfileImage;

            this.Invoke(new Action(() =>
            {
                pictureBoxProfileCover.Image = coverImage ?? BasicFacebookFeatures.Properties.Resources.PlaceholderImage;
                pictureBoxProfilePicture.Image = profileImage;
                labelProfileFullName.Text = string.IsNullOrEmpty(r_AppLogic.UserName) ? "Unavailable" : r_AppLogic.UserName;
                labelProfileFullName.Visible = true;
                labelProfileNumOfFriends.Text = $@"{r_AppLogic.UserFriendsCount} friends";
                labelProfileNumOfFriends.Visible = true;
                pictureBoxProfilePicture.Region = r_AppLogic.CreateCircularRegion(pictureBoxProfilePicture.Width);
            }));
        }

        private void initiateProfileAboutPanel()
        {
            string birthday = r_AppLogic.UserBirthday.HasValue ? r_AppLogic.UserBirthday.Value.ToString("dd/MM/yyyy") : "Unavailable";
            string city = string.IsNullOrEmpty(r_AppLogic.UserCity) ? "Unavailable" : r_AppLogic.UserCity;
            string gender = r_AppLogic.UserGender.HasValue ? r_AppLogic.UserGender.Value.ToString() : "Unavailable";

            this.Invoke(new Action(() =>
            {
                labelProfilePostsAboutBirthday.Text = $@"Birthday - {birthday}";
                labelProfilePostsAboutCity.Text = $@"Lives in - {city}";
                labelProfilePostsAboutGender.Text = $@"Gender - {gender}";
            }));
        }


        private void initiateProfileFlowLayoutPanels()
        {
            new Thread(() =>
            {
                populateLayout(flowLayoutPanelProfilePostsPhotos, k_ColumnsForProfile, k_BoxesForProfile, new PhotosPopulationStrategy());
                populateLayout(flowLayoutPanelProfilePostsFriends, k_ColumnsForProfile, k_BoxesForProfile, new FriendsPopulationStrategy());
                populateLayout(flowLayoutPanelPosts, k_ColumnsForProfile + 1, k_BoxesForTab, new PostsPopulationStrategy());
            }).Start();
        }

        private void populateLayout(FlowLayoutPanel i_FlowLayoutPanel, int i_NumberOfColumns, int i_MaxBoxes, ILayoutPopulationStrategy i_Strategy)
        {
            i_Strategy.PopulateLayout(r_AppLogic.SimplifiedUser, i_FlowLayoutPanel, i_NumberOfColumns, i_MaxBoxes);
        }

        private void loadingFinished()
        {
            labelLoadingNotifier.Visible = false;
            labelLoadingCoffeeReminder.Visible = false;
        }

        private void loadTabContent(TabPage i_SelectedTab)
        {
            if (m_TabPopulationStrategies.TryGetValue(i_SelectedTab, out Func<ILayoutPopulationStrategy> populationStrategyFunc))
            {
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        FlowLayoutPanel flowLayoutPanel = getFlowLayoutForTab(i_SelectedTab);
                        ILayoutPopulationStrategy strategy = populationStrategyFunc();

                        this.Invoke(new Action(() =>
                        {
                            populateLayout(flowLayoutPanel, k_ColumnsForTab, k_BoxesForTab, strategy);
                        }));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show($"Error populating {i_SelectedTab.Text}: {e.Message}");
                    }
                });

                thread.Start();
            }
        }

        private FlowLayoutPanel getFlowLayoutForTab(TabPage i_TabPage)
        {
            FlowLayoutPanel relevantPanel;

            switch (i_TabPage.Name)
            {
                case nameof(tabPageProfileLikedPages):
                    relevantPanel = flowLayoutPanelLikedPages;
                    break;

                case nameof(tabPageProfileFavoriteTeams):
                    relevantPanel = flowLayoutPanelFavoriteTeams;
                    break;

                case nameof(tabPageProfileFriends):
                    relevantPanel = flowLayoutPanelFriends;
                    break;

                case nameof(tabPageProfileCheckins):
                    relevantPanel = flowLayoutPanelCheckins;
                    break;

                case nameof(tabPageProfilePhotos):
                    relevantPanel = flowLayoutPanelPhotos;
                    break;

                default:
                    throw new ArgumentException(@"Unknown tab", nameof(i_TabPage));
            }

            return relevantPanel;
        }

        private void changeProfileTab(string i_TabName)
        {
            int tabNum;

            switch (i_TabName)
            {
                case "Posts":
                    tabNum = 0;
                    break;
                case "Friends":
                    tabNum = 1;
                    break;
                case "Photos":
                    tabNum = 2;
                    break;
                case "Check-ins":
                    tabNum = 3;
                    break;
                case "Liked Pages":
                    tabNum = 4;
                    break;
                case "Favorite Teams":
                    tabNum = 5;
                    break;

                default:
                    throw new ArgumentException(@"Unknown tab", i_TabName);
            }

            tabControlProfileTabs.SelectedTab = tabControlProfileTabs.TabPages[tabNum];
        }

        private void resetScrollPosition()
        {
            panelMain.AutoScrollPosition = new Point(0, 0);
        }

        private void showRandomFact()
        {
            string randomFact = r_AppLogic.GenerateRandomFact();

            MessageBox.Show(randomFact);
        }

        private void fillTabDetails()
        {
            TabPage selectedTab = tabControlProfileTabs.SelectedTab;

            if (selectedTab.Tag == null)
            {
                selectedTab.Tag = "Loaded";

                loadTabContent(selectedTab);
            }
        }

        private void editProfile()
        {
            using (EditProfileForm editProfileForm = new EditProfileForm())
            {
                if (editProfileForm.ShowDialog() == DialogResult.OK)
                {
                    updateProfileAfterEdit();
                }
            }
        }

        private void updateProfileAfterEdit()
        {
            labelProfileFullName.Text = string.IsNullOrEmpty(r_AppLogic.UserName) ? "Unavailable" : r_AppLogic.UserName;
            labelProfilePostsAboutBirthday.Text = $@"Birthday - {(r_AppLogic.UserBirthday.HasValue ? r_AppLogic.UserBirthday.Value.ToString("dd/MM/yyyy") : "Unavailable")}";
            labelProfilePostsAboutCity.Text = $@"Lives in - {(string.IsNullOrEmpty(r_AppLogic.UserCity) ? "Unavailable" : r_AppLogic.UserCity)}";
        }

        private void OnTimerElapsed(object sender, EventArgs e)
        {
            MessageBox.Show(@"According to your age you've used Facebook too much time, it's time to say goodbye!");
            Close();
        }

        private void OnTimeUpdated(object sender, EventArgs e)
        {
            this.Text = $@"Facebook - {r_AppLogic.GetRemainingTime()} left before shutdown.";
        }

        private void profilePostsPhotosSeeAllPhotos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            changeProfileTab("Photos");
        }

        private void profilePostsFriendsSeeAllFriends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            changeProfileTab("Friends");
        }

        private void toolbarFacebookLogo_Click(object sender, EventArgs e)
        {
            resetScrollPosition();
        }

        private void toolbarExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void factGeneratorButton_Click(object sender, EventArgs e)
        {
            showRandomFact();
        }

        private void tabControlProfileTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillTabDetails();
        }

        private void linkLabelEditProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            editProfile();
        }
    }
}