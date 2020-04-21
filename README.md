# DesktopAlertsDemo
Proof-of-concept Winforms application that listens for incoming alerts and notifies Windows users with a popup on the screen.

## Description
This demo program runs in background waiting for "alerts" to be triggered. When an alert is detected it displays a popup, on the screen, optionally including a image and a sound.

## End-user features
  - Alerts are triggered by dropping a json formatted file in a configurable file system folder.
  - When triggered. alerts display a popup on the screen.
  - Alerts title, duration, (text) content, image, sound, popup size, text size are all configurable.
  - Alerts "dismissable" property can also be set.
  - This demo runs in the background. An icon is added in the systray from where it can be shut-down.
  
## Developer features
  - Extensible Scanner support: New custom scanners can be implemented by inheriting from IAlertScanner, it includes a "LocalPathAlertScanner" for demo and testing purposes.
  - New popup engines and audio players can also be implemented by inheriting from IPopupEngine and IAudioPlayer respectively. "TulpepPopupEngine" and "NaudioPlayer" are included as examples.

## Showcased programming technologies  
  - C#
  - .Net Framework 4.7.1
  - OOP
  - Dependency Injection
  
## Test-run
  - When ran for the first time, a dialog pops up allowing to folder choice to scan for new alerts.
  - A folder structure will be automatically created there, with "audio" and "image" sub-folders along with several json sample alert templates. Please take a look at those sample json files to understand how to customize an alert.
  - AlertScanner will monitor the alerts folder for an incoming "alert.json" file.
  - Alert processing can produce 3 different outcomes:
    - **Success**: Alert is displayed
    - **Warning**: In case that a resource is missing (an image or audio file was not found) Alert shows without the missing media. An "alert_warn.txt" file is created which shows more detail about what has gone wrong.
    - **Error**: json file format is invalid, or some required parameter is missing. 
      - Alert will not be triggered
      - An alert_error.txt file will be created with error detail
      - The alert.json file originating the error will be renamed to "alert.json.failed".
      
  
  

