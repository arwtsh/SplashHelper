using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Playables;
using SplashHelper;

namespace SplashHelpter.Samples.JJASundry
{
    [RequireComponent(typeof(PlayableDirector))]
    public class SplashScreenManager : MonoBehaviour
    {
        [SerializeField]
        private ThrobberTransparenter throbberTransparenter;

        [SerializeField]
        PlayableAsset FadeOutSequence;

        private bool isLoadingFinished = false;

        void Start()
        {
            Assert.IsNotNull(FadeOutSequence);
            Assert.IsNotNull(throbberTransparenter);

            throbberTransparenter.SetAlphaInstant(0); //Start invisible
        }

        public void LoadingFinished()
        {
            isLoadingFinished = true;
            throbberTransparenter.SetAlphaTransition(0); //Get rid of the loading symbol
        }

        public void OnSplashFadeIn()
        {
            if(!isLoadingFinished)
            {
                throbberTransparenter.SetAlphaTransition(1);
            }
        }

        public async void OnSplashDelayFinished()
        {
            //Wait until the game has finished loading
            while(!isLoadingFinished)
            {
                await Awaitable.NextFrameAsync();
            }

            PlayableDirector pd = GetComponent<PlayableDirector>();
            Assert.IsNotNull(pd);
            pd.Play(FadeOutSequence);
        }
    }
}
