using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class FavoriteTeamPanelFactory
    {
        public static Panel CreateFavoriteTeamPanel(Page i_FavoriteTeam, int i_PanelDimensions)
        {
            int pictureBoxDimensions = i_PanelDimensions - 20;
            Panel teamPanel = new Panel
            {
                Size = new Size(i_PanelDimensions, i_PanelDimensions),
                BorderStyle = BorderStyle.FixedSingle,
            };
            Label teamLabel = new Label
            {
                Text = i_FavoriteTeam.Name,
                Dock = DockStyle.Bottom,
                Font = new Font("Arial", 8),
                TextAlign = ContentAlignment.MiddleCenter
            };
            PictureBox pictureBox = PhotoBoxFactory.CreatePictureBox(i_FavoriteTeam.PictureNormalURL, i_PanelDimensions);

            teamPanel.Controls.Add(pictureBox);
            teamPanel.Controls.Add(teamLabel);

            return teamPanel;
        }
    }
}