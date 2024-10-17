using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.UI.ComponentFactories
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
            Label friendLabel = new Label
            {
                Text = i_Friend.Name,
                Dock = DockStyle.Bottom,
                Font = new Font("Arial", 8),
                TextAlign = ContentAlignment.MiddleCenter
            };
            PictureBox pictureBox = PictureBoxFactory.CreatePictureBox(i_Friend.PictureNormalURL, i_PanelDimensions);

            friendPanel.Controls.Add(pictureBox);
            friendPanel.Controls.Add(friendLabel);

            return friendPanel;
        }
    }
}