using UnityEngine;
using Hextant;
using Eflatun.SceneReference;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("SplashHelperEditor")]

namespace SplashHelper
{
    [Settings(SettingsUsage.RuntimeProject, "Game Startup")]
    public sealed class SplashHelperConfig : Settings<SplashHelperConfig>
    {
        internal const string blackScreenPath = "Packages/com.jjasundry.splash-helper/Assets/DefaultBlankScreen/BlankScreen.unity";
        internal const string unitySplashScreenPath = "Packages/com.jjasundry.splash-helper/Assets/DefaultBlankScreen/UnitySplash.unity";

        internal SceneReference SplashScreen => splashScreen;
        [SerializeField]
        private SceneReference splashScreen;

        internal SceneReference FirstScene => firstScene;
        [SerializeField]
        private SceneReference firstScene;

        internal bool UseBuiltInSplashScreen => useBuiltInSplashScreen;
        [SerializeField]
        [Tooltip("Uses the splash settings in the Unity Player Preferences settings.")]
        private bool useBuiltInSplashScreen = false;
    }
}
