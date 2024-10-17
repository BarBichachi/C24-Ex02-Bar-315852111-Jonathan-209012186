using System.Collections.Generic;
using System.Windows.Forms;
using BasicFacebookFeatures.UI.ComponentFactories;
using BasicFacebookFeatures.Logic;

using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.UI.Strategy
{
    internal class FriendsPopulationStrategy : AbstractLayoutPopulationStrategy<User>
    {
        protected override IEnumerable<User> GetItems(SimplifiedUser i_User) => i_User.Friends;

        protected override Panel CreatePanel(User i_Item, int i_PanelDimensions) => FriendPanelFactory.CreateFriendPanel(i_Item, i_PanelDimensions);
    }
}