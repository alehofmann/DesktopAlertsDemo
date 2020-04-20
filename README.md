# DesktopAlertsDemo
Desktop Alerts small demo app

This demo program runs in background waiting for "alerts" to be triggered. When an alert is detected it displays a popup, on the screen, optionally including a picture and a sound.

## End-user features
  - Alerts are triggered by dropping a json formatted file in a configurable file system folder.
  - When triggered. alerts display a popup on the screen.
  - Alerts title, duration, (text) content, picture, sound, popup size, text size are configurable.
  - Alerts "dismissable" property can also be set.
  - This demo runs on the background. An icon is added on the systray from where it can be shut-down.
  
## Developer features
  - Extensible Scanner support: New custom scanners could be built by inheriting from IAlertScanner, it includes a "LocalPathAlertScanner" for demo and testing purposes.
  - New popup engines and audio players can also be implemented by inheriting from IPopupEngine and IAudioPlayer respectively. "TulpepPopupEngine" and "NaudioPlayer" are included as examples.
  

