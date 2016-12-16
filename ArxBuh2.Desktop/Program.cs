using System;
using Eto;
using Eto.Forms;

namespace ArxBuh2.Desktop
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            new Application(Platform.Detect).Run(new MainForm());
        }
    }
}
