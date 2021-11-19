using System;
using CommandTerminal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ComboSystem
{
    public class CommandsController
    {
        public static void Init()
        {
            Terminal.Autocomplete.Register(new string[] { "timespeed", "gamespeed", "settime", "dev", "pause" });
        }

        public static void Update(float deltaTime)
        {
            if (Input.GetButtonDown("Pause"))
            {
                GameController.TimeController.PauseOrResume();
            }
        }

        [RegisterCommand(Name = "gamespeed", MinArgCount = 1, MaxArgCount = 1, Help = "Set gamespeed (dev/testing)")]
        private static void CommandSetGameSpeed(CommandArg[] args)
        {
            if (Terminal.IssuedError)
                return;

            GameController.GameSpeed = args[0].Float;
        }

        [RegisterCommand(Name = "timespeed", MinArgCount = 1, MaxArgCount = 1, Help = "Set timespeed (balance factor for non-Unity mechanics")]
        private static void CommandSetTimeSpeed(CommandArg[] args)
        {
            if (Terminal.IssuedError)
                return;

            TimeController.TimeSpeed = args[0].Float;
        }

        [RegisterCommand(Name = "settime", MinArgCount = 1, MaxArgCount = 1, Help = "Set time to X seconds")]
        private static void CommandSetTime(CommandArg[] args)
        {
            if (Terminal.IssuedError)
                return;

            GameController.TimeController.SetTime(args[0].Float);
        }

        [RegisterCommand(Help = "Pause/Unpause the game", MaxArgCount = 0)]
        private static void CommandPause(CommandArg[] args)
        {
            if (Terminal.IssuedError)
                return;

            GameController.TimeController.PauseOrResume();
        }

        [RegisterCommand(Name = "scene", MinArgCount = 1, MaxArgCount = 1, Help = "Load scene")]
        private static void CommandSwitchScene(CommandArg[] args)
        {
            if (Terminal.IssuedError)
                return;

            int numberOfScenes = SceneManager.sceneCountInBuildSettings;

            for (int i = 0; i < numberOfScenes; i++)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                if (sceneName == args[0].String.ToLower())
                {
                    SceneManager.LoadScene(args[0].String);//Use a specific function instead if the scene needs arguments/pre-load stuff
                    return;
                }
            }

            Debug.LogErrorFormat("No scene named {0}, or scene is invalid", args[0].String);
            CommandListScenes(null);
        }

        [RegisterCommand(Help = "Pause/Unpause the game", MaxArgCount = 0)]
        private static void CommandListScenes(CommandArg[] args)
        {
            if (Terminal.IssuedError)
                return;

            int numberOfScenes = SceneManager.sceneCountInBuildSettings;
            string[] sceneNames = new string[numberOfScenes];
            for (int i = 0; i < numberOfScenes; i++)
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                sceneNames[i] = sceneName;
            }
            Debug.Log("Scenes: " + string.Join(" ", sceneNames));
        }

        [RegisterCommand(Help = "Forcequit the game", MaxArgCount = 0)]
        private static void CommandQuit(CommandArg[] args)
        {
            if (Terminal.IssuedError)
                return;

            SceneController.QuitGame();
        }

        #region Optional Custom Commands

        /// <summary>
        /// Activate dev mode, disable achievements, etc
        /// </summary>
        /// <param name="args"></param>
        //[RegisterCommand(Name = "devmode", Help = "DevMode for testing")]
        private static void CommandActivateDevMode(CommandArg[] args)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Unlocks everything, for resting purposes
        /// </summary>
        /// <param name="args"></param>
        //[RegisterCommand(Name = "devboost", Help = "DevMode for testing")]
        private static void CommandDevModeBoost(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandNoclip(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandKill(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandSave(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandLoad(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandSetDifficulty(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand(name="restart")]
        private static void CommandRestartGame(CommandArg[] args)//Couldn't find an easy, multi-platform way of restarting the whole applcation with Unity
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandRestartSettings(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandResetProgress(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandDump(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //[RegisterCommand()]
        private static void CommandNetGraph(CommandArg[] args)
        {
            throw new NotImplementedException();
        }

        //Possible command ideas for your project (they might get implemented at some point here):
        //Set input/graphics/misc settings
        //Play out a "predefined coded" dev scenario (instead of using the scene system)


        #endregion


    }
}