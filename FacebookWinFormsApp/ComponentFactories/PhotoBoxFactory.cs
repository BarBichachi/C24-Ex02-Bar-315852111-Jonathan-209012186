using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class PhotoBoxFactory
    {
        public static PictureBox CreatePictureBox(string i_Photo, int i_PictureBoxDimensions)
        {
            PictureBox photoPictureBox = new PictureBox
            {
                Size = new Size(i_PictureBoxDimensions, i_PictureBoxDimensions),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Top
            };

            if (!string.IsNullOrEmpty(i_Photo))
            {
                photoPictureBox.LoadAsync(i_Photo);
            }
            else
            {
                photoPictureBox.Image = Properties.Resources.PlaceholderImage;
            }

            return photoPictureBox;
        }
    }
}