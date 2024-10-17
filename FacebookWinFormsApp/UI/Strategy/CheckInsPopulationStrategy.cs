using System.Collections.Generic;
using System.Windows.Forms;
using BasicFacebookFeatures.UI.ComponentFactories;
using BasicFacebookFeatures.Logic;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.UI.Strategy
{
    internal class CheckInsPopulationStrategy : AbstractLayoutPopulationStrategy<Checkin>
    {
        protected override IEnumerable<Checkin> GetItems(SimplifiedUser i_User) => i_User.Checkins;

        protected override Panel CreatePanel(Checkin i_Item, int i_PanelDimensions) => CheckInPanelFactory.CreateCheckInPanel(i_Item, i_PanelDimensions);
    }
}