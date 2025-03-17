# Documentation

## Overview

A Unity package that assists with the splash screen and initial loading logic.

## Installation Instructions

Installing this package via the Unity package manager is the best way to install this package. There are multiple methods to install a third-party package using the package manager, the recommended one is `Install package from Git URL`. The URL for this package is `https://github.com/arwtsh/SplashHelper.git`. The Unity docs contains a walkthrough on how to install a package. It also contains information on [specifying a specific branch or release](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-git.html#revision).

Alternatively, you can download directly from this GitHub repo and extract the .zip file into the folder `ProjectRoot/Packages/com.jjasundry.splash-helper`. This will also allow you to edit the contents of the package.

## Requirements

Tested on Unity version 6000.0; will most likely work on older versions, but you will need to manually port it.

## Description of Assets

This package comes with two attributes that can be used for initialization logic. 

Any static method marked `CallMethodOnStartup` will execute at the start of the game. During a build, this is before the splash screen shows, right when the window first appears. These methods should be related to adjusting the display and other app or os related functionality that needs to be set when the game is starting.

Any static method marked `CallMethodOnSplash` will execute during the splash screen. These methods must be asyncronous and awaitable (returning [Awaitable](https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Awaitable.html), [Task](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-9.0), or [UniTask](https://github.com/Cysharp/UniTask)). These methods should be related to loading in resources at the start of the game. Note that when starting the game outside the splash screen, the game will begin before the async methods complete.

You can set the Splash scene in the project settings, or tell it to use Unity's native splash screen. You can also set which scene the game will transition to after the splash scene (such as the main menu). Custom splash screens must have a GameObject with the `SplashScreenEvents` component. The function `TransitionToFirstScene` should be called to notify the splash sequence that the splash scene has finished and is ready to enter into a gameplay scene. The event `OnLoadingFinish` should be used to notify the splash screen when it is allowed to stop. In custom splash screens, if the splash animation is shorter than the load time, a loading symbol should appear.

This package does not force the game to begin at the splash screen. If a developer tells the unity editor to enter play mode, it will enter the scene it was in, not jump to the splash screen. This package only forces builds to start at the splash screen. Scenes are automatically added or rearranged in the build settings so that the splash screen loads first.

## Samples

Includes 1 sample, an example splash screen. Has a logo and name that animates in and out, with a loading symbol if the `CallMethodOnSplash` calls last longer than the animations.