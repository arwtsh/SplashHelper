using UnityEngine;
using Eflatun.SceneReference;
using UnityEngine.Assertions;
using System.Runtime.CompilerServices;
using SceneLoading;

[assembly: InternalsVisibleTo("SplashHelperEditor")]

namespace SplashHelper
{
    internal class SplashHelperScenes
    {
        private SceneReference unitySplash;

        internal SceneReference BlackScreen { get; private set; }

        // Redirect variables from the Config, or if null, create empty SceneReference
        internal SceneReference SplashScreen { 
            get 
            {
                if (SplashHelperConfig.instance.UseBuiltInSplashScreen)
                {
                    if(unitySplash == null)
                        unitySplash = GetUnitySplashReference();

                    return unitySplash;
                }
                else
                {
                    return SplashHelperConfig.instance.SplashScreen ?? new SceneReference();
                }
            } 
        }
        internal SceneReference FirstScene => SplashHelperConfig.instance.FirstScene ?? new SceneReference();

        internal SplashHelperScenes()
        {
            CreateBlackScreenReference();
            ValidateReferences();
        }

        private void CreateBlackScreenReference()
        {
            try
            {
                BlackScreen = SceneReference.FromScenePath(SplashHelperConfig.blackScreenPath);
            }
            catch (System.Exception ex)
            {
                //The only reason this should hit is if the path was bad or the SceneReference system hasn't discovered the scene yet.
                Debug.LogException(ex);
                Debug.LogAssertion("There was an error finding the Black Screen scene asset in package Splash Helper");
                //Assert.IsTrue(false);
                //TODO: test if LogAssertion or Assert actually stops the code from executing.
            }

            //Make sure the BlackScreen isn't null (it shouldn't be, if creating the SceneReference went wrong it would exist in the unsafe state.).
            Assert.IsNotNull(BlackScreen);
        }

        private void ValidateReferences()
        {
            //Double check everything for BlackScreen. It should be 100% reliable failsafe in builds.
            Assert.IsNotNull(BlackScreen, "The BlackScreen scene reference failed to be created.");
            Assert.IsFalse(BlackScreen.UnsafeReason == SceneReferenceUnsafeReason.NotInMaps, "The SceneReference system has not discovered the BlackScreen scene!");
#if !UNITY_EDITOR //The assertion only functions in development standalone builds. It doesn't get automatically added to the build settings until the game builds, because the editor doesn't need scenes to be in the build list. So if this were left on in the editor, it would assert when it isn't needed.
            Assert.IsFalse(BlackScreen.UnsafeReason == SceneReferenceUnsafeReason.NotInBuild, "The BlackScreen scene reference has not yet been added to the build.");
#endif
            Assert.IsFalse(BlackScreen.State == SceneReferenceState.Addressable, "The BlackScreen scene reference cannot be an Addressable!");

            //Both of these should default to an empty SceneReference, not be null.
            Assert.IsNotNull(SplashScreen);
            Assert.IsNotNull(FirstScene);

            Assert.IsFalse(SplashScreen.UnsafeReason == SceneReferenceUnsafeReason.NotInMaps, "The SceneReference system has not discovered the SplashScreen scene!");
            if (SplashScreen.UnsafeReason == SceneReferenceUnsafeReason.NotInBuild)
            {
                Debug.LogWarning("SplashScreen " + SplashScreen.Name + " is not included in the build settings.");
            }
            else if (SplashScreen.UnsafeReason == SceneReferenceUnsafeReason.Empty)
            {
                Debug.LogWarning("The SplashScreen does not reference a scene. There will be no splash screen.");
            }

            Assert.IsFalse(FirstScene.UnsafeReason == SceneReferenceUnsafeReason.NotInMaps, "The SceneReference system has not discovered the FirstScene scene!");
            if (FirstScene.UnsafeReason == SceneReferenceUnsafeReason.NotInBuild)
            {
                Debug.LogWarning("FirstScene " + FirstScene.Name + " is not included in the build settings.");
            }
            else if (FirstScene.UnsafeReason == SceneReferenceUnsafeReason.Empty)
            {
                //This isn't an error or assert because it is possible to test the splash screen without an actual game.
                Debug.LogWarning("The FirstScene does not reference a scene. The game will not start past the start screen.");
            }
        }

        internal static SceneReference GetUnitySplashReference()
        {
            SceneReference unitySplashReference = null;

            try
            {
                unitySplashReference = SceneReference.FromScenePath(SplashHelperConfig.unitySplashScreenPath);
            }
            catch (System.Exception ex)
            {
                //The only reason this should hit is if the path was bad or the SceneReference system hasn't discovered the scene yet.
                Debug.LogException(ex);
                Debug.LogAssertion("There was an error finding the UnitySplash scene asset in package Splash Helper");
                //Assert.IsTrue(false);
                //TODO: test if LogAssertion or Assert actually stops the code from executing.
            }

            //Make sure the BlackScreen isn't null (it shouldn't be, if creating the SceneReference went wrong it would exist in the unsafe state.).
            Assert.IsNotNull(unitySplashReference);

            //Make sure it was created properly and isn't an addressable.
            //It is allowed to not be in the build, because the scene loader will handle that condition, and the build process will auto-add it.
            Assert.IsTrue(unitySplashReference.State == SceneReferenceState.Regular || unitySplashReference.UnsafeReason == SceneReferenceUnsafeReason.NotInBuild);

            return unitySplashReference;
        }

        internal bool IsBlackScreenValid()
        {
            return BlackScreen.IsLoadable();
        }

        internal bool IsSplashScreenValid()
        {
            return SplashScreen.IsLoadable();
        }

        internal bool IsFirstSceneValid()
        {
            return FirstScene.IsLoadable();
        }
    }
}
