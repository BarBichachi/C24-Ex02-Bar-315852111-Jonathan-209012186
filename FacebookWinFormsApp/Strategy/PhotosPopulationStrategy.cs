using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using BasicFacebookFeatures.ComponentFactories;
using BasicFacebookFeatures.Logic;
using FacebookWrapper.ObjectModel;

namespace BasicFacebookFeatures.Strategy
{
    internal class PhotosPopulationStrategy : AbstractLayoutPopulationStrategy<Photo>
    {
        protected override IEnumerable<Photo> GetItems(SimplifiedUser i_User) => i_User.Albums.SelectMany(i_Album => i_Album.Photos);

        protected override Panel CreatePanel(Photo i_Item, int i_PanelDimensions) => PhotoPanelFactory.CreatePhotoPanel(i_Item, i_PanelDimensions);
    }
}
