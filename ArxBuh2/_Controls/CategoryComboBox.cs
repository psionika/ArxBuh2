using System;
using System.Collections.Generic;
using System.Linq;

using Eto.Forms;
using Eto.Drawing;

using System.Collections.ObjectModel;

using ArxBuh2.Data;
using ArxBuh2.Data.Entity;

namespace ArxBuh2._Controls
{
    class CategoryComboBox : ComboBox
    {
        public CategoryComboBox()
        {
            var list = Database.GetMany<Category>();

            foreach (var item in list)
            {
                Items.Add(item.Name, item.ObjectId.ToString());
            }

            if (list != null && list.Count() > 0) SelectedIndex = 0;

            ReadOnly = true;
        }
    }
}
