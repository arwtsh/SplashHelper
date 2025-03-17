using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace SplashHelper
{
    [RequireComponent(typeof(SplashScreenEvents))]
    public class UnitySplashScreen : MonoBehaviour
    {
        private void Awake()
        {
            SplashScreen.Begin();
        }

        void Start()
        {
            SplashScreen.Draw();

            WaitForSplashEnd();
        }

        async void WaitForSplashEnd()
        {
            while (!SplashScreen.isFinished)
            {
                await Awaitable.NextFrameAsync();
            }

            SplashScreenEvents events = GetComponent<SplashScreenEvents>();

            //After everything is loaded, transition to the first scene.
            await events.WaitForLoading();
            events.TransitionToFirstScene();
        }
    }
}
