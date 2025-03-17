using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

namespace SplashHelper
{
    public class SplashScreenEvents : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("Called when all asyncronous loading methods finish.")]
        private UnityEvent OnLoadingFinish = new UnityEvent();

        /// <summary>
        /// Propagate the OnLoadingFinish to every component
        /// </summary>
        internal static async Awaitable WaitForSplashScreenCompletion()
        {
            Assert.IsFalse(HasFinishedLoading, "Cannot call WaitForSplashScreenCompletion more than once.");

            //Make the UnityEvent awaitable
            TaskCompletionSource<bool> waitForSplashComplete = new TaskCompletionSource<bool>();
            SplashScreenEvents.OnSplashFinish.AddListener(() => waitForSplashComplete.SetResult(true));

            //Tell the SplashScreen scene that all the loading has finished.
            //This lets it finish any animations, enable a skip button, or any other stuff.
            HasFinishedLoading = true;
            SplashScreenEvents[] splashScreenEvents = FindObjectsByType<SplashScreenEvents>(FindObjectsSortMode.None);
            foreach (SplashScreenEvents splashScreenEvent in splashScreenEvents)
            {
                splashScreenEvent.OnLoadingFinish?.Invoke();
            }

            //Wait for the SplashScreen scene to finish completely, all animations, it was skipped, etc.
            await waitForSplashComplete.Task;
        }

        private static bool HasFinishedLoading = false;

        /// <summary>
        /// Invoked by when Splash scene is ready to change to the starting screen.
        /// </summary>
        internal static UnityEvent OnSplashFinish = new UnityEvent();

        /// <summary>
        /// Call to transition out of the splash screen into the game.
        /// </summary>
        public void TransitionToFirstScene()
        {
            Assert.IsTrue(HasFinishedLoading, "Cannot transition out of the splash screen if the game has not finished loading!");

            OnSplashFinish?.Invoke();
            OnSplashFinish = null; //Clear the UnityEvent after it gets invoked so it doesn't get invoked twice
        }

        public async Awaitable WaitForLoading()
        {
            if (!HasFinishedLoading)
            {
                //Make the UnityEvent awaitable
                TaskCompletionSource<bool> waitForLoadingToComplete = new TaskCompletionSource<bool>();
                OnLoadingFinish.AddListener(() => waitForLoadingToComplete.SetResult(true));
                await waitForLoadingToComplete.Task;
            }
        }
    }
}
