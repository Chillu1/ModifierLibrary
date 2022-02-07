using System;
using BaseProject;
using UnityEngine;
using ModifierSystem;
using Random = System.Random;

namespace ModifierSystem.Sample
{
    public class GameController : MonoBehaviour
    {
        public static Random GlobalRandom;

        private BeingController _beingController;

        //public ModifierPrototypes ModifierPrototypes { get; private set; }

        private float _timer;

        public void Start()
        {
            int seed = Environment.TickCount;
            GlobalRandom = new Random(seed);

            var modifiers = new ModifierPrototypesBase(true);
            var _ = new ComboModifierPrototypes();

            //ModifierPrototypes = new ModifierPrototypes();
            _beingController = new BeingController();
            Being character = (Being)_beingController.SpawnBaseBeing(new BeingProperties()
            {
                Id = "Player", DamageData = new DamageData(20, DamageType.Physical, new ElementData(ElementalType.Fire, 10, 10)),
                Health = 50_000_000, UnitType = UnitType.Ally
            }, false);
            //TODO Add random modifier to player, then enemies
            var disarmModifier = modifiers.Get("DisarmModifierTest");
            var damageOnKillModifier = modifiers.Get("DamageOnKillTest");

            //var modifier = modifiers.GetRandomApplier(GlobalRandom);
            character.AddModifier(disarmModifier);
            character.AddModifier(damageOnKillModifier);

            for (int i = 0; i < 100; i++)
            {
                var thornsModifier = modifiers.Get("ThornsOnHitTest");
                var dotStackingModifier = modifiers.Get("DoTStackTestApplier");
                var enemy = (Being)_beingController.SpawnBaseBeing(new BeingProperties()
                {
                    Id = "Enemy", DamageData = new DamageData(20, DamageType.Physical, new ElementData(ElementalType.Fire, 10, 10)),
                    Health = 30, UnitType = UnitType.Enemy
                }, true);

                enemy.AddModifier(thornsModifier);
                enemy.AddModifier(dotStackingModifier, AddModifierParameters.NullStartTarget);
            }
        }

        private void Update()
        {
            _beingController.Update(Time.deltaTime);
        }
    }
}