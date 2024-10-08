using System;
using System.Windows.Forms;
using BasicFacebookFeatures.UI;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            FacebookService.s_UseForamttedToStrings = true;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginForm = new LoginForm();

            Application.Run(loginForm);

            if (loginForm.DialogResult == DialogResult.Yes)
            {
                Application.Run(new FormMain());
            }
            else
            {
                MessageBox.Show("See you next time!");
            }
        }
    }
}