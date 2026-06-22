using System;
using System.Windows.Forms;

namespace ChatLoadBalancerGUI
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new LoadBalancerForm());
        }
    }
}