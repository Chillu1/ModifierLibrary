using CustomGameNamespace.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CustomGameNamespace
{
    public class GameController : Singleton<GameController>
    {
        /// <summary>
        /// Actual game speed (dev/testing purposes)
        /// </summary>
        public static float GameSpeed { get { return Time.timeScale; } set { Time.timeScale = value; } }//We could limit the gamespeed at which mechanics (might) break

        public static UIController UIController { get; private set; }
        public static TimeController TimeController { get; private set; }

        public void Start()
        {
            SceneManager.sceneLoaded += delegate { GameplaySceneStarted(); };
            GameplaySceneStarted();

            CommandsController.Init();
        }

        public void GameplaySceneStarted()//Move to scene controller?
        {
            TimeController = new TimeController();

            UIController = FindObjectOfType<UIController>();
        }

        public void Update()
        {
            if (!TimeController.Paused)
            {
                TimeController.Update();
            }
            CommandsController.Update(Time.deltaTime);
        }
    }

}