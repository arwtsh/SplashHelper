using System.Reflection;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using SceneLoading;

namespace SplashHelper
{
    internal class SplashHelperManager
    {
        internal SplashHelperScenes scenes { get; private set; }

        private SplashHelperManager()
        {
            scenes = new SplashHelperScenes();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void GameStartup()
        {
            SplashHelperManager splashHelper = new SplashHelperManager();

            splashHelper.Startup();
        }

        private async void Startup()
        {
            //Wait one frame so the game has a chance to display
            await Awaitable.NextFrameAsync();

            CallAllStartupMethods();

            //Load the splash screen if the variable has been assigned in the config and if the game started in the black screen.
            if (scenes.IsSplashScreenValid())
            {
                if(scenes.BlackScreen.IsLoaded())
                {
                    //Load into the splash screen
                    await SceneLoader.LoadScene(scenes.SplashScreen);
                }
            }
            else
            {
                Debug.LogWarning("No splash screen scene defined.");
            }

            //Allow any system to start loading resources or anything else they should do while the game is starting up
            await CallAllLoadMethods();

            //Only preload the first scene if we're in the splash screen. Otherwise there is no reason.
            if (scenes.IsFirstSceneValid() && scenes.SplashScreen.IsLoaded())
            {
                //Preload the first scene while the splash screen is still going.
                //This should help reduce hitching
                await SceneLoader.PreloadScene(scenes.FirstScene);
            }

            //If the splash screen is the active scene, wait for it to complete
            if (scenes.IsSplashScreenValid() && scenes.SplashScreen.IsLoaded())
            {
                //All loading has finished
                //Wait for the splash screen to give the go-ahead to transition to the next scene.
                await SplashScreenEvents.WaitForSplashScreenCompletion();
            }

            //Go to the first scene only if the game is in the startup sequence.
            //Otherwise, likely what happened is the editor user is testing a scene and changing scenes suddenly would be disruptive to their work.
            if (scenes.IsFirstSceneValid())
            {
                if(scenes.SplashScreen.IsLoaded() || scenes.BlackScreen.IsLoaded())
                {
                    await SceneLoader.LoadScene(scenes.FirstScene);
                }
            }
            else
            {
                Debug.LogWarning("No starting scene assigned. Game cannot start properly.");
            }
        }

        /// <summary>
        /// Call all the methods that happen when the game starts up for the first time. 
        /// This should be related to graphics & application settings, things that need done before the splash screen.
        /// </summary>
        private void CallAllStartupMethods()
        {
            Debug.Log("Calling Startup methods.");

            List<MethodInfo> startupMethods = ReflectionUtils.FindMethodsWithAttribute<CallMethodOnStartupAttribute>();

            foreach (MethodInfo startupMethod in startupMethods)
            {
                startupMethod.Invoke(null, null);
            }
        }


        /// <summary>
        /// Allow any system to start loading resources or anything else they should do while the game is starting up
        /// </summary>
        private async Awaitable CallAllLoadMethods()
        {
            Debug.Log("Calling Load methods.");

            //Find all the methods that should be invoked for game loading.
        #if !UNITY_WEBGL	
            //The function should be safe to execute on a background thread because it doesn't use any unity-specific API calls.
            List<MethodInfo> loadMethods = await UniTask.RunOnThreadPool(ReflectionUtils.FindMethodsWithAttribute<CallMethodOnSplashAttribute>);
        #else
            List<MethodInfo> loadMethods = ReflectionUtils.FindMethodsWithAttribute<CallMethodOnSplashAttribute>();
        #endif

            List<UniTask> results = new List<UniTask>();

            foreach (MethodInfo loadMethod in loadMethods)
            {
                //Call the method
                var result = loadMethod.Invoke(null, null);

                //Find the right awaitable return type and convert it to UniTask so that all of them can be awaited.
                if(result is Awaitable awaitableResult)
                {
                    results.Add(awaitableResult.ToUniTask());
                }
                else if(result is UniTask uniTaskResult)
                {
                    results.Add(uniTaskResult);
                }
                else if(result is Task taskResult)
                {
                    results.Add(taskResult.AsUniTask());
                }
                else
                {
                    Debug.LogWarning("Method " + loadMethod.Name + " in type " + loadMethod.DeclaringType + " was marked as \"CallMethodOnSplash\" but does not have a return type of Awaitable, UniTask, or Task.");
                }
            }

            await UniTask.WhenAll(results);
        }
    }
}
