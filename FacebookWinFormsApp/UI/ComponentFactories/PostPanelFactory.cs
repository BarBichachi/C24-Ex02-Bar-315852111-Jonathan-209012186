﻿using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.UI.ComponentFactories
{
    internal static class PostPanelFactory
    {
        public static Panel CreatePostPanel(Post i_Post, int i_PanelDimensions)
        {
            Panel postPanel = new Panel
            {
                Size = new Size(i_PanelDimensions, i_PanelDimensions),
                BorderStyle = BorderStyle.FixedSingle,
            }; 
            Label postLabel = new Label
            {
                Text = i_Post.Message,
                Dock = DockStyle.Bottom,
                Font = new Font("Arial", 8),
                TextAlign = ContentAlignment.MiddleCenter
            };
            LazyPictureBox pictureBox = PictureBoxFactory.CreateLazyPictureBox(i_Post.PictureURL, i_PanelDimensions);

            postPanel.Controls.Add(pictureBox);
            postPanel.Controls.Add(postLabel);

            return postPanel;
        }
    }
}