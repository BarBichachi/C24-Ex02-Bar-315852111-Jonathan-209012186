using System.Drawing;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.ComponentFactories
{
    internal static class CheckInPanelFactory
    {
        public static Panel CreateCheckInPanel(Checkin i_CheckIn, int i_PanelDimensions)
        {
            Panel checkInPanel = new Panel
            {
                Size = new Size(i_PanelDimensions, 125),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(3)
            };
            Label locationName = new Label
            {
                Text = i_CheckIn.Name,
                Location = new Point(115, 30),
                Font = new Font("Arial", 8, FontStyle.Bold),
                AutoSize = true,
                MaximumSize = new Size(160, 0)
            };
            Label visitedDate = new Label
            {
                Text = @"Visited on " + (i_CheckIn.UpdateTime?.ToString("dd/MM/yyyy") ?? "Unknown date"),
                Location = new Point(115, 60),
                Font = new Font("Arial", 8),
                AutoSize = true
            };
            PictureBox pictureBoxCheckIn = new PictureBox
            {
                Size = new Size(100, 100),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            if (!string.IsNullOrEmpty(i_CheckIn.PictureURL))
            {
                pictureBoxCheckIn.LoadAsync(i_CheckIn.PictureURL);
            }
            else
            {
                pictureBoxCheckIn.Image = Properties.Resources.CheckInPinImage;
            }

            checkInPanel.Controls.Add(pictureBoxCheckIn);
            checkInPanel.Controls.Add(locationName);
            checkInPanel.Controls.Add(visitedDate);

            return checkInPanel;
        }
    }
}