using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Eto;
using Eto.Forms;
using Eto.Drawing;


using ArxBuh2.Data;
using ArxBuh2.Data.Entity;

using ArxBuh2.TabPages;
using System.ComponentModel;

namespace ArxBuh2.TabPages
{
    public class TabAccounting : DynamicLayout
    {
        SelectableFilterCollection<InOut> _collection;
        private GridView _grid;
        public GridView Grid
        {
            get
            {
                _grid = CreateGrid();
                _collection = new SelectableFilterCollection<InOut>(_grid, Database.GetMany<InOut>());
                _grid.DataStore = _collection;

                return _grid;
            }
        }        

        public TabAccounting()
        {

        }

        public DynamicLayout Layout1
        {
            get
            {
                var layout = new DynamicLayout();
                
                layout.Add(panelButton(), yscale: false);
                layout.Add(Grid, yscale: true);
                layout.Add(panelCalc(), yscale: false);

                return layout;
            }
        }


        GridView CreateGrid()
        {
            var grid = new GridView();
            grid.ID = "mainGrid";
            grid.GridLines = GridLines.Both;

            var colType = new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<InOut, string>(r => r.TypeOperation) },
                HeaderText = "Type",
                AutoSize = false,
                Width = 80,
                Sortable = true,
            };

            var colCategory = new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<InOut, string>(r => r.Category) },
                HeaderText = "Category",
                AutoSize = false,
                Width = 80,
                Sortable = true
            };

            var colCreatedAt = new GridColumn
            {
                DataCell = new TextBoxCell()
                {
                    Binding = Binding.Property((InOut m) => m.CreatedAt).Convert(r => ((DateTime)r).ToString("dd.MM.yyyy"), v => (DateTime)Enum.Parse(typeof(InOut), v))
                },
                HeaderText = "CreatedAt",
                AutoSize = false,
                Width = 80,
                Sortable = true

            };

            var colSum = new GridColumn
            {
                DataCell = new TextBoxCell()
                {
                    Binding = Binding.Property((InOut m) => m.Sum).Convert(r => r.ToString(), v => (int)Enum.Parse(typeof(InOut), v))
                },
                Width = 50,
                AutoSize = false,
                HeaderText = "Sum",
                Sortable = true

            };

            var colComment = new GridColumn
            {
                DataCell = new TextBoxCell { Binding = Binding.Property<InOut, string>(r => r.Comment) },
                AutoSize = false,
                HeaderText = "Comment",
                Width = 205,
                Sortable = true

            };

            grid.CellFormatting += (sender, e) =>
            {
                var item = (InOut)e.Item;

                if (item != null)
                {

                    e.BackgroundColor = item.TypeOperation == "Доход" ? Colors.LightGreen : Colors.LightPink;

                    e.Column.Sortable = true;
                }


            };

            grid.Columns.Add(colType);
            grid.Columns.Add(colCategory);
            grid.Columns.Add(colCreatedAt);
            grid.Columns.Add(colSum);
            grid.Columns.Add(colComment);


            grid.SizeChanged += (sender, e) =>
            {
                var x = grid.Columns.First(p => p.HeaderText == "Comment");
                x.Width = grid.Width - 295;
            };

            grid.ColumnHeaderClick += (sender, e) =>
            {
                Comparison<InOut> ascending = null;
                Comparison<InOut> descending = null;

                switch (e.Column.HeaderText)
                {
                    case "Type":
                        ascending = (x, y) => (string.Compare(x.TypeOperation, y.TypeOperation, StringComparison.Ordinal));
                        descending = (x, y) => (string.Compare(y.TypeOperation, x.TypeOperation, StringComparison.Ordinal));
                        break;
                    case "Category":
                        ascending = (x, y) => (string.Compare(x.Category, y.Category, StringComparison.Ordinal));
                        descending = (x, y) => (string.Compare(y.Category, x.Category, StringComparison.Ordinal));
                        break;
                    case "CreatedAt":
                        ascending = (x, y) => (x.CreatedAt.Value.CompareTo(y.CreatedAt.Value));
                        descending = (x, y) => (y.CreatedAt.Value.CompareTo(x.CreatedAt.Value));
                        break;
                    case "Sum":
                        ascending = (x, y) => (x.Sum.CompareTo(y.Sum));
                        descending = (x, y) => (y.Sum.CompareTo(x.Sum));
                        break;
                    case "Comment":
                        ascending = (x, y) => (string.Compare(x.Comment, y.Comment, StringComparison.Ordinal));
                        descending = (x, y) => (string.Compare(y.Comment, x.Comment, StringComparison.Ordinal));
                        break;
                }

                _collection.Sort = (_collection.Sort == ascending) ? descending : ascending;
            };

            grid.CellDoubleClick += (sender, e) =>
            {
                var menu = CreateGridContextMenu();
                menu.Show(grid);
            };

            return grid;
        }

        ContextMenu gridContextMenu;
        ContextMenu CreateGridContextMenu()
        {
            if (gridContextMenu != null)
                return gridContextMenu;

            gridContextMenu = new ContextMenu();

            gridContextMenu.Items.Add(new ButtonMenuItem { Text = "Удалить" });

            return gridContextMenu;
        }


        Panel panelButton()
        {
            var pickerDown = new DateTimePicker { Width = 110, Enabled = true, Value = DateTime.Now.AddYears(-1) };
            var pickerUp = new DateTimePicker { Width = 110, Enabled = true, Value = DateTime.Now.AddYears(1) };
            var filterComboBox = new ComboBox { Items = { "Доход", "Расход" } };

            pickerDown.ValueChanged += ((sender, e) => filter(sender, e, (System.DateTime)pickerDown.Value, (System.DateTime)pickerUp.Value, filterComboBox.Text));
            pickerUp.ValueChanged += ((sender, e) => filter(sender, e, (System.DateTime)pickerDown.Value, (System.DateTime)pickerUp.Value, filterComboBox.Text));
            filterComboBox.TextChanged += ((sender, e) => filter(sender, e, (System.DateTime)pickerDown.Value, (System.DateTime)pickerUp.Value, filterComboBox.Text));

            var buttonClearFilter = new Button { Text = "Отчистить фильтр" };
            buttonClearFilter.Click += (sender, e) =>
            {
                pickerDown.Value = DateTime.Now.AddYears(-1);
                pickerUp.Value = DateTime.Now.AddYears(1);
                filterComboBox.Text = "";

                _collection.Filter = null;
            };

            var layout1 = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                Spacing = 3,
                Padding = new Padding(1),
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,

                Items =
                {
                    new Button { Text = "Доход" },
                            new Button { Text = "Расход" },
                            filterComboBox,
                            new Label { Text = "С" },
                            pickerDown,
                            new Label { Text = "по" },
                            pickerUp,
                            buttonClearFilter,
                            null,
                            settingsButton(),
                            exitButton()
                }
            };

            var panel = new Panel
            {
                Content = layout1
            };

            return panel;
        }

        private void filter(object sender, EventArgs e, DateTime down, DateTime up, string substring)
        {
            _collection.Filter = x => ((x.CreatedAt.Value.CompareTo(new DateTime(down.Year, down.Month, down.Day, 0, 0, 0)) >= 0)
                                        && (x.CreatedAt.Value.CompareTo((new DateTime(up.Year, up.Month, up.Day + 1)).AddMilliseconds(-1)) <= 0)

                                        && (x.TypeOperation.Contains(substring)
                                           || (x.Comment != null && x.Comment.Contains(substring))
                                           || (x.Category != null && x.Category.Contains(substring))));

        }

        Control settingsButton()
        {
            var icon = new Bitmap(Bitmap.FromResource("ArxBuh2.Resources.gear_wheel_32x32.png"), 16, 16, ImageInterpolation.Default);

            var button = new Button
            {
                Image = icon,
                ToolTip = "Настройки",
                Width = 22
            };
            button.Click += (sender, e) =>
            {
                var menu = CreateSettingsMenu();
                menu.Show(button);
            };
            return button;
        }

        Control exitButton()
        {
            var icon = new Bitmap(Bitmap.FromResource("ArxBuh2.Resources.door_32x32.png"), 16, 16, ImageInterpolation.Default);

            var button = new Button
            {
                Image = icon,
                ToolTip = "Выход",
                Width = 22
            };

            button.Click += (sender, e) =>
            {
                Environment.Exit(0);
            };

            return button;
        }

        ContextMenu settingsMenu;
        ContextMenu CreateSettingsMenu()
        {
            if (settingsMenu != null)
                return settingsMenu;

            settingsMenu = new ContextMenu();

            settingsMenu.Items.Add(new ButtonMenuItem { Text = "О программе" });
            settingsMenu.Items.AddSeparator();

            settingsMenu.Items.Add(new ButtonMenuItem { Text = "Категории доходов и расходов" });
            settingsMenu.Items.AddSeparator();

            settingsMenu.Items.Add(new ButtonMenuItem { Text = "Выгрузить в CSV" });

            settingsMenu.Items.AddSeparator();
            var subMenuSettings = settingsMenu.Items.GetSubmenu("Настройки");
            subMenuSettings.Items.Add(new ButtonMenuItem { Text = "Резервное копирование" });
            subMenuSettings.Items.Add(new ButtonMenuItem { Text = "Шифрование" });
            subMenuSettings.Items.Add(new ButtonMenuItem { Text = "Автоматическое обновление" });



            return settingsMenu;
        }


        ResultModel _resultModel;
        Panel panelCalc()
        {
            var label = new Label();
            label.TextBinding.BindDataContext((ResultModel m) => m.Calc);
            DataContext = _resultModel;

            var panel = new Panel
            {
                Size = new Size(0, 50),

                ID = "PanelCalc",

                Content = new StackLayout
                {
                    Padding = new Padding(10),
                    Spacing = 5,
                    Items =
                    {
                        new StackLayoutItem( label, HorizontalAlignment.Left),
                    }
                }
            };

            return panel;
        }
        public class ResultModel : INotifyPropertyChanged
        {
            string _result;
            public string Calc
            {
                get { return _result; }
                set
                {
                    if (_result != value)
                    {
                        _result = value;
                        OnPropertyChanged();
                    }
                }
            }

            void OnPropertyChanged([CallerMemberName] string memberName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
