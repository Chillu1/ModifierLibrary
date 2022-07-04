using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class TargetTest : ModifierBaseTest
    {
        [Test]
        public void AllySameResistanceAuraTest()
        {
            var applier = modifierPrototypes.GetApplier("FriendlyPhysicalDamageResistanceAuraTest");
            character.AddModifier(applier, AddModifierParameters.NullStartTarget);

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
            enemy.AddModifier(applier, AddModifierParameters.NullStartTarget);

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
            enemy.AddModifier(applier, AddModifierParameters.NullStartTarget);

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
            enemy.AddModifier(applier, AddModifierParameters.NullStartTarget);

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
    }
}