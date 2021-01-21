
# EplandowntimeModule - Open Source

### Repository

This repository is an open source project - EplanDowntimeModule. The module offer possibility to manage Eplan downtime, when users are not using Windows.
The Add-in resolve problems, when users block a computer and don't close Eplan.
We are working on the project and solving various issues related to the development and project life.

### EplanDowntimeModule
For set up the add-in, please, use config file (_add-in name with __.config__ extension_).

Settings:
1. _Key_: __maxChecksCount__ - how many checks we need to make before showing warning about closing Eplan. Default value - __60__ times.
2. _Key_: __checkIntervalSec__ - interval in seconds before run check. Default value - __60__ seconds.

For example, with default values, we have __1__ hour downtime. If module noticed user input, the time will reset.
After showing warning window user have to accept message or Eplan will be close in __60__ seconds.

### License
The project is licensed under [MIT](LICENSE.txt) license.
