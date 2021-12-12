using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ComboModifierTest : ModifierBaseTest
    {
        // [Test]
        // public void CheckComboModifierAspectOfCat()
        // {
        //     var movementSpeedOfCat = modifierPrototypes.GetItem("TestMovementSpeedOfCat");
        //     var attackSpeedOfCat = modifierPrototypes.GetItem("TestAttackSpeedOfCat");
        //     character.AddModifier(movementSpeedOfCat);
        //     character.AddModifier(attackSpeedOfCat);
        //
        //     Assert.True(character.ContainsModifier(comboModifierPrototypes.GetItem("TestAspectOfTheCat")));
        // }

        [Test]
        public void CheckComboModifierByElements()
        {
            //Attack with poison & bleed
            var poisonAttackApplier = modifierPrototypes.GetItem("PoisonApplier");
            var bleedAttackApplier = modifierPrototypes.GetItem("BleedApplier");
            character.AddModifier(poisonAttackApplier, AddModifierParameters.DefaultOffensive);
            character.AddModifier(bleedAttackApplier, AddModifierParameters.DefaultOffensive);
            character.Attack(enemy);

            //Check for infected comboMod
            Assert.True(enemy.ContainsModifier("Infection"));

            //var fireAttackApplier = modifierPrototypes.GetModifierApplier("TestFireDamageApplier");
            //var coldAttackApplier = modifierPrototypes.GetModifierApplier("TestColdDamageApplier");
            //character.AddModifierApplier(fireAttackApplier);
            //character.AddModifierApplier(coldAttackApplier);
            //character.Attack(enemy);
            //
            //Assert.True(enemy.ContainsModifier(comboModifierPrototypes.GetItem("TestExplosion")));
        }
    }
}