using System;
using System.Drawing;
using System.Windows.Forms;
using BasicFacebookFeatures.Logic;

namespace BasicFacebookFeatures.UI
{
    public partial class LoginForm : Form
    {
        private readonly AppLogic r_AppLogic = AppLogic.Instance;
        private bool m_IsLoggedIn;

        public LoginForm()
        {
            InitializeComponent();
            tryConnectingToSavedUser();
        }

        private void tryConnectingToSavedUser()
        {
            if (r_AppLogic.ShouldAutoLogin())
            {
                login();
            }
        }

        private void login()
        {
            try
            {
                r_AppLogic.Initialize();

                if (r_AppLogic.SimplifiedUser != null)
                {
                    loggedInSuccessfully();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error while trying to login" + e.Message);
            }
        }

        private void loggedInSuccessfully()
        {
            r_AppLogic.AutoLogin = rememberMeCheckBox.Checked;
            rememberMeCheckBox.Visible = false;
            buttonLogin.Enabled = false;
            buttonLogin.Text = $@"Logged in as {r_AppLogic.UserName}";
            buttonLogin.BackColor = Color.LightGreen;
            buttonLogout.Visible = true;
            buttonContinueToFacebook.Visible = true;
            m_IsLoggedIn = true;

            if (rememberMeCheckBox.Checked)
            {
                r_AppLogic.SaveAppSettings();
            }
        }

        private void logout()
        {
            try
            {
                r_AppLogic.Logout();
                loggedOutSuccessfully();
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Error while trying to logout " + e.Message);
            }
        }

        private void loggedOutSuccessfully()
        {
            buttonLogin.Text = @"Click here to login!";
            buttonLogin.BackColor = buttonLogout.BackColor;
            buttonLogin.Enabled = true;
            rememberMeCheckBox.Visible = true;
            rememberMeCheckBox.Checked = false;
            buttonLogout.Visible = false;
            buttonContinueToFacebook.Visible = false;
            m_IsLoggedIn = false;
        }

        private void continueToFacebook()
        {
            DialogResult = m_IsLoggedIn ? DialogResult.Yes : DialogResult.No;
            Close();
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