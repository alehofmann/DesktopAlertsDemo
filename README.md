# DesktopAlertsDemo
Desktop Alerts small demo app

This demo program runs in background waiting for "alerts" to be triggered. When an alert is detected it displays a popup, on the screen, optionally including a picture and a sound.

## Features
  - Extensible Scanner support: New custom scanners could be built by inheriting from IAlertScanner, it includes a "LocalPathAlertScanner" for demo and testing purposes.
  - New popup engines can also be developed by inheriting from IPopupEngine. "TulpepPopupEngine" is included which allows to display simple popups, always on top including text, audio and images.


