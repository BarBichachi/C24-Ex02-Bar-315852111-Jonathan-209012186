using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class LikedPagePanelFactory
    {
        public static Panel CreateLikedPagePanel(Page i_Page, int i_PanelDimensions)
        {
            int pictureBoxDimensions = i_PanelDimensions - 20;
            Panel likedPagePanel = new Panel
            {
                Size = new Size(i_PanelDimensions, i_PanelDimensions),
                BorderStyle = BorderStyle.FixedSingle,
            };
            Label nameLabel = new Label
            {
                Text = i_Page.Name,
                Dock = DockStyle.Bottom,
                Font = new Font("Arial", 8),
                TextAlign = ContentAlignment.MiddleCenter
            };
            PictureBox pictureBox = PhotoBoxFactory.CreatePictureBox(i_Page.PictureNormalURL, i_PanelDimensions);

            likedPagePanel.Controls.Add(pictureBox);
            likedPagePanel.Controls.Add(nameLabel);

            return likedPagePanel;
        }
    }
}