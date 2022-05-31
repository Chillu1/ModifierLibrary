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

        private ModifierPrototypes<Modifier> _modifiers;

        private float _timer;
        private Being _character;

        public void Start()
        {
            int seed = Environment.TickCount;
            GlobalRandom = new Random(seed);

            _modifiers = new ModifierPrototypes<Modifier>(true);
            var _ = new ComboModifierPrototypes();

            //ModifierPrototypes = new ModifierPrototypes();
            _beingController = new BeingController(_modifiers);
            _character = (Being)_beingController.SpawnBaseBeing(new BeingProperties()
            {
                Id = "Player", DamageData = new DamageData(20, DamageType.Physical, new ElementData(ElementalType.Fire, 10, 10)),
                Health = 50_000_000, UnitType = UnitType.Ally
            }, false);
            //TODO Add random modifier to player, then enemies
            var disarmModifier = _modifiers.Get("DisarmModifierTest");
            var damageOnKillModifier = _modifiers.Get("DamageOnKillTest");

            //var modifier = modifiers.GetRandomApplier(GlobalRandom);
            _character.AddModifier(disarmModifier);
            _character.AddModifier(damageOnKillModifier);
        }

        private void Update()
        {
            //Debug.Log(_character.Stats.Health);
            _beingController.Update(Time.deltaTime);
        }
    }
}