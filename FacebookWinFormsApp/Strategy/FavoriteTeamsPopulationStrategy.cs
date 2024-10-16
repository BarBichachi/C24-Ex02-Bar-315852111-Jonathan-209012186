using System.Collections.Generic;
using System.Windows.Forms;
using BasicFacebookFeatures.ComponentFactories;
using BasicFacebookFeatures.Logic;

using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.Strategy
{
    internal class FavoriteTeamsPopulationStrategy : AbstractLayoutPopulationStrategy<Page>
    {
        protected override IEnumerable<Page> GetItems(SimplifiedUser i_User) => i_User.FavoriteTeams;

        protected override Panel CreatePanel(Page i_Item, int i_PanelDimensions) => FavoriteTeamPanelFactory.CreateFavoriteTeamPanel(i_Item, i_PanelDimensions);
    }
}