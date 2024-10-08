using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
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

            int pictureBoxDimensions = i_PanelDimensions - 25; // Adjust size for label

            PictureBox postPictureBox = new PictureBox
                                            {
                                                Size = new Size(pictureBoxDimensions, pictureBoxDimensions),
                                                Dock = DockStyle.Top,
                                                SizeMode = PictureBoxSizeMode.StretchImage
                                            };

            if (!string.IsNullOrEmpty(i_Post.PictureURL))
            {
                postPictureBox.LoadAsync(i_Post.PictureURL);
            }
            else
            {
                postPictureBox.Image = Properties.Resources.PlaceholderImage; // Use a placeholder image
            }

            Label postLabel = new Label
                                  {
                                      Text = i_Post.Message,
                                      Dock = DockStyle.Bottom,
                                      Font = new Font("Arial", 8), // Directly instantiate the font here
                                      TextAlign = ContentAlignment.MiddleCenter
                                  };

            postPanel.Controls.Add(postPictureBox);
            postPanel.Controls.Add(postLabel);

            return postPanel;
        }
    }
}