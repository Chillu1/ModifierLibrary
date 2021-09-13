using NUnit.Framework;

namespace ComboSystem.Tests
{
    public class ComboModifierTest : ModifierBaseTest
    {
        [Test]
        public void CheckComboModifierAspectOfCat()
        {
            var movementSpeedOfCat = modifierPrototypes.GetItem("MovementSpeedOfCat");
            var attackSpeedOfCat = modifierPrototypes.GetItem("AttackSpeedOfCat");
            character.AddModifier(movementSpeedOfCat);
            character.AddModifier(attackSpeedOfCat);

            Assert.True(character.ContainsModifier(comboModifierPrototypes.GetItem("AspectOfTheCat")));
        }

        [Test]
        public void CheckComboModifierExplosion()
        {
            var fireAttackApplier = modifierPrototypes.GetModifierApplier("FireDamageApplier");
            var coldAttackApplier = modifierPrototypes.GetModifierApplier("ColdDamageApplier");
            character.AddModifierApplier(fireAttackApplier);
            character.AddModifierApplier(coldAttackApplier);
            character.Attack(enemy);

            Assert.True(enemy.ContainsModifier(comboModifierPrototypes.GetItem("Explosion")));
        }
    }
}