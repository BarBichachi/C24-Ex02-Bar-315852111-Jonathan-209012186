using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BasicFacebookFeatures.Logic;

using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.UI
{
    internal partial class EditProfileForm : Form
    {
        public EditProfileForm()
        {
            InitializeComponent();
            AppLogic appLogic = AppLogic.Instance;
            appLogicBindingSource.DataSource = appLogic;
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
