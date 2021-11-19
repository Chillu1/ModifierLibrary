using BaseProject;
using BaseProject.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ComboSystem
{
    public class GameController : Singleton<GameController>
    {
        private ComboBeing player;//TEMP

        /// <summary>
        /// Actual game speed (dev/testing purposes)
        /// </summary>
        public static float GameSpeed { get { return Time.timeScale; } set { Time.timeScale = value; } }//We could limit the gamespeed at which mechanics (might) break

        public ModifierPrototypes ModifierPrototypes { get; private set; }
        public ComboModifierPrototypes ComboModifierPrototypes { get; private set; }
        public static UIController UIController { get; private set; }
        public static TimeController TimeController { get; private set; }

        public void Start()
        {
            SceneManager.sceneLoaded += delegate { GameplaySceneStarted(); };
            GameplaySceneStarted();

            CommandsController.Init();

            ModifierPrototypes = new ModifierPrototypes();
            ComboModifierPrototypes = new ComboModifierPrototypes();

            player = new ComboBeing(new ComboBeingProperties() { Id = "player", Health = 50, Damage = 5, MovementSpeed = 3});
            //Modifier mod1 = ModifierPrototypes.GetModifier("MovementSpeedOfCat");
            //player.AddModifier(mod1);
            //Modifier mod2 = ModifierPrototypes.GetModifier("AttackSpeedOfCat");
            //player.AddModifier(mod2);
            //player.ModifierController.ListModifiers();

            //Modifier fireDamage = ModifierPrototypes.GetModifier("FireDamage");
            //fireDamage.AddCondition(delegate(Modifier modifier)
            //{
            //    return modifier.Target?.StatusEffects.HasFlag(StatusEffect.Stunned) == true;//TODO Test
            //});
            //Modifier coldDamage = ModifierPrototypes.GetModifier("ColdDamage");
            //player.AddModifier(fireDamage);
            //player.AddModifier(coldDamage);

            player.AddModifier(ModifierPrototypes.GetItem("PlayerMovementSpeedDurationOnKillBuff"), AddModifierParameters.Default, ActivationCondition.Kill);

            ComboBeing spider = new ComboBeing(new ComboBeingProperties() { Id = "Spider", Health = 20, Damage = 3, MovementSpeed = 2});
            //var spiderPoisonBuff = (ModifierApplier<ModifierApplierData>)ModifierPrototypes.GetItem<ModifierApplierData>("SpiderPoisonBuff");
            //spider.AddModifierApplier(spiderPoisonBuff);
            //slimePoisonBuff.ApplyModifierToTarget(player);
            player.Attack(spider);
            player.Attack(spider);
            //Log.Info(player.MovementSpeed.Value);
            //Log.Info(spider.Health.ToString());
            player.Attack(spider);
            player.Attack(spider);
            player.Attack(spider);
            //Log.Info(player.MovementSpeed.Value);
            //Log.Info(spider.Health.ToString());
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
            //player.ModifierController.Update(Time.deltaTime);
        }
    }

}