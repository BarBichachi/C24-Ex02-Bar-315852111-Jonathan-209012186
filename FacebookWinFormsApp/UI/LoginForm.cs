using System;
using System.Drawing;
using System.Windows.Forms;

using BasicFacebookFeatures.Logic;

using FacebookWrapper;

namespace BasicFacebookFeatures.UI
{
    public partial class LoginForm : Form
    {
        private static readonly AppSettings sr_AppSettings = AppSettings.Instance;
        private string m_AccessToken;
        private string m_ConnectedUser;
        private bool m_IsLoggedIn;

        public LoginForm()
        {
            InitializeComponent();
            initializeSavedSettings();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            configureAppSettings();
            base.OnFormClosing(e);
        }

        private void initializeSavedSettings()
        {
            m_AccessToken = sr_AppSettings.SavedAccessToken;
            m_ConnectedUser = sr_AppSettings.SavedUsername;

            if (m_AccessToken != "defaultToken")
            {
                loggedInSuccessfully();
            }
        }

        private void login()
        {
            try
            {
                LoginResult loginResult = FacebookService.Connect("EAAF5GQRl3KQBO2FKspw47BqSjwRQA8aVZCYMHI1n9sNfzf5PgurT6orBWpZBJPqIeUZAqB6pAb8OjjsUoSdd5gTzZBxfgHRYVjMB7IIAxbAshoPbV6mNNNoZCg9wyX7WHSRE0dZBTOzGbz9pfvhPlm1mLaHLxVBqfE0xCdaLejLSE77ThNmLi9tt2apOJKv6YoeCczzw2fkZArXqHZB0zoOUZB0PclqSWQA60wiGYYZCIUeFIs3ZCAnqm4ZD");
                //LoginResult loginResult = FacebookService.Login("414623331638436", 
                //    // Permissions
                //    "public_profile",
                //    "email",
                //    "user_age_range",
                //    "user_birthday",
                //    "user_events",
                //    "user_friends",
                //    "user_gender",
                //    "user_hometown",
                //    "user_likes",
                //    "user_link",
                //    "user_location",
                //    "user_photos",
                //    "user_posts",
                //    "user_videos"
                //);

                if (!string.IsNullOrEmpty(loginResult.AccessToken))
                {
                    m_AccessToken = loginResult.AccessToken;
                    m_ConnectedUser = loginResult.LoggedInUser.Name;

                    userJustLoggedIn();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while trying to login\n\n" + e.Message);
            }
        }

        private void userJustLoggedIn()
        {
            loggedInSuccessfully();
            sr_AppSettings.AutoLogin = rememberMeCheckBox.Checked;
        }

        private void loggedInSuccessfully()
        {
            buttonLogin.Enabled = false;
            buttonLogin.Text = $"Logged in as {m_ConnectedUser}";
            buttonLogin.BackColor = Color.LightGreen;
            rememberMeCheckBox.Visible = false;
            buttonLogout.Visible = true;
            buttonContinueToFacebook.Visible = true;
            m_IsLoggedIn = true;

            configureAppSettings();
            AppLogic.Instance.InitializeAppLogic();
        }

        private void logout()
        {
            try
            {
                FacebookService.Logout();
                configureAppSettings();
                loggedOutSuccessfully();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while trying to logout " + e.Message);
            }
        }

        private void loggedOutSuccessfully()
        {
            buttonLogin.Text = "Click here to login!";
            buttonLogin.BackColor = buttonLogout.BackColor;
            buttonLogin.Enabled = true;
            rememberMeCheckBox.Visible = true;
            rememberMeCheckBox.Checked = false;
            buttonLogout.Visible = false;
            buttonContinueToFacebook.Visible = false;
            m_IsLoggedIn = false;
        }

        private void configureAppSettings()
        {
            sr_AppSettings.SavedAccessToken = m_IsLoggedIn ? m_AccessToken : "defaultToken";
            sr_AppSettings.SavedUsername = m_IsLoggedIn ? m_ConnectedUser : "defaultUser";
            sr_AppSettings.AutoLogin = m_IsLoggedIn && rememberMeCheckBox.Checked;

            sr_AppSettings.Save();
        }

        private void continueToFacebook()
        {
            this.DialogResult = m_IsLoggedIn ? DialogResult.Yes : DialogResult.No;
            this.Close();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            logout();
        }

        private void buttonContinueToFacebook_Click(object sender, EventArgs e)
        {
            continueToFacebook();
        }
    }
}