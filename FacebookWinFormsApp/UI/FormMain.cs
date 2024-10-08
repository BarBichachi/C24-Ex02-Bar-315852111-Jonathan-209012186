using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using BasicFacebookFeatures.Logic;
using BasicFacebookFeatures.Strategy;

using FacebookWrapper;

namespace BasicFacebookFeatures.UI
{
    // TODO - Add LazyLoadingPhoto
    // TODO - Add new feature
    // TODO - Add another design pattern
    // TODO - Add Two-Way Data Binding
    // TODO - Replace everything to Ex02!!!!

    public partial class FormMain : Form
    {
        private readonly AppLogic r_AppLogic;
        private Dictionary<TabPage, Func<ILayoutPopulationStrategy>> m_TabPopulationStrategies;

        public FormMain()
        {
            FacebookService.s_CollectionLimit = 25;

            InitializeComponent();
            initializeTabStrategies();

            r_AppLogic = AppLogic.Instance;
            this.Shown += FormMain_Shown;
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            resetScrollPosition();
            initiateProfilePage();
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
            r_AppLogic.SaveAppSettings();
            base.OnClosing(e);
        }

        private void initiateProfilePage()
        {
            try
            {
                this.Invoke(new Action(() =>
                            {
                                initiateProfileElements();
                                initiateProfileAboutPanel();
                                initiateProfileFlowLayoutPanels();
                            }));
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
            labelProfileFullName.Text = r_AppLogic.UserName;
            labelProfileFullName.Visible = true;
            labelProfileNumOfFriends.Text = $@"{r_AppLogic.UserFriendsCount} friends";
            labelProfileNumOfFriends.Visible = true;
            pictureBoxProfilePicture.Region = r_AppLogic.CreateCircularRegion(pictureBoxProfilePicture.Width);
        }

        private void initiateProfileAboutPanel()
        {
            labelProfilePostsAboutBirthday.Text = $@"Birthday - {r_AppLogic.UserBirthday?.ToString("MM/dd/yyyy") ?? "Unavailable"}";
            labelProfilePostsAboutCity.Text = $@"Lives in - {r_AppLogic.UserCity ?? "Unavailable"}";
            labelProfilePostsAboutGender.Text = $@"Gender - {r_AppLogic.UserGender.ToString() ?? "Unavailable"}";
        }

        private void initiateProfileFlowLayoutPanels()
        {
            r_AppLogic.PopulateLayout(flowLayoutPanelProfilePostsPhotos, 3, 9, new PhotosPopulationStrategy());
            r_AppLogic.PopulateLayout(flowLayoutPanelProfilePostsFriends, 3, 9, new FriendsPopulationStrategy());
            r_AppLogic.PopulateLayout(flowLayoutPanelPosts, 4, 25, new PostsPopulationStrategy());
        }

        private void loadingFinished()
        {
            labelLoadingNotifier.Visible = false;
            labelLoadingCoffeeReminder.Visible = false;
        }

        private void loadTabContent(TabPage i_SelectedTab)
        {
            if (m_TabPopulationStrategies.TryGetValue(
                    i_SelectedTab,
                    out Func<ILayoutPopulationStrategy> populationStrategyFunc))
            {
                new Thread(() =>
                {
                    try
                    {
                        this.Invoke((Action)(() =>
                        {
                            var populationStrategy = populationStrategyFunc();

                            r_AppLogic.PopulateLayout(getFlowLayoutForTab(i_SelectedTab), 5, 25,
                                populationStrategy);
                            loadingFinished();
                        }));
                    }
                    catch (Exception e)
                    {
                        throw new ApplicationException(
                            $"An error occurred while populating {i_SelectedTab.Text}!\n\n" + e.Message,
                            e);
                    }
                })
                { IsBackground = true }.Start();
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
                    throw new ArgumentException("Unknown tab", nameof(i_TabPage));
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

        private void ProfilePostsPhotosSeeAllPhotos_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            changeProfileTab("Photos");
        }

        private void ProfilePostsFriendsSeeAllFriends_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            changeProfileTab("Friends");
        }

        private void ToolbarFacebookLogo_Click(object sender, EventArgs e)
        {
            resetScrollPosition();
        }

        private void ToolbarExitButton_Click(object sender, EventArgs e)
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
                    // After closing the form, the currentUser object will already be updated
                    initiateProfileElements();
                    initiateProfileAboutPanel();
                }
            }
        }
    }
}