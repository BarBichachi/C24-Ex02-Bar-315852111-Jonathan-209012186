using System.Windows.Forms;
using BasicFacebookFeatures.Logic;

namespace BasicFacebookFeatures.Strategy
{
    internal interface ILayoutPopulationStrategy
    {
        void PopulateLayout(SimplifiedUser i_User, FlowLayoutPanel i_FlowLayoutPanel, int i_NumberOfColumns, int i_MaxBoxes);
    }
}
