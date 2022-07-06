using System.Text.RegularExpressions;
using BaseProject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ModifierSystem.Tests
{
    public class TargetTest : ModifierBaseTest
    {
        [Test]
        public void AllySameResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetAutomaticCastAll();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                ally.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }

        [Test]
        public void EnemySameResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            enemy.AddModifier(applier);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                enemy.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            enemy.TargetingSystem.AddAllyAuraTarget(enemyAlly);
            enemy.TargetingSystem.AddEnemyAuraTarget(character);
            enemy.SetAutomaticCastAll();

            enemy.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                enemy.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                enemyAlly.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }

        [Test]
        public void OppositeResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("OppositePhysicalDamageResistanceAuraTest");
            enemy.AddModifier(applier);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                enemy.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            enemy.TargetingSystem.AddAllyAuraTarget(enemyAlly);
            enemy.TargetingSystem.AddEnemyAuraTarget(character);
            enemy.SetAutomaticCastAll();

            enemy.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                enemy.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                enemyAlly.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }

        [Test]
        public void EveryoneResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("EveryonePhysicalDamageResistanceAuraTest");
            enemy.AddModifier(applier);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                enemy.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            enemy.TargetingSystem.AddAllyAuraTarget(enemyAlly);
            enemy.TargetingSystem.AddEnemyAuraTarget(character);
            enemy.SetAutomaticCastAll();

            enemy.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                enemy.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                enemyAlly.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }

        [Test]
        public void AllySameResistanceAuraTimeoutTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetAutomaticCastAll();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                ally.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            ally.Update((float)AuraEffectModifierGenerationProperties.AuraRemoveTime);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                ally.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }

        [Test]
        public void AllySameResistanceAuraReplenishTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetAutomaticCastAll();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                ally.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            ally.Update((float)AuraEffectModifierGenerationProperties.AuraRemoveTime * 2);

            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                ally.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }

        [Test]
        public void AllySameResistanceAuraRepeatTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(0),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);

            character.TargetingSystem.AddAllyAuraTarget(ally);
            character.SetAutomaticCastAll();
            character.Update(CastingController.AutomaticAuraCastCooldown);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                ally.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            
            //TODOPRIO
            //EffectPropertyInfo in ModGenProperties, is being reused, "so we need to clone it to make it unique"
            //Or no reference types in ModGenProperties, excluding modifier info
            
            character.Update(CastingController.AutomaticAuraCastCooldown * 2);

            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
            Assert.AreEqual(1d - BaseProject.Curves.DamageResistance.Evaluate(100),
                ally.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }

        [Test]
        public void SelfManaBurnBeings()
        {
            var applier = modifierPrototypes.GetApplier("TimePercentFlatManaBurnBeingsTest");
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
            character.Update((float)character.Stats.AttackSpeed);
            
            Assert.False(character.Stats.Health.IsFull);
            LogAssert.Expect(LogType.Error, new Regex("Targeting ourselves*"));
            Assert.True(character.Stats.Mana.IsFull);//Applier doesn't work on self
        }
    }
}