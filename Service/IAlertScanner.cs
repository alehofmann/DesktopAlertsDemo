
namespace Tikisoft.DesktopAlertsDemo.Service
{
    interface IAlertScanner
    {
        bool StartScan();
        void StopScan();
        string LastError { get ;}
    }
}
