using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace SplashHelpter.Samples.JJASundry
{
    internal class ThrobberTransparenter : MonoBehaviour
    {
        const float MIN_SPEED = 0.0001f;

        [SerializeField]
        [Min(MIN_SPEED)]
        private float speed = 0.1f;

        private float Speed => speed > MIN_SPEED ? speed : MIN_SPEED; //Return MIN_SPEED if the speed is less than it somehow

        Awaitable currentProcess;

        [SerializeField]
        private Image target;

        // 0 is transparent, 1 is opaque
        private float Transparency
        {
            get
            {
                Assert.IsNotNull(target);

                float blockingAlpha = target.color.a;
                float result = Mathf.Clamp01(1 - blockingAlpha);
                return result;
            }
            set
            {
                Assert.IsNotNull(target);

                float newAlpha = Mathf.Clamp01(1 - value);
                Color newColor = target.color;
                newColor.a = newAlpha;
                target.color = newColor;
            }
        }

        private void Awake()
        {

        }

        public void SetAlphaTransition(float newAlpha)
        {
            currentProcess = SetAlpha(newAlpha);
        }

        public void SetAlphaInstant(float newAlpha)
        {
            //Cancel the current process, if one exists.
            if (currentProcess != null)
            {
                currentProcess.Cancel();
                currentProcess = null;
            }

            Transparency = newAlpha;
        }

        private async Awaitable SetAlpha(float goal)
        {
            currentProcess?.Cancel();
            currentProcess = null;

            Assert.IsTrue(goal >= 0 && goal <= 1, "The parameter \"goal\" of function SetAlpha must be between the values of 0 and 1, inclusive.");

            if(Mathf.Approximately(Transparency, goal)) //If it's already at the goal, don't do anything.
                return;

            bool isGoingOpaque = goal > Transparency; //Mark the direction the transparency will go.

            while(!Mathf.Approximately(Transparency, goal))
            {
                Assert.IsTrue(isGoingOpaque == (goal > Transparency), "Infinite loop detected!"); //Make sure the logic is flowing in the correct direction.

                if (isGoingOpaque)
                {
                    Transparency += Time.deltaTime * Speed;
                }
                else
                {
                    Transparency -= Time.deltaTime * Speed;
                }

                await Awaitable.NextFrameAsync();
            }

            //After this async loop finishes, clear its reference from memory.
            currentProcess = null;
        }
    }
}
