using System.Linq;

using Eto.Forms;

using ArxBuh2.Data;
using ArxBuh2.Data.Entity;

namespace ArxBuh2._Controls
{
    internal class CategoryComboBox : ComboBox
    {
        public CategoryComboBox()
        {
            var list = Database.GetMany<Category>();

            foreach (var item in list)
            {
                Items.Add(item.Name, item.ObjectId.ToString());
            }

            if (list.Count() > 0) SelectedIndex = 0;

            ReadOnly = true;
        }
    }
}
