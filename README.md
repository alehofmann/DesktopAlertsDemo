# DesktopAlertsDemo
Proof-of-concept Winforms application that listens for incoming alerts and notifies Windows users with a popup on the screen.

## Description
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

## Showcased programming technologies  
  - C#
  - .Net Framework 4.7.1
  - OOP
  - Dependency Injection
  
## Test-run
  - When ran for the first time, a dialog will popup to choose a folder to scan for new alerts.
  - A folder structure will be automatically created there, with an "audio" and "image" sub-folders and several json sample alert files. Please take a look into those sample json files to understand how to configure an alert.
  - AlertScanner will monitor alerts folder for an incoming "alert.json" file.
  - Processing an alert can produce 3 different outcomes:
    - **Success**: Alert shows
    - **Warning**: In the case that for example a resource is missing (an image or audio file was not found) Alert shows, but missing media is not played, and an "alert_warn.txt" file is created which shows more detail about what gone wrong.
    - **Error**: json file format is invalid, or some required parameter is missing. 
      - Alert will not be triggered
      - An alert_error.txt file will be created with error detail
      - The alert.json file originating the error will be renamed to "alert.json.failed".
      
  
  

