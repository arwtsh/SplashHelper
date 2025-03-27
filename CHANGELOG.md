# Changelog

## v1.0.1 (3/27/2025)

### Summary

Minor changes to build automation, fixing aspect ratio of sample splash screen and making it WebGL compatable.

## Added

- Auto-adding startup scenes to the build also makes them enabled if they were disabled.

## Changed 

- The example splash screen provided in the sample will now scale to the width of the screen. This means the window can get very skinny and the splash is still completely visible.

## Fixed

- The sample screen used a Reverb Filter component to add reverb to the `logoBurnIn.wav` audio file. WebGL does not support that component but doesn't throw an error. The reverb has been baked into the audio file instead.

- Does not find methods marked as CallMethodOnSplashAttribute on a seperate thread in WebGL builds (it will hang the application).

## v1.0.0 (3/15/2025)

### Summary

Package released.

### Added

- CallMethodOnStartup attribute
- CallMethodOnSplash attribute
- Auto-discovery of static methods marked with the attribute using reflection
- Project Settings UI and settings object to store which scenes should be used to start the game
- Auto-adding startup scenes to the build if they are not included
- Option to use the default Unity splash features in the Player Settings.
- Sample of a splash screen showing a logo and name with a fancy particle animation and audio. 