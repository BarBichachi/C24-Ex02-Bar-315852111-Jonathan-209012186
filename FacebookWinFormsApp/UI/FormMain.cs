using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BasicFacebookFeatures.ComponentFactories;
using BasicFacebookFeatures.Logic;
using BasicFacebookFeatures.Properties;
using BasicFacebookFeatures.Strategy;

using FacebookWrapper;

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
            FacebookService.s_CollectionLimit = 25;

            InitializeComponent();
            initializeTabStrategies();

            r_AppLogic.TimerElapsed += onTimerElapsed;
            r_AppLogic.TimeUpdated += onTimeUpdated;
            this.Shown += formMain_Shown;
        }

        private void formMain_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            resetScrollPosition();
            initiateProfilePage();
        }

        private void onTimerElapsed(object sender, EventArgs e)
        {
            MessageBox.Show(@"According to your age you've used Facebook too much time, it's time to say goodbye!");
            closeApplication();
        }

        private void onTimeUpdated(object sender, EventArgs e)
        {
            this.Text = $@"Facebook - {r_AppLogic.GetRemainingTime()} left before shutdown.";
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

        protected override void OnClosing(CancelEventArgs e)
        {
            resetAppSettingsByNeed();
            base.OnClosing(e);
        }

        private void initiateProfilePage()
        {
            try
            {
                initiateProfileElements();
                initiateProfileAboutPanel();
                initiateProfileFlowLayoutPanels();
            }
            catch (Exception e)
            {
                throw new ApplicationException("Initiating the profile page has failed! >\n" + e.Message, e);
            }
            finally
            {
                loadingFinished();
            }
        }

        private void initiateProfileElements()
        {
            Image coverImage = r_AppLogic.UserCoverImage;
            pictureBoxProfileCover.Image = coverImage ?? BasicFacebookFeatures.Properties.Resources.PlaceholderImage;
            pictureBoxProfilePicture.Image = r_AppLogic.UserProfileImage;
            labelProfileFullName.Text = string.IsNullOrEmpty(r_AppLogic.UserName) ? "Unavailable" : r_AppLogic.UserName;
            labelProfileFullName.Visible = true;
            labelProfileNumOfFriends.Text = $@"{r_AppLogic.UserFriendsCount} friends";
            labelProfileNumOfFriends.Visible = true;
            pictureBoxProfilePicture.Region = r_AppLogic.CreateCircularRegion(pictureBoxProfilePicture.Width);
        }

        private void initiateProfileAboutPanel()
        {
            labelProfilePostsAboutBirthday.Text = $@"Birthday - {(r_AppLogic.UserBirthday.HasValue ? r_AppLogic.UserBirthday.Value.ToString("dd/MM/yyyy") : "Unavailable")}";
            labelProfilePostsAboutCity.Text = $@"Lives in - {(string.IsNullOrEmpty(r_AppLogic.UserCity) ? "Unavailable" : r_AppLogic.UserCity)}";
            labelProfilePostsAboutGender.Text = $@"Gender - {(r_AppLogic.UserGender.HasValue ? r_AppLogic.UserGender.Value.ToString() : "Unavailable")}";
        }

        private void initiateProfileFlowLayoutPanels()
        {
            r_AppLogic.PopulateLayout(flowLayoutPanelProfilePostsPhotos, k_ColumnsForProfile, k_BoxesForProfile, new PhotosPopulationStrategy());
            r_AppLogic.PopulateLayout(flowLayoutPanelProfilePostsFriends, k_ColumnsForProfile, k_BoxesForProfile, new FriendsPopulationStrategy());
            r_AppLogic.PopulateLayout(flowLayoutPanelPosts, k_ColumnsForProfile + 1, k_BoxesForTab, new PostsPopulationStrategy());
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
                new Thread(() =>
                {
                    try
                    {
                        this.Invoke((Action)(() =>
                        {
                            r_AppLogic.PopulateLayout(getFlowLayoutForTab(i_SelectedTab), k_ColumnsForTab, k_BoxesForTab, populationStrategyFunc());
                        }));
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException($"An error occurred while populating {i_SelectedTab.Text}!\n\n" + e.Message, e);
                    }
                }) { IsBackground = true }.Start(); 
            }
        }

        private FlowLayoutPanel getFlowLayoutForTab(TabPage i_TabPage)
        {
            switch (i_TabPage.Name)
            {
                case nameof(tabPageProfileLikedPages):
                    return flowLayoutPanelLikedPages;

                case nameof(tabPageProfileFavoriteTeams):
                    return flowLayoutPanelFavoriteTeams;

                case nameof(tabPageProfileFriends):
                    return flowLayoutPanelFriends;

                case nameof(tabPageProfileCheckins):
                    return flowLayoutPanelCheckins;

                case nameof(tabPageProfilePhotos):
                    return flowLayoutPanelPhotos;

                default:
                    throw new ArgumentException(@"Unknown tab", nameof(i_TabPage));
            }
        }

        private void resetAppSettingsByNeed()
        {
            r_AppLogic.ResetAppSettings();
        }

        private void changeProfileTab(string i_TabName)
        {
            int tabNum = 0;

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

        private void closeApplication()
        {
            this.Close();
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
            closeApplication();
        }

        private void factGeneratorButton_Click(object sender, EventArgs e)
        {
            showRandomFact();
        }

        private void tabControlProfileTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage selectedTab = tabControlProfileTabs.SelectedTab;

            if (selectedTab.Tag == null)
            {
                selectedTab.Tag = "Loaded";
                loadTabContent(selectedTab);
            }
        }

        private void linkLabelEditProfile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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
    }
}