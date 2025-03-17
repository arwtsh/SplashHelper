using System;
using UnityEngine.Scripting;

namespace SplashHelper
{
    /// <summary>
    /// This method is called when the game first initializes, before the splash screen.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CallMethodOnStartupAttribute : PreserveAttribute
    {
    }

    /// <summary>
    /// This method is called when the splash screen starts.
    /// This method must be awaitable.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class CallMethodOnSplashAttribute : PreserveAttribute
    {
    }
}
