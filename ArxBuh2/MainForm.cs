using Eto.Forms;
using Eto.Drawing;

using ArxBuh2.TabPages;

namespace ArxBuh2
{
	public class MainForm : Form
	{
		public MainForm()
		{
			Title = "ArxBuh2: Personal finance";

			ClientSize = new Size(900, 612);
			MinimumSize = new Size(600, 460);

			var tabControl = new TabControl();

			tabControl.Pages.Add(new TabPage { Text = "Учёт", Content = (new TabAccounting()).Layout1 });

			tabControl.Pages.Add(new TabPage { Text = "Бюджет" });
			tabControl.Pages.Add(new TabPage { Text = "Цели" });
			tabControl.Pages.Add(new TabPage { Text = "Отчёты" });

			Content = tabControl;
		}

	}
}