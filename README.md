# Butt Saber revival

Quick n dirty port of [LovenseBSControl](https://github.com/Sesch69/LovenseBSControl) by Sesch69 to connect [Intiface](https://intiface.com/) to Beat Saber.

**Intiface**, formerly [buttplug.io](buttplug.io) is an open-source standard to control connected toys.

**Big thanks to Sesch69 who did all of the work in the parent project.**

## Prerequisites

- [Intiface-Central](https://intiface.com/central/) to connect your toys
- Mod [BSIPA](https://nike4613.github.io/BeatSaber-IPA-Reloaded/) is required
- [ModAssistant](https://github.com/Assistant/ModAssistant) can help you!
- Mod is based on Lib Harmony

## Compilation
* If you have downloaded the project as a zip on Windows, please right click on it, click on properties and then "Unblock". Some dlls may not work if you don't.
* Edit ButtSaber.csproj.user with your own Beat Saber path
* Launch the game a first time
* Launch it a second time after using [ModAssistant](https://github.com/Assistant/ModAssistant) to install basic mods
* Open the solution in Visual Studio
* Restore nuget packages
* Rebuild your project

## Installation
* Copy dlls from your bin folder into the Plugin folder of the main directory of Beat Saber
* Goto to Settings -> Mod Settings -> Check Lovense BS Control settings

## Setting Options
* Enable Mod
* Mode: Select a mode to play
* Vibrate on miss: Vibrate toys on miss (For Default mode)

* Random Intense (miss): Random intensity between 1 and 20 (For Default mode)
* Intense (miss): Fix intensity on miss block (For Default mode)
* Duration (miss): Duration of vibration in milliseconds (more or less exact, for Default mode)

* Vibrate on hit: Vibrate toys on hitting boxes (For Default mode) 
* Random Intense (hit): Random intensity between 1 and 20 (For Default mode)
* Intense (hit): Fix intensity on hit block (For Default mode)
* Duration (hit): Duration of vibration in milliseconds (more or less exact, for Default mode)

* Preset on bomb hit: Vibrate toys with a preset on hitting a bomb (For Default mode) //Not implemented yet

* Toys... -> Shows connected toys on all connections in a list, allows to refresh list and test the connected toys. Also possible to select which hand controls the toy

## Connections...

*Only working on local PC at this time*

## Mode
* Default: Use the configuration for hit/miss/intense/duration
* Challenge 1: With each miss, the vibration increases, after 15 correct hits, it is reducing by 1 intense level
* Preset: Vibrate on Miss with a fixed defined preset

## Known issues
* OK button doesn't work on mod setting. You can save with Cancel button...

## Todo
- [ ] Fix OK Buton
- [ ] Fix conflict with Newtonsoft.json versions
