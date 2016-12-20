using System;
using Eto.Drawing;
using Eto.Forms;
using System.ComponentModel;
using System.Collections.ObjectModel;

using System.Collections.Generic;

using ArxBuh2.Data.Entity;
using ArxBuh2._Controls;

namespace ArxBuh2._Forms
{

    public class AddEditInOut : Dialog<bool>
    {
        InOut newItem;

        public InOut NewItem
        {
            get { return newItem; }
            set
            {
                newItem = value;
            }
        }

        public AddEditInOut(string title)
        {
            Title = title;

            NewItem = new InOut();
            this.DataContext = NewItem;


            var comboType = new ComboBox { ReadOnly = true };
            var comboCategory = new CategoryComboBox();
            // buttons
            DefaultButton = new Button { Text = "OK" };
            AbortButton = new Button { Text = "C&ancel" };
            AbortButton.Click += (sender, e) =>
            {

                Close(false);
            };

            comboType.Items.Add("Доход");
            comboType.Items.Add("Расход");

            comboType.Text = title;

            var dateTimePickerCreatedAt = new DateTimePicker(); dateTimePickerCreatedAt.Value = DateTime.Now;

            var textBoxSum = new TextBox();

            var textBoxComment = new TextBox();
            textBoxComment.Size = new Size(300, 200);
            textBoxComment.TextBinding.BindDataContext((InOut m) => m.Comment);

            var layout = new TableLayout
            {
                Padding = new Padding(10),
                Spacing = new Size(5, 5)
            };

            layout.Rows.Add(new TableRow(null, new Label { Text = "Type" }, comboType));
            layout.Rows.Add(new TableRow(null, new Label { Text = "Category" }, comboCategory));
            layout.Rows.Add(new TableRow(null, new Label { Text = "Sum" }, textBoxSum));
            layout.Rows.Add(new TableRow(null, new Label { Text = "Date" }, dateTimePickerCreatedAt));
            layout.Rows.Add(new TableRow(null, new Label { Text = "Comment" }, textBoxComment));

            var buttons = new TableLayout { Rows = { new TableRow(null, DefaultButton, AbortButton) }, Spacing = new Size(5, 5) };

            Content = new StackLayout
            {
                Padding = new Padding(10),
                Spacing = 5,
                Items =
                {
                    new StackLayoutItem(layout, HorizontalAlignment.Left),

                    new StackLayoutItem(buttons, HorizontalAlignment.Center)
                }
            };

            DefaultButton.Click += (sender, e) =>
            {
                NewItem.TypeOperation = comboType.SelectedValue.ToString();
                NewItem.Category = comboCategory.Text;
                NewItem.CreatedAt = dateTimePickerCreatedAt.Value;
                NewItem.Comment = textBoxComment.Text;

                try
                {
                    var sum = Convert.ToDecimal(textBoxSum.Text);
                    NewItem.Sum = sum;
                }
                catch
                {
                    NewItem.Sum = 0;
                }

                Close(true);
            };
        }


    }
}
