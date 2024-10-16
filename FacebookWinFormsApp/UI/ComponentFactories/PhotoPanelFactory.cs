﻿using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.UI.ComponentFactories
{
    internal static class PhotoPanelFactory
    {
        public static Panel CreatePhotoPanel(Photo i_Photo, int i_PanelDimensions)
        {
            Panel photoPanel = new Panel
            {
                Size = new Size(i_PanelDimensions, i_PanelDimensions),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(3)
            };
            LazyPictureBox photoPictureBox = PictureBoxFactory.CreateLazyPictureBox(i_Photo.PictureNormalURL, i_PanelDimensions);

            photoPanel.Controls.Add(photoPictureBox);

            return photoPanel;
        }
    }
}
