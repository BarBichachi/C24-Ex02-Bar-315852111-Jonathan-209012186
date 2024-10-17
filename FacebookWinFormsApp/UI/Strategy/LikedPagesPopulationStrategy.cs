using System.Collections.Generic;
using System.Windows.Forms;
using BasicFacebookFeatures.UI.ComponentFactories;
using BasicFacebookFeatures.Logic;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.UI.Strategy
{
    internal class LikedPagesPopulationStrategy : AbstractLayoutPopulationStrategy<Page>
    {
        protected override IEnumerable<Page> GetItems(SimplifiedUser i_User) => i_User.LikedPages;

        protected override Panel CreatePanel(Page i_Item, int i_PanelDimensions) => LikedPagePanelFactory.CreateLikedPagePanel(i_Item, i_PanelDimensions);
    }
}