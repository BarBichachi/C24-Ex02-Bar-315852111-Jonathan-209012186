using System.Drawing;
using System.Windows.Forms;

namespace BasicFacebookFeatures.UI.ComponentFactories
{
    internal static class PictureBoxFactory
    {
        public static PictureBox CreatePictureBox(string i_PhotoUrl, int i_PictureBoxDimensions)
        {
            PictureBox photoPictureBox = new PictureBox
            {
                Size = new Size(i_PictureBoxDimensions, i_PictureBoxDimensions),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Top
            };

            if (!string.IsNullOrEmpty(i_PhotoUrl))
            {
                photoPictureBox.LoadAsync(i_PhotoUrl);
            }
            else
            {
                photoPictureBox.Image = BasicFacebookFeatures.Properties.Resources.PlaceholderImage;
            }

            return photoPictureBox;
        }
    }
}