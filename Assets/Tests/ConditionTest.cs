using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ConditionTest : ModifierBaseTest
    {
        [Test]
        public void ConditionDamageOnLowHealth()
        {
            var modifier = modifierPrototypes.GetItem("DamageOnLowHealthTest");
            character.AddModifier(modifier);
            enemy.ChangeDamageStat(new DamageData(initialHealthCharacter-3, DamageType.Physical));

            enemy.Attack(character);

            Assert.AreEqual(initialDamageCharacter+50, character.Stats.Damage.DamageSum(), Delta);
        }

        [Test]
        public void ConditionModifierId()
        {
            var flagModifier = modifierPrototypes.GetItem("FlagTest");
            var modifier = modifierPrototypes.GetItem("DamageOnModifierIdTest");
            character.AddModifier(modifier);//No flag, returns effect (has modifier)
            Assert.AreEqual(initialDamageCharacter, character.Stats.Damage.DamageSum(), Delta);

            character.AddModifier(flagModifier);
            Assert.True(character.ContainsModifier(flagModifier));

            character.AddModifier(modifier);//Has flag modifier now, add

            Assert.AreEqual(double.MaxValue, character.Stats.Damage.DamageSum(), Delta);
        }

        [Test]
        public void ConditionElementalIntensity()
        {
            var modifier = modifierPrototypes.GetItem("DealDamageOnElementalIntensityTest");
            character.AddModifier(modifier);

            character.Attack(enemy);
            Assert.False(enemy.Stats.Health.IsDead);

            character.ChangeDamageStat(new DamageData(1, DamageType.Physical, new ElementData(ElementalType.Fire, 1000, 10)));
            character.Attack(enemy);

            Assert.True(enemy.Stats.Health.IsDead);
        }
    }
}