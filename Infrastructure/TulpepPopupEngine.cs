using System;
using Tulpep.NotificationWindow;
using Tikisoft.DesktopAlertsDemo.Model;
using System.Drawing;
using System.IO;

namespace Tikisoft.DesktopAlertsDemo.Infrastructure
{
    class TulpepPopupEngine : IPopupEngine
    {
        private IAudioPlayer _audioPlayer;
        private PopupNotifier _popupNotifier;

        public TulpepPopupEngine(IAudioPlayer audioPlayer)
        {
            _audioPlayer = audioPlayer;
            ConfigurePopupNotifier();
            
        }

        private void ConfigurePopupNotifier()
        {
            
        }

        private void PopupClosed(object sender, EventArgs args)
        {
            _audioPlayer.StopPlaying();
        }

        public string LastError { get; set; }

        public void ShowPopup(PopupAlert popup)
        {
            Image popupImage=null;
            LastError = "";            
            

            if (!string.IsNullOrEmpty(popup.AlertAudioFile) && _audioPlayer!=null)
            {
                var audioFile = Path.Combine(Path.Combine(popup.SourcePath, "audio"), popup.AlertAudioFile);
                try
                {
                    _audioPlayer.PlayAudio(audioFile,popup.LoopAudio);
                }
                catch(Exception ex)
                {
                    LastError = "Unable to play audio file [" + audioFile  + "': " + ex.Message;
                }
                
            }

            //if(popup.AlertImageFile!="" && File.Exists(popup.AlertImageFile))
            if (!string.IsNullOrEmpty(popup.AlertImageFile))
            {
                var imageFile = Path.Combine(Path.Combine(popup.SourcePath, "images"), popup.AlertImageFile);
                try
                {
                    popupImage = Image.FromFile(imageFile);                    
                }
                catch (Exception ex)
                {
                    LastError = "Unable to LOAD image file '" + imageFile + "' (" + ex.Message + ")";
                }
            }

            FontFamily fontFamily = new FontFamily("Arial");
            Font font = new Font(
               fontFamily,
               30,
               FontStyle.Regular,
               GraphicsUnit.Pixel);

            _popupNotifier = new PopupNotifier
            {
                ShowCloseButton = popup.AllowClose,
                ShowOptionsButton = true,
                ShowGrip = false,
                AnimationDuration = 100,
                Scroll = false,
                ContentFont = font,
                TitleFont = font,
                TitleText = popup.AlertTitle,
                ContentText = popup.AlertContent,
                Size = new Size(popup.PopupWidth, popup.PopupHeight),
                Delay = popup.PopupDuration,
                Image=popupImage                
            };

            _popupNotifier.Disappear += new EventHandler(PopupClosed);
            _popupNotifier.Close += new EventHandler(PopupClosed);
                                                
            _popupNotifier.Popup();            
        }
    }
}
