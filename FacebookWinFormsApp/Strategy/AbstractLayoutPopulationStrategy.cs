using System;
using System.Collections.Generic;
using System.Windows.Forms;
using BasicFacebookFeatures.Logic;

namespace BasicFacebookFeatures.Strategy
{
    internal abstract class AbstractLayoutPopulationStrategy<T> : ILayoutPopulationStrategy
    {
        public void PopulateLayout(SimplifiedUser i_User, FlowLayoutPanel i_FlowLayoutPanel, int i_NumberOfColumns, int i_MaxBoxes)
        {
            int panelDimensions = (i_FlowLayoutPanel.ClientSize.Width / i_NumberOfColumns) - 10;

            try
            {
                foreach (T item in GetItems(i_User))
                {
                    Panel itemPanel = CreatePanel(item, panelDimensions);

                    i_FlowLayoutPanel.Controls.Add(itemPanel);

                    if (i_FlowLayoutPanel.Controls.Count == i_MaxBoxes)
                    {
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApplicationException($"Populating the {typeof(T).Name}s has failed! >\n{e.Message}");
            }
        }

        protected abstract IEnumerable<T> GetItems(SimplifiedUser i_User);

        protected abstract Panel CreatePanel(T i_Item, int i_PanelDimensions);
    }
}
