using UnityEngine;

namespace ComboSystem
{
    public class TimeController
    {
        /// <summary>
        /// Balance factor for speeding up time for our own non Unity related mechanics
        /// </summary>
        public static float TimeSpeed { get; set; } = 1f;
        public static bool Paused { get; private set; } = false;
        public static float Time { get; private set; }

        public void Update()
        {
            Time += UnityEngine.Time.deltaTime * TimeSpeed;
        }

        public void PauseOrResume(bool strictResume = false)
        {
            if (strictResume)
            {
                Paused = false;
                GameController.UIController.OnPauseOrResume();
                return;
            }

            if (Paused)
                Paused = false;
            else if (!Paused)
                Paused = true;
            GameController.UIController.OnPauseOrResume();
        }

        public void SetTime(float time) => Time = time;

    }
}