using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Tikisoft.DesktopAlertsDemo.Infrastructure;
using Tikisoft.DesktopAlertsDemo.Model;


//todo: Que las images y audio sean un subfolder del scanpath

namespace Tikisoft.DesktopAlertsDemo.Service
{
    class LocalPathAlertScanner : IAlertScanner
    {
        IPopupEngine _popupEngine;
        Timer _timer;
        string _scanPath;
        string _lastError;

        public string LastError => (_lastError);
        public LocalPathAlertScanner(IPopupEngine popupEngine)
        {
            _popupEngine = popupEngine;

            _scanPath = ConfigurationManager.AppSettings["AlertsPath"];

            _timer = new Timer();
            _timer.Interval = 200;
            _timer.Tick += new EventHandler(timerTick);
        }

        private bool HasWriteAccessToFolder(string folderPath)
        {
            try
            {
                // Attempt to get a list of security permissions from the folder. 
                // This will raise an exception if the path is read only or do not have access to view the permissions. 
                System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(folderPath);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        private bool SetupAlertsPath()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select Alerts Folder";
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    try
                    {
                        var imagesPath = Path.Combine(fbd.SelectedPath, "images");
                        var audioPath = Path.Combine(fbd.SelectedPath, "audio");
                        if (!Directory.Exists(imagesPath)) { Directory.CreateDirectory(imagesPath); }
                        if (!Directory.Exists(audioPath)) { Directory.CreateDirectory(audioPath); }

                        foreach (FileInfo file in new DirectoryInfo(fbd.SelectedPath).GetFiles())
                        { file.Delete(); }

                        string sourcePath;
                        sourcePath =
                            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "SampleAlertFolder");

                        foreach (FileInfo file in new DirectoryInfo(sourcePath).GetFiles())
                        {
                            File.Copy(file.FullName, Path.Combine(fbd.SelectedPath, file.Name), true);
                        }

                        sourcePath =
                            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "SampleAlertFolder/images");

                        foreach (FileInfo file in new DirectoryInfo(sourcePath).GetFiles())
                        {
                            File.Copy(file.FullName, Path.Combine(Path.Combine(fbd.SelectedPath, "images"), file.Name), true);
                        }

                        sourcePath =
                            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "SampleAlertFolder/audio");

                        foreach (FileInfo file in new DirectoryInfo(sourcePath).GetFiles())
                        {
                            File.Copy(file.FullName, Path.Combine(Path.Combine(fbd.SelectedPath, "audio"), file.Name), true);
                        }
                    }
                    catch (Exception ex)
                    {
                        _lastError = "Runtime error configuring alerts path" +
                            System.Environment.NewLine +
                            ex.ToString();
                        return false;
                    }
                }
                else
                {
                    _lastError = "Alerts path not configured";
                    return false;
                }

                _scanPath = fbd.SelectedPath;

                Configuration config =
                    ConfigurationManager.OpenExeConfiguration
                    (ConfigurationUserLevel.None);                

                if (config.AppSettings.Settings["AlertsPath"]==null)
                {
                    config.AppSettings.Settings.Add("AlertsPath", _scanPath);                    
                }
                else
                {
                    config.AppSettings.Settings["AlertsPath"].Value = _scanPath;                    
                }
                
                config.Save();
                return true;
            }
        }

    
        public bool StartScan()
        {
            if (_timer.Enabled)
            {
                throw (new InvalidOperationException("Already Scanning"));
            }

            if (string.IsNullOrEmpty(_scanPath))
            {                
                if (!SetupAlertsPath()) { return false; }
            }

            if (!Directory.Exists(_scanPath))
            {              
                if (!SetupAlertsPath()) { return false; }
            }

            _timer.Start();
            return true;
        }

        public void StopScan()
        {
            if (!_timer.Enabled)
            {
                throw (new InvalidOperationException("Not Scanning"));
            }
            _timer.Stop();
        }

        public void timerTick(object sender, EventArgs e)
        {
            try
            {
                _timer.Stop();

                var alertFilePath = Path.Combine(_scanPath, "alert.json");
                if (File.Exists(alertFilePath))
                {
                    var errorText = "";
                    if (File.Exists(alertFilePath + ".failed")) { File.Delete(alertFilePath + ".failed"); }
                    if (File.Exists(Path.Combine(_scanPath, "alert_error.txt"))) { File.Delete(Path.Combine(_scanPath, "alert_error.txt")); }
                    if (File.Exists(Path.Combine(_scanPath, "alert_warn.txt"))) { File.Delete(Path.Combine(_scanPath, "alert_warn.txt")); }


                    PopupAlert alert = null;
                    try
                    {
                        alert = JsonConvert.DeserializeObject<PopupAlert>(File.ReadAllText(alertFilePath));
                    }
                    catch (Exception ex)
                    {
                        errorText = "Unable to decode json file '" + alertFilePath + "': " + ex.Message;
                    }

                    if (alert != null)
                    {
                        alert.SourcePath = _scanPath;
                        try
                        {
                            _popupEngine.ShowPopup(alert);
                        }
                        catch (Exception ex)
                        {
                            errorText = "Runtime error in ShowPopup: '" + ex.ToString() + "'";
                        }

                    }
                    if (errorText != "") //alert failed
                    {
                        File.WriteAllText(Path.Combine(_scanPath, "alert_error.txt"), errorText);
                        try
                        {
                            File.Move(alertFilePath, alertFilePath + ".failed");
                        }
                        finally
                        {

                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(_popupEngine.LastError))
                        {
                            File.WriteAllText(Path.Combine(_scanPath, "alert_warn.txt"), _popupEngine.LastError);
                        }
                    }
                    if (File.Exists(alertFilePath)) { File.Delete(alertFilePath); }
                }

            }
            finally
            {
                _timer.Start();
            }
        }
    }
}
