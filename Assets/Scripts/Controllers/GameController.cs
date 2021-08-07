using ComboSystem.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ComboSystem
{
    public class GameController : Singleton<GameController>
    {
        private Player player;//TEMP

        /// <summary>
        /// Actual game speed (dev/testing purposes)
        /// </summary>
        public static float GameSpeed { get { return Time.timeScale; } set { Time.timeScale = value; } }//We could limit the gamespeed at which mechanics (might) break

        public ModifierPrototypes ModifierPrototypes { get; private set; }
        public static UIController UIController { get; private set; }
        public static TimeController TimeController { get; private set; }

        public void Start()
        {
            SceneManager.sceneLoaded += delegate { GameplaySceneStarted(); };
            GameplaySceneStarted();

            CommandsController.Init();

            ModifierPrototypes = new ModifierPrototypes();

            player = new Player();
            //Modifier playerSpeedBuff = ModifierPrototypes.GetModifier("PlayerMovementSpeedDurationBuff");
            //player.AddModifier(playerSpeedBuff);
            player.AddModifier(ModifierPrototypes.GetModifier("MovementSpeedBuff"));
            player.AddModifier(ModifierPrototypes.GetModifier("RefreshableMovementSpeedBuff"));
            player.AddModifier(ModifierPrototypes.GetModifier("RefreshableMovementSpeedBuff2"));

            Slime slime = new Slime();
            var slimePoisonBuff = (ModifierApplier<ModifierApplierData>)ModifierPrototypes.GetModifier<ModifierApplierData>("SlimePoisonBuff");
            slime.AddModifier(slimePoisonBuff, false);
            //slimePoisonBuff.ApplyModifierToTarget(player);
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
            player.ModifierController.Update(Time.deltaTime);
        }
    }

}