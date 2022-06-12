using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ChanceTest : ModifierBaseTest
    {
        [Test]
        public void ChanceZeroThorns()
        {
            var thornsOnHitModifier = modifierPrototypes.Get("ThornsOnHitChanceZeroTest");
            character.AddModifier(thornsOnHitModifier);
            enemy.Attack(character);

            Assert.AreEqual(initialHealthEnemy, enemy.Stats.Health.CurrentHealth);
        }

        [Test]
        public void ChanceHalfIncDmg()
        {
            BaseBeingEventController.MaxEventRecursions = 100;
            character.ChangeStat(StatType.Health, 200);
            var dmgHitModifier = modifierPrototypes.Get("IncreaseDmgHitHalfTest");
            enemy.AddModifier(dmgHitModifier);

            for (int i = 0; i < 100; i++)
            {
                enemy.Attack(character);
            }

            //Between 1 and 99 damage added (from attacking)
            Assert.Greater(enemy.Stats.Damage.DamageSum(), initialDamageEnemy);
            Assert.Less(enemy.Stats.Damage.DamageSum(), initialDamageEnemy + 100);

            BaseBeingEventController.MaxEventRecursions = BaseBeingEventController.MaxEventRecursionsDefault;
        }

        [Test]
        public void ChanceHalfHeal()
        {
            BaseBeingEventController.MaxEventRecursions = 100;

            var healthCostModifier = modifierPrototypes.Get("HealOnHitHalfChanceTest");
            character.AddModifier(healthCostModifier);

            for (int i = 0; i < 10; i++)
                enemy.Attack(character);

            enemy.ChangeDamageStat(new DamageData(-1, DamageType.Physical));//Reduce dmg to 0
            //try to heal 100 times, should have healed at least once
            for (int i = 0; i < 100; i++)
                enemy.Attack(character);

            Assert.Greater(character.Stats.Health.CurrentHealth, initialHealthCharacter-initialDamageEnemy*10);

            BaseBeingEventController.MaxEventRecursions = BaseBeingEventController.MaxEventRecursionsDefault;
        }

        [Test]
        public void ChanceFullIncDmg()
        {
            BaseBeingEventController.MaxEventRecursions = 100;
            character.ChangeStat(StatType.Health, 200);
            var dmgHitModifier = modifierPrototypes.Get("IncreaseDmgHitFullTest");
            enemy.AddModifier(dmgHitModifier);

            for (int i = 0; i < 100; i++)
                enemy.Attack(character);

            Assert.AreEqual(initialDamageEnemy+100, enemy.Stats.Damage.DamageSum(), Delta);

            BaseBeingEventController.MaxEventRecursions = BaseBeingEventController.MaxEventRecursionsDefault;
        }

        [Test]
        public void ChanceAttackHalfHitHalfApply()
        {
            character.ChangeStat(StatType.Health, 20000);

            var fireAttack = modifierPrototypes.GetApplier("FireAttackChanceToHitHalfTest");
            enemy.AddModifier(fireAttack);

            for (int i = 0; i < 5000; i++)
            {
                //5000 attacks, 1 physical dmg, 50% chance to hit 50% chance to apply >= 25% chance 1dmg
                //effective dmg = 6250 on average
                enemy.Attack(character);
            }

            Assert.True(character.ElementController.HasAnyIntensity(ElementalType.Fire));

            //20000 to 15000, 20000
            Assert.Less(character.Stats.Health.CurrentHealth, initialHealthCharacter+20000 - initialDamageEnemy * 5000);
            //20000 to 15000, 17500
            Assert.Greater(character.Stats.Health.CurrentHealth, initialHealthCharacter+20000 - initialDamageEnemy * 5000 - initialDamageEnemy * 2500);
        }

        [Test]
        public void ChanceAttackFullHitZeroApply()
        {
            character.ChangeStat(StatType.Health, 2000);

            var fireAttack = modifierPrototypes.GetApplier("FireAttackChanceToHitFullZeroTest");
            enemy.AddModifier(fireAttack);

            for (int i = 0; i < 500; i++)
            {
                //5000 attacks, 1 physical dmg, 100% chance to hit 0% chance to apply >= 0% chance 1dmg
                //effective dmg = 500
                enemy.Attack(character);
            }

            Assert.False(character.ElementController.HasAnyIntensity(ElementalType.Fire));
            Assert.AreEqual(initialHealthCharacter+2000-500, character.Stats.Health.CurrentHealth, Delta);
        }

        [Test]
        public void ChanceAttackZeroHitFullApply()
        {
            character.ChangeStat(StatType.Health, 2000);

            var fireAttack = modifierPrototypes.GetApplier("FireAttackChanceToHitZeroFullTest");
            enemy.AddModifier(fireAttack);

            for (int i = 0; i < 500; i++)
            {
                //5000 attacks, 1 physical dmg, 0% chance to hit 100% chance to apply >= 0% chance 1dmg
                //effective dmg = 500
                enemy.Attack(character);
            }

            Assert.False(character.ElementController.HasAnyIntensity(ElementalType.Fire));
            Assert.AreEqual(initialHealthCharacter+2000-500, character.Stats.Health.CurrentHealth, Delta);
        }

        [Test]
        public void ChanceAttackFullHitFullApply()
        {
            character.ChangeStat(StatType.Health, 2000);

            var fireAttack = modifierPrototypes.GetApplier("FireAttackChanceToHitFullFullTest");
            enemy.AddModifier(fireAttack);

            for (int i = 0; i < 500; i++)
            {
                //5000 attacks, 1 physical dmg, 100% chance to hit 100% chance to apply >= 100% chance 1dmg
                //effective dmg = 1000
                enemy.Attack(character);
            }

            Assert.True(character.ElementController.HasAnyIntensity(ElementalType.Fire));
            Assert.AreEqual(initialHealthCharacter+2000-1000, character.Stats.Health.CurrentHealth, Delta);
        }
    }
}