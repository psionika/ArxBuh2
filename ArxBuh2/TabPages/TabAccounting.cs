﻿using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Eto.Forms;
using Eto.Drawing;

using ArxBuh2.Data;
using ArxBuh2.Data.Entity;

using System.ComponentModel;

namespace ArxBuh2.TabPages
{
    public class TabAccounting : DynamicLayout
    {
        private SelectableFilterCollection<InOut> _collection;
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

        public DynamicLayout Layout1
        {
            get
            {
                var layout = new DynamicLayout();
                
                layout.Add(PanelButton(), yscale: false);
                layout.Add(Grid, yscale: true);
                layout.Add(PanelCalc(), yscale: false);

                return layout;
            }
        }

        private class MyCustomCell : CustomCell
        {
            protected override void OnConfigureCell(CellEventArgs args, Control control)
            {
                var item = (InOut)args.Item;
                var label = (Label)control;

                var col = item.TypeOperation == "Доход" ? Colors.LightGreen : Colors.LightPink;

                if (args.CellState.HasFlag(CellStates.Selected)) col = SystemColors.Highlight;

                label.BackgroundColor = col;

                label.TextColor = args.CellState.HasFlag(CellStates.Selected) ? Colors.White : Colors.Black;

                base.OnConfigureCell(args, control);
            }
        }

        private class MyCustomCellComment : MyCustomCell
        {
            protected override Control OnCreateCell(CellEventArgs args)
            {
                var controlLabel = new Label();
                controlLabel.TextBinding.BindDataContext((InOut m) => m.Comment);

                return controlLabel;
            }

            protected override void OnPaint(CellPaintEventArgs args)
            {
                var item = (InOut)args.Item;
                if (!args.IsEditing)
                    args.Graphics.DrawText(SystemFonts.Default(), Colors.Black, args.ClipRectangle.Location, item.Comment);
            }
        }

        private class MyCustomCellType : MyCustomCell
        {
            protected override Control OnCreateCell(CellEventArgs args)
            {
                var controlLabel = new Label();
                controlLabel.TextBinding.BindDataContext((InOut m) => m.TypeOperation);

                return controlLabel;
            }
            protected override void OnPaint(CellPaintEventArgs args)
            {
                var item = (InOut)args.Item;
                if (!args.IsEditing)
                    args.Graphics.DrawText(SystemFonts.Default(), Colors.Black, args.ClipRectangle.Location, item.TypeOperation);
            }
        }

        private class MyCustomCellCategory : MyCustomCell
        {
            protected override Control OnCreateCell(CellEventArgs args)
            {
                var controlLabel = new Label();
                controlLabel.TextBinding.BindDataContext((InOut m) => m.Category);

                return controlLabel;
            }

            protected override void OnPaint(CellPaintEventArgs args)
            {
                var item = (InOut)args.Item;
                if (!args.IsEditing)
                    args.Graphics.DrawText(SystemFonts.Default(), Colors.Black, args.ClipRectangle.Location, item.Category);
            }
        }

        private class MyCustomCellCreatedAt : MyCustomCell
        {
            protected override Control OnCreateCell(CellEventArgs args)
            {
                var controlLabel = new Label();
                controlLabel.TextBinding.BindDataContext((InOut m) => (m.CreatedAt.Value.ToShortDateString()));

                return controlLabel;
            }

            protected override void OnPaint(CellPaintEventArgs args)
            {
                var item = (InOut)args.Item;
                if (args.IsEditing) return;

                if (item.CreatedAt != null)
                    args.Graphics.DrawText(SystemFonts.Default(), Colors.Black, args.ClipRectangle.Location, item.CreatedAt.Value.ToShortDateString());
            }
        }

        private class MyCustomCellSum : MyCustomCell
        {
            protected override Control OnCreateCell(CellEventArgs args)
            {
                var controlLabel = new Label();
                controlLabel.TextBinding.BindDataContext((InOut m) => m.Sum.ToString());

                return controlLabel;
            }

            protected override void OnPaint(CellPaintEventArgs args)
            {
                var item = (InOut)args.Item;
                if (!args.IsEditing)
                    args.Graphics.DrawText(SystemFonts.Default(), Colors.Black, args.ClipRectangle.Location, item.Sum.ToString());
            }
        }

        private GridView CreateGrid()
        {

            var grid = new GridView
            {
                ID = "mainGrid",
                GridLines = GridLines.Both
            };

            Cell cellType;
            Cell cellCategory;
            Cell cellCreatedAt;
            Cell cellSum;
            Cell cellComment;

            if (Platform.Supports<CustomCell>())
            {
                cellType = new MyCustomCellType();
                cellCategory = new MyCustomCellCategory();
                cellCreatedAt = new MyCustomCellCreatedAt();
                cellSum = new MyCustomCellSum();
                cellComment = new MyCustomCellComment();
            }
            else
            {
                cellType = new TextBoxCell { Binding = Binding.Property<InOut, string>(r => r.TypeOperation) };

                cellCategory = new TextBoxCell { Binding = Binding.Property<InOut, string>(r => r.Category) };

                cellCreatedAt = new TextBoxCell
                {
                    Binding = Binding.Property((InOut m) => m.CreatedAt).Convert(r => ((DateTime)r).ToString("dd.MM.yyyy"), v => (DateTime)Enum.Parse(typeof(InOut), v))
                };

                cellSum = new TextBoxCell
                {
                    Binding = Binding.Property((InOut m) => m.Sum).Convert(r => r.ToString(), v => (int)Enum.Parse(typeof(InOut), v))
                };

                cellComment = new TextBoxCell { Binding = Binding.Property<InOut, string>(r => r.Comment) };
            }

            var colType = new GridColumn
            {
                DataCell = cellType,
                HeaderText = "Type",
                Sortable = true,
                AutoSize = false,
                Width = 80,

            };

            var colCategory = new GridColumn
            {
                DataCell = cellCategory,
                HeaderText = "Type",
                Sortable = true,
                AutoSize = false,
                Width = 80,
            };

            var colCreatedAt = new GridColumn
            {
                DataCell = cellCreatedAt,
                HeaderText = "Type",
                Sortable = true,
                AutoSize = false,
                Width = 80,

            };

            var colSum = new GridColumn
            {
                DataCell = cellSum,
                HeaderText = "Type",
                Sortable = true,
                AutoSize = false,
                Width = 80,
            };
            
            var colComment = new GridColumn
            {
                DataCell = cellComment,
                AutoSize = false,
                HeaderText = "Comment",
                Width = 185,
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
                x.Width = grid.Width - 322;
            };

            grid.ColumnHeaderClick += (sender, e) =>
            {
                Comparison<InOut> ascending = null;
                Comparison<InOut> descending = null;

                switch (e.Column.HeaderText)
                {
                    case "Type":
                        ascending = (x, y) => string.Compare(x.TypeOperation, y.TypeOperation, StringComparison.Ordinal);
                        descending = (x, y) => string.Compare(y.TypeOperation, x.TypeOperation, StringComparison.Ordinal);
                        break;
                    case "Category":
                        ascending = (x, y) => string.Compare(x.Category, y.Category, StringComparison.Ordinal);
                        descending = (x, y) => string.Compare(y.Category, x.Category, StringComparison.Ordinal);
                        break;
                    case "CreatedAt":
                        ascending = (x, y) => x.CreatedAt.Value.CompareTo(y.CreatedAt.Value);
                        descending = (x, y) => y.CreatedAt.Value.CompareTo(x.CreatedAt.Value);
                        break;
                    case "Sum":
                        ascending = (x, y) => x.Sum.CompareTo(y.Sum);
                        descending = (x, y) => y.Sum.CompareTo(x.Sum);
                        break;
                    case "Comment":
                        ascending = (x, y) => string.Compare(x.Comment, y.Comment, StringComparison.Ordinal);
                        descending = (x, y) => string.Compare(y.Comment, x.Comment, StringComparison.Ordinal);
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

        private ContextMenu _gridContextMenu;

        private ContextMenu CreateGridContextMenu()
        {
            if (_gridContextMenu != null)
                return _gridContextMenu;

            _gridContextMenu = new ContextMenu();

            _gridContextMenu.Items.Add(new ButtonMenuItem { Text = "Редактировать" });
            _gridContextMenu.Items.Add(new ButtonMenuItem { Text = "Удалить" });

            return _gridContextMenu;
        }


        private Panel PanelButton()
        {
            var pickerDown = new DateTimePicker { Width = 110, Enabled = true, Value = DateTime.Now.AddYears(-1) };
            var pickerUp = new DateTimePicker { Width = 110, Enabled = true, Value = DateTime.Now.AddYears(1) };
            var filterComboBox = new ComboBox { Items = { "Доход", "Расход" } };

            pickerDown.ValueChanged += (sender, e) => Filter(sender, e, (DateTime)pickerDown.Value, (DateTime)pickerUp.Value, filterComboBox.Text);
            pickerUp.ValueChanged += (sender, e) => Filter(sender, e, (DateTime)pickerDown.Value, (DateTime)pickerUp.Value, filterComboBox.Text);
            filterComboBox.TextChanged += (sender, e) => Filter(sender, e, (DateTime)pickerDown.Value, (DateTime)pickerUp.Value, filterComboBox.Text);

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
                    new Button { Text = "Доход", Command = AddEdit("Доход") },
                            new Button { Text = "Расход", Command = AddEdit("Расход") },
                            filterComboBox,
                            new Label { Text = "С" },
                            pickerDown,
                            new Label { Text = "по" },
                            pickerUp,
                            buttonClearFilter,
                            null,
                            SettingsButton(),
                            ExitButton()
                }
            };

            var panel = new Panel
            {
                Content = layout1
            };

            return panel;
        }

        private Command AddEdit(string title)
        {
            var add = new Command();

            add.Executed += (sender, e) =>
            {
                var panel = new _Forms.AddEditInOut(title);

                if (panel.ShowModal())
                {
                    Database.Create(panel.NewItem);
                    _collection.Add(panel.NewItem);
                }
            };

            return add;
        }

        private void Filter(object sender, EventArgs e, DateTime down, DateTime up, string substring)
        {
            _collection.Filter = x => ((x.CreatedAt.Value.CompareTo(new DateTime(down.Year, down.Month, down.Day, 0, 0, 0)) >= 0)
                                        && (x.CreatedAt.Value.CompareTo((new DateTime(up.Year, up.Month, up.Day + 1)).AddMilliseconds(-1)) <= 0)

                                        && (x.TypeOperation.Contains(substring)
                                           || (x.Comment != null && x.Comment.Contains(substring))
                                           || (x.Category != null && x.Category.Contains(substring))));

        }

        private Control SettingsButton()
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

        private Control ExitButton()
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

        private ContextMenu _settingsMenu;

        private ContextMenu CreateSettingsMenu()
        {
            if (_settingsMenu != null)
                return _settingsMenu;

            _settingsMenu = new ContextMenu();

            _settingsMenu.Items.Add(new ButtonMenuItem { Text = "О программе" });
            _settingsMenu.Items.AddSeparator();

            _settingsMenu.Items.Add(new ButtonMenuItem { Text = "Категории доходов и расходов" });
            _settingsMenu.Items.AddSeparator();

            _settingsMenu.Items.Add(new ButtonMenuItem { Text = "Выгрузить в CSV" });

            _settingsMenu.Items.AddSeparator();
            var subMenuSettings = _settingsMenu.Items.GetSubmenu("Настройки");
            subMenuSettings.Items.Add(new ButtonMenuItem { Text = "Резервное копирование" });
            subMenuSettings.Items.Add(new ButtonMenuItem { Text = "Шифрование" });
            subMenuSettings.Items.Add(new ButtonMenuItem { Text = "Автоматическое обновление" });



            return _settingsMenu;
        }


        private ResultModel _resultModel;

        private Panel PanelCalc()
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
            private string _result;
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

            private void OnPropertyChanged([CallerMemberName] string memberName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
