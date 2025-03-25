# Changelog

## v1.0.1 (3/25/2025)

### Summary

The pre-build logic now forces the scenes to be enabled in the build settings in addition to adding them if they're missing.

## Fixed

- Auto-adding startup scenes to the build also makes them enabled if they were disabled.

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