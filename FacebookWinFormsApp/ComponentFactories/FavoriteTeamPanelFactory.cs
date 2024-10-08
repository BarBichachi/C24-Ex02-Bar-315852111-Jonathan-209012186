using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class FavoriteTeamPanelFactory
    {
        public static Panel CreateFavoriteTeamPanel(Page i_FavoriteTeam, int i_PanelDimensions)
        {
            Panel teamPanel = new Panel
                                  {
                                      Size = new Size(i_PanelDimensions, i_PanelDimensions),
                                      BorderStyle = BorderStyle.FixedSingle,
                                  };

            int pictureBoxDimensions = i_PanelDimensions - 20;
            PictureBox teamPictureBox = new PictureBox
                                            {
                                                Size = new Size(pictureBoxDimensions, pictureBoxDimensions),
                                                Dock = DockStyle.Top,
                                                SizeMode = PictureBoxSizeMode.StretchImage
                                            };

            if (!string.IsNullOrEmpty(i_FavoriteTeam.PictureURL))
            {
                teamPictureBox.LoadAsync(i_FavoriteTeam.PictureURL);
            }
            else
            {
                teamPictureBox.Image = Properties.Resources.PlaceholderImage;
            }

            Label teamLabel = new Label
                                  {
                                      Text = i_FavoriteTeam.Name,
                                      Dock = DockStyle.Bottom,
                                      Font = new Font("Arial", 8), // Implemented directly
                                      TextAlign = ContentAlignment.MiddleCenter
                                  };

            teamPanel.Controls.Add(teamPictureBox);
            teamPanel.Controls.Add(teamLabel);

            return teamPanel;
        }
    }
}