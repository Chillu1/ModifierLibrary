using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ComboModifierTest : ModifierBaseTest
    {
        [Test]
        public void CheckComboModifierAspectOfCat()
        {
            var movementSpeedOfCat = modifierPrototypes.GetItem("MovementSpeedOfCatTest");
            var attackSpeedOfCat = modifierPrototypes.GetItem("AttackSpeedOfCatTest");
            character.AddModifier(movementSpeedOfCat);
            Assert.True(character.CheckStat(StatType.MovementSpeed, 5));
            character.AddModifier(attackSpeedOfCat);

            Assert.True(character.ContainsModifier(comboModifierPrototypesTest.GetItem("AspectOfTheCatTest")));
            Assert.True(character.CheckStat(StatType.MovementSpeed, 15));
        }

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
            //Attack with poison & bleed
            var poisonAttackApplier = modifierPrototypes.GetItem("PoisonTestApplier");//2 dmg per 2 secs
            var bleedAttackApplier = modifierPrototypes.GetItem("BleedTestApplier");//2 dmg per 2 secs
            character.AddModifier(poisonAttackApplier, AddModifierParameters.DefaultOffensive);
            character.AddModifier(bleedAttackApplier, AddModifierParameters.DefaultOffensive);
            character.Attack(enemy);//1 damage physical, 2*2 damage with normal modifiers (init), 10 damage infection (init)
            enemy.Update(2.1f);//infection, 10 damage (time), 2*2

            Assert.AreEqual(29, initialHealthEnemy-enemy.Health.CurrentHealth);
        }
    }
}