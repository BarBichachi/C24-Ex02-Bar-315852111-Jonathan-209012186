using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class FriendPanelFactory
    {
        public static Panel CreateFriendPanel(User i_Friend, int i_PanelDimensions)
        {
            Panel friendPanel = new Panel
                                    {
                                        Size = new Size(i_PanelDimensions, i_PanelDimensions),
                                        BorderStyle = BorderStyle.FixedSingle,
                                    };

            int pictureBoxDimensions = i_PanelDimensions - 25;

            PictureBox friendPictureBox = new PictureBox
                                              {
                                                  Size = new Size(pictureBoxDimensions, pictureBoxDimensions),
                                                  Dock = DockStyle.Top,
                                                  SizeMode = PictureBoxSizeMode.StretchImage
                                              };

            if (!string.IsNullOrEmpty(i_Friend.PictureNormalURL))
            {
                friendPictureBox.LoadAsync(i_Friend.PictureNormalURL);
            }
            else
            {
                friendPictureBox.Image = Properties.Resources.PlaceholderImage;
            }

            Label friendLabel = new Label
                                    {
                                        Text = i_Friend.Name,
                                        Dock = DockStyle.Bottom,
                                        Font = new Font("Arial", 8),
                                        TextAlign = ContentAlignment.MiddleCenter
                                    };

            friendPanel.Controls.Add(friendPictureBox);
            friendPanel.Controls.Add(friendLabel);

            return friendPanel;
        }
    }
}