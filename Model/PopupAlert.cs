
namespace Tikisoft.DesktopAlertsDemo.Model
{
    class PopupAlert
    {
        public int PopupWidth { get; set; }
        public int PopupHeight { get; set; }
        public string AlertTitle { get; set; }
        public string AlertContent { get; set; }
        public string AlertImageFile { get; set; }
        public string AlertAudioFile { get; set; }
        public bool LoopAudio { get; set; }
        public int FontSize { get; set; }
        public int PopupDuration { get; set; }
        public bool AllowClose { get; set; }
        public string SourcePath { get; set; }
    }
}
