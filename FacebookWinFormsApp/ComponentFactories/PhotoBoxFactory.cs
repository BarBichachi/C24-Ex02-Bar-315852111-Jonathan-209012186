using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class PhotoBoxFactory
    {
        public static PictureBox CreatePictureBox(Photo i_Photo, int i_PictureBoxDimensions)
        {
            PictureBox photoPictureBox = new PictureBox
            {
                Size = new Size(i_PictureBoxDimensions, i_PictureBoxDimensions),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            if (!string.IsNullOrEmpty(i_Photo.PictureNormalURL))
            {
                photoPictureBox.LoadAsync(i_Photo.PictureNormalURL);
            }
            else
            {
                photoPictureBox.Image = Properties.Resources.PlaceholderImage;
            }

            return photoPictureBox;
        }
    }
}