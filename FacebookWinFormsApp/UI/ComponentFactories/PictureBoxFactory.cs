using System;
using System.Drawing;
using System.Windows.Forms;

namespace BasicFacebookFeatures.UI.ComponentFactories
{
    internal static class PictureBoxFactory
    {
        public static LazyPictureBox CreateLazyPictureBox(string i_PhotoUrl, int i_PictureBoxDimensions)
        {
            LazyPictureBox photoPictureBox = new LazyPictureBox
            {
                Size = new Size(i_PictureBoxDimensions, i_PictureBoxDimensions),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Top
            };

            if (!string.IsNullOrEmpty(i_PhotoUrl))
            {
                photoPictureBox.Load(i_PhotoUrl);
            }
            else
            {
                photoPictureBox.Image = BasicFacebookFeatures.Properties.Resources.PlaceholderImage;
            }

            return photoPictureBox;
        }

    }
}