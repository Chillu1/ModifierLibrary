using System;
using System.Collections.Generic;
using BaseProject;
using BaseProject.Utils;
using JetBrains.Annotations;
using static ModifierSystem.Sample.GameController;

namespace ModifierSystem.Sample
{
    public class BeingController
    {
        private readonly List<BaseBeing> _characters;
        private readonly List<BaseBeing> _enemies;

        private readonly List<BaseBeing> _deadCharacters;
        private readonly List<BaseBeing> _deadEnemies;

        private float _enemySpawnTimer;
        private float _enemySpawnCooldown = 0.1f;

        private readonly BeingProperties _enemyProperties = new BeingProperties()
        {
            Id = "Enemy no init #",
            UnitType = UnitType.Enemy
        };

        private readonly ModifierPrototypes<Modifier> _modifiers;

        private int _enemyCount;

        public BeingController(ModifierPrototypes<Modifier> modifiers)
        {
            _modifiers = modifiers;
            _characters = new List<BaseBeing>(1);
            _enemies = new List<BaseBeing>(10);

            _deadCharacters = new List<BaseBeing>(1);
            _deadEnemies = new List<BaseBeing>(10);
        }

        public void Update(float deltaTime)
        {
            UpdateBeings(deltaTime, _characters, _deadCharacters);
            UpdateBeings(deltaTime, _enemies, _deadEnemies);

            _enemySpawnTimer += deltaTime;

            if (_enemySpawnTimer < _enemySpawnCooldown)
                return;

            _enemySpawnTimer = 0;
            _enemyProperties.Id = "Enemy #" + _enemyCount;
            _enemyProperties.Health = GlobalRandom.Next(10, 100);
            _enemyProperties.DamageData = new DamageData(GlobalRandom.Next(5, 25), DamageType.Physical);
            var enemy = (Being)SpawnBaseBeing(_enemyProperties, true);

            var thornsModifier = _modifiers.Get("ThornsOnHitTest");
            var dotStackingModifier = _modifiers.Get("DoTStackTestApplier");

            enemy.AddModifier(thornsModifier);
            enemy.AddModifier(dotStackingModifier, AddModifierParameters.NullStartTarget);
            //being.TargetingSystem.SetupTargets(GetRandomCharacterTarget);
            //TODO Add random modifier

            _enemyCount++;
        }

        public BaseBeing SpawnBaseBeing(BeingProperties properties, bool isEnemy)
        {
            Being being = new Being(properties);

            if (isEnemy)
            {
                being.TargetingSystem.SetupTargets(GetRandomCharacterTarget, GetRandomCharacterTarget, GlobalRandom.NextFloat(0.05f, 0.3f));
                _enemies.Add(being);
            }
            else
            {
                being.TargetingSystem.SetupTargets(GetRandomEnemyTarget, GetRandomEnemyTarget, GlobalRandom.NextFloat(0.05f, 0.3f));
                _characters.Add(being);
            }

            return being;
        }

        [CanBeNull]
        public BaseBeing GetRandomCharacterTarget()
        {
            return _characters.Count == 0 ? null : _characters.RandomElement(GlobalRandom);
        }

        [CanBeNull]
        public BaseBeing GetRandomEnemyTarget()
        {
            return _enemies.Count == 0 ? null : _enemies.RandomElement(GlobalRandom);
        }

        private void UpdateBeings(in float deltaTime, List<BaseBeing> beings, List<BaseBeing> deadBeings)
        {
            for (int i = 0; i < beings.Count; i++)
            {
                var being = beings[i];
                being.Update(deltaTime);
                if (being.Stats.Health.IsDead)
                    deadBeings.Add(being);
            }

            for (int j = 0; j < deadBeings.Count; j++)
            {
                //Log.Info(deadBeings[j].Id + " died");
                beings.Remove(deadBeings[j]);
            }

            deadBeings.Clear();
        }
    }
}