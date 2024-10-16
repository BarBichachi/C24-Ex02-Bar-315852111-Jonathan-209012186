using System;
using System.Windows.Forms;
using BasicFacebookFeatures.Logic;

namespace BasicFacebookFeatures.UI
{
    internal partial class EditProfileForm : Form
    {
        public EditProfileForm()
        {
            InitializeComponent();

            AppLogic appLogic = AppLogic.Instance;
            appLogicBindingSource.DataSource = appLogic;
            birthdayDateTimePicker.MaxDate = DateTime.Today;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}