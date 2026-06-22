using System;
using System.Windows.Forms;

namespace ChatServerGUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new ChatServerForm());
        }
    }
}