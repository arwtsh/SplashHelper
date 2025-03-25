using UnityEditor.Build.Reporting;
using UnityEditor.Build;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;
using UnityEngine.SceneManagement;

namespace SplashHelper.Editor
{
    public class PreBuildLogic : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            try
            {
                //Force the BlackScreen to be the first scene in the build settigns so it is always loaded first in builds.
                if (!EditorBuildSettings.scenes[0].path.Equals(SplashHelperConfig.blackScreenPath))
                {
                    Debug.Log("Adding BlackScreen to the first of the Build Settings scene list.");

                    List<EditorBuildSettingsScene> oldSceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                    List<EditorBuildSettingsScene> newSceneList = new List<EditorBuildSettingsScene>();

                    //First thing add the BlackScreen to the build settings
                    newSceneList.Add(new EditorBuildSettingsScene(SplashHelperConfig.blackScreenPath, true));

                    //Add all the scenes from the build list that aren't the BlackScreen scene (because it's already been added)
                    //This removes any old BlackScreen scenes that the user messed with.
                    newSceneList.AddRange(oldSceneList.Where(oldScene => !oldScene.path.Equals(SplashHelperConfig.blackScreenPath)));

                    //Apply the changes
                    EditorBuildSettings.scenes = newSceneList.ToArray();
                }

                //Force the black screen to be enabled
                EditorBuildSettings.scenes[0].enabled = true;

                //Find which splash screen the project is using and add it
                SceneReference splashScene = (SplashHelperConfig.instance != null && SplashHelperConfig.instance.UseBuiltInSplashScreen) ?
                    SplashHelperScenes.GetUnitySplashReference() :
                    SplashHelperConfig.instance.SplashScreen;
                if(splashScene != null)
                {
                    int index = SceneUtility.GetBuildIndexByScenePath(splashScene.Path);
                    if(index < 0) // Not in build
                    {
                        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                        scenes.Add(new EditorBuildSettingsScene(splashScene.Path, true));
                        EditorBuildSettings.scenes = scenes.ToArray();
                    }

                    //Force the splash screen to be enabled.
                    EditorBuildSettings.scenes[index].enabled = true;
                }


                if (SplashHelperConfig.instance != null && SplashHelperConfig.instance.FirstScene)
                {
                    int index = SceneUtility.GetBuildIndexByScenePath(SplashHelperConfig.instance.FirstScene.Path);
                    if (index < 0) // Not in build
                    {
                        List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                        scenes.Add(new EditorBuildSettingsScene(SplashHelperConfig.instance.FirstScene.Path, true));
                        EditorBuildSettings.scenes = scenes.ToArray();
                    }

                    //Force the first scene to be enabled.
                    EditorBuildSettings.scenes[index].enabled = true;
                }
            }
            catch (System.Exception ex)
            {
                throw new BuildFailedException("Encountered an exception when adding scenes to the build, splash screen and game startup sequence will not work correctly.\n" + ex);
            }
        }
    }
}
