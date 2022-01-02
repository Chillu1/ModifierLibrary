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
    }
}