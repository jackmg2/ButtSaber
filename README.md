# Butt Saber revival

Quick n dirty port of [LovenseBSControl](https://github.com/Sesch69/LovenseBSControl) by Sesch69 to connect [Intiface](https://intiface.com/) to Beat Saber.

**Intiface**, formerly [buttplug.io](buttplug.io) is an open-source standard to control connected toys.

**Big thanks to Sesch69 who did all of the work.**

I have just changed the Lovense API to Intiface one.

## Prerequisites

- [Intiface-Central](https://intiface.com/central/) to connect your toys
- Mod BSIPA is required
- Mod is based on Lib Harmony

## Compilation

* Open your project in Visual Studio Community
* Restore nuget packages
* Use your Beat Saber folder with your required mods installed ([ModAssistant](https://github.com/Assistant/ModAssistant) is the easiest way) for missing dependencies
* Rebuild your project

TBD

## Installation

- Copy dlls from your bin folder into the Plugin folder of the main directory of Beat Saber
- Goto to Settings -> Mod Settings -> Check Lovense BS Control settings

## Setting Options

* Enable Mod
* Mode: Select a mode to play *(at this time only Default and Challenge 1 are working)*
* Vibrate on miss: Vibrate toys on miss (For Default mode)

* Random Intense (miss): Random intense between 1 and 20 (For Default mode)
* Intense (miss): Fix intense on miss block (For Default mode)
* Duration (miss): Duration of vibration in milliseconds (more or less exact, for Default mode)

* Vibrate on hit: Vibrate toys on hitting boxes (For Default mode) 
* Random Intense (hit): Random intense between 1 and 20 (For Default mode)
* Intense (hit): Fix intense on hit block (For Default mode)
* Duration (hit): Duration of vibration in milliseconds (more or less exact, for Default mode)

* Preset on bomb hit: Vibrate toys with a preset on hitting a bomb (For Default mode)
* Rotate Intense (Nora)
* Air Intense (Max)

* Toys... -> Shows connected toys on all connections in a list, allows to refresh list and test the connected toys. Also possible to select which hand controls the toy

## Connections...

*Only working on local PC at this time*

## Mode

- Default: Use the configuration for hit/miss/intense/duration
- Challenge 1: With each miss, the vibration inreases, after 15 correct hits, it is reducing by 1 intense level
- Preset: Vibrate on Miss with a fixed defined preset
