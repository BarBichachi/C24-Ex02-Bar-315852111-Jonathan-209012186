using System.Collections.Generic;
using System.Windows.Forms;
using BasicFacebookFeatures.ComponentFactories;
using BasicFacebookFeatures.Logic;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.Strategy
{
    internal class PostsPopulationStrategy : AbstractLayoutPopulationStrategy<Post>
    {
        protected override IEnumerable<Post> GetItems(SimplifiedUser i_User) => i_User.Posts;

        protected override Panel CreatePanel(Post i_Item, int i_PanelDimensions) => PostPanelFactory.CreatePostPanel(i_Item, i_PanelDimensions);
    }
}