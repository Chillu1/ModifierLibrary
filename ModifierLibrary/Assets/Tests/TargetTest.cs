using System.Text.RegularExpressions;
using UnitLibrary;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ModifierLibrary.Tests
{
    public class TargetTest : ModifierBaseTest
    {
        [Test]
        public void AllySameResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetGlobalAutomaticCast();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(ally.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
        }

        [Test]
        public void EnemySameResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            enemy.AddModifier(applier);

            Assert.True(enemy.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            enemy.TargetingSystem.AddAllyAuraTarget(enemyAlly);
            enemy.TargetingSystem.AddEnemyAuraTarget(character);
            enemy.SetGlobalAutomaticCast();

            enemy.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(enemy.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(enemyAlly.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
        }

        [Test]
        public void OppositeResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("OppositePhysicalDamageResistanceAuraTest");
            enemy.AddModifier(applier);

            Assert.True(enemy.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            enemy.TargetingSystem.AddAllyAuraTarget(enemyAlly);
            enemy.TargetingSystem.AddEnemyAuraTarget(character);
            enemy.SetGlobalAutomaticCast();

            enemy.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(enemy.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
            Assert.True(enemyAlly.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
        }

        [Test]
        public void EveryoneResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("EveryonePhysicalDamageResistanceAuraTest");
            enemy.AddModifier(applier);

            Assert.True(enemy.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            enemy.TargetingSystem.AddAllyAuraTarget(enemyAlly);
            enemy.TargetingSystem.AddEnemyAuraTarget(character);
            enemy.SetGlobalAutomaticCast();

            enemy.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(enemy.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(enemyAlly.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
        }

        [Test]
        public void AllySameResistanceAuraTimeoutTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetGlobalAutomaticCast();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(ally.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));

            ally.Update((float)AuraEffectModifierGenerationProperties.AuraRemoveTime);

            Assert.True(ally.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));
        }

        [Test]
        public void AllySameResistanceAuraReplenishTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetGlobalAutomaticCast();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(ally.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));

            ally.Update((float)AuraEffectModifierGenerationProperties.AuraRemoveTime * 2);

            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(ally.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
        }

        [Test]
        public void AllySameResistanceAuraRepeatTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetGlobalAutomaticCast();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(ally.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            
            character.Update(CastingController.AutomaticAuraCastCooldown * 2);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
            Assert.True(ally.DamageTypeDamageResistances.IsValue(DamageType.Physical, 100));
        }

        [Test]
        public void SelfManaBurnUnits()
        {
            var applier = modifierPrototypes.GetApplier("TimePercentFlatManaBurnUnitsTest");
            character.AddModifier(applier);

            Assert.True(character.Stats.Mana.IsFull);

            character.TargetingSystem.SetTarget(TargetType.Attack, character);
            character.Update((float)character.Stats.AttackSpeed);
            
            Assert.False(character.Stats.Health.IsFull);
            Assert.False(character.Stats.Mana.IsFull);
        }
        
        [Test]
        public void SelfManaBurnOpposite()
        {
            var applier = modifierPrototypes.GetApplier("TimePercentFlatManaBurnOppositeTest");
            character.AddModifier(applier);

            Assert.True(character.Stats.Mana.IsFull);
            character.TargetingSystem.SetTarget(TargetType.Attack, character);
            LogAssert.Expect(LogType.Error, new Regex("Targeting ourselves*"));
            character.Update((float)character.Stats.AttackSpeed);
            
            Assert.False(character.Stats.Health.IsFull);
            Assert.True(character.Stats.Mana.IsFull);//Applier doesn't work on self
        }
    }
}