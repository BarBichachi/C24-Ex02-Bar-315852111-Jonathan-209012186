using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class LikedPagePanelFactory
    {
        public static Panel CreateLikedPagePanel(Page i_Page, int i_PanelDimensions)
        {
            Panel likedPagePanel = new Panel
                                       {
                                           Size = new Size(i_PanelDimensions, i_PanelDimensions),
                                           BorderStyle = BorderStyle.FixedSingle,
                                       };

            int pictureBoxDimensions = i_PanelDimensions - 20; // Adjust size for label

            PictureBox pictureBox = new PictureBox
                                        {
                                            Size = new Size(pictureBoxDimensions, pictureBoxDimensions),
                                            Dock = DockStyle.Top,
                                            SizeMode = PictureBoxSizeMode.StretchImage
                                        };

            if (!string.IsNullOrEmpty(i_Page.PictureURL))
            {
                pictureBox.LoadAsync(i_Page.PictureURL);
            }
            else
            {
                pictureBox.Image = Properties.Resources.PlaceholderImage;
            }

            Label nameLabel = new Label
                                  {
                                      Text = i_Page.Name,
                                      Dock = DockStyle.Bottom,
                                      Font = new Font("Arial", 8), // Directly instantiate the font here
                                      TextAlign = ContentAlignment.MiddleCenter
                                  };

            likedPagePanel.Controls.Add(pictureBox);
            likedPagePanel.Controls.Add(nameLabel);

            return likedPagePanel;
        }
    }
}