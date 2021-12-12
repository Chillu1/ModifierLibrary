using BaseProject;
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
            var poisonAttackApplier = modifierPrototypes.GetItem("PoisonTestApplier");
            var bleedAttackApplier = modifierPrototypes.GetItem("BleedTestApplier");
            character.AddModifier(poisonAttackApplier, AddModifierParameters.DefaultOffensive);
            character.AddModifier(bleedAttackApplier, AddModifierParameters.DefaultOffensive);
            character.Attack(enemy);

            //Check for infected comboMod
            Assert.True(enemy.ContainsModifier("InfectionTest"));
        }

        [Test]
        public void CheckComboModifierDoT()
        {
            var infection = comboModifierPrototypesTest.GetItem("InfectionTest");
            Log.Info(infection.TargetComponent.GetTarget());
            //Attack with poison & bleed
            var poisonAttackApplier = modifierPrototypes.GetItem("PoisonTestApplier");//2 dmg per 2 secs
            var bleedAttackApplier = modifierPrototypes.GetItem("BleedTestApplier");//2 dmg per 2 secs
            character.AddModifier(poisonAttackApplier, AddModifierParameters.DefaultOffensive);
            character.AddModifier(bleedAttackApplier, AddModifierParameters.DefaultOffensive);
            Log.Info(enemy.CurrentHealth);
            character.Attack(enemy);//1 damage physical, 2*2 damage with normal modifiers (init), 10 damage infection (init)
            Log.Info(enemy.CurrentHealth);
            enemy.Update(2.1f);//infection, 10 damage (time), 2*2
            Log.Info(enemy.CurrentHealth);

            Assert.AreEqual(29, initialHealthEnemy-enemy.CurrentHealth);
        }
    }
}