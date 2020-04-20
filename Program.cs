using System;
using System.Windows.Forms;
using Autofac;
using Tikisoft.DesktopAlertsDemo.Infrastructure;
using Tikisoft.DesktopAlertsDemo.Service;
using Tikisoft.DesktopAlertsDemo.SystraySupport;

namespace Tikisoft.DesktopAlertsDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var builder = new ContainerBuilder();
            builder.RegisterType<NaudioPlayer>().As<IAudioPlayer>();
            builder.RegisterType<TulpepPopupEngine>().As<IPopupEngine>();
            builder.RegisterType<LocalPathAlertScanner>().As<IAlertScanner>();
            
            var container = builder.Build();
            var scanner = container.Resolve<IAlertScanner>();

            try
            {
                if (!scanner.StartScan())
                {
                    MessageBox.Show(scanner.LastError, "Cannot Start Program", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    return;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Fatal Error Starting Program", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            
            
            using(ProcessIcon pi=new ProcessIcon())
            {
                pi.Display();
                Application.Run();
            }
            
        }
    }
}
