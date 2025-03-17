using Hextant.Editor;
using Hextant;
using UnityEditor;

namespace SplashHelper.Editor
{
    public class SplashHelperConfigEditor
    {
        [SettingsProvider]
        static SettingsProvider GetSettingsProvider() => Settings<SplashHelperConfig>.instance.GetSettingsProvider();
    }
}
