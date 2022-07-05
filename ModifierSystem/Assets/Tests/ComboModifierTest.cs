using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ComboModifierTest : ModifierBaseTest
    {
        [Test]
        public void ComboModifierAspectOfCat()
        {
            var movementSpeedOfCat = modifierPrototypes.Get("MovementSpeedOfCatTest");
            var attackSpeedOfCat = modifierPrototypes.Get("AttackSpeedOfCatTest");
            character.AddModifier(movementSpeedOfCat);
            Assert.True(character.Stats.HasStat(StatType.MovementSpeed, 5));
            character.AddModifier(attackSpeedOfCat);

            Assert.True(character.ContainsModifier(comboModifierPrototypes.Get("ComboAspectOfTheCatTest")));
            Assert.True(character.Stats.HasStat(StatType.MovementSpeed, 15));
        }

        [Test]
        public void ComboModifierByElements()
        {
            //Attack with poison & bleed
            var poisonAttackApplier = modifierPrototypes.Get("PoisonTestApplier");
            var bleedAttackApplier = modifierPrototypes.Get("BleedTestApplier");
            character.AddModifierWithParameters(poisonAttackApplier, AddModifierParameters.CheckRecipes | AddModifierParameters.NullStartTarget);
            character.AddModifierWithParameters(bleedAttackApplier, AddModifierParameters.CheckRecipes | AddModifierParameters.NullStartTarget);
            character.Attack(enemy);

            //Check for infected comboMod
            Assert.True(enemy.ContainsModifier("ComboInfectionTest"));
        }

        [Test]
        public void ComboModifierDoTInit()
        {
            //Attack with poison & bleed
            var poisonAttackApplier = modifierPrototypes.Get("PoisonTestApplier"); //2 dmg per 2 secs
            var bleedAttackApplier = modifierPrototypes.Get("BleedTestApplier"); //2 dmg per 2 secs
            character.AddModifierWithParameters(poisonAttackApplier, AddModifierParameters.CheckRecipes | AddModifierParameters.NullStartTarget);
            character.AddModifierWithParameters(bleedAttackApplier, AddModifierParameters.CheckRecipes | AddModifierParameters.NullStartTarget);
            character.Attack(enemy); //1 damage physical, 2*2 damage with normal modifiers (init), 10 damage infection * multiplier (init)

            Assert.AreEqual(initialHealthEnemy, enemy.Stats.Health.CurrentHealth+1+2*2+10*Curves.ComboElementMultiplier.Evaluate(5));
        }

        [Test]
        public void ComboModifierDoT()
        {
            //Attack with poison & bleed
            var poisonAttackApplier = modifierPrototypes.Get("PoisonTestApplier"); //2 dmg per 2 secs
            var bleedAttackApplier = modifierPrototypes.Get("BleedTestApplier"); //2 dmg per 2 secs
            character.AddModifierWithParameters(poisonAttackApplier, AddModifierParameters.CheckRecipes | AddModifierParameters.NullStartTarget);
            character.AddModifierWithParameters(bleedAttackApplier, AddModifierParameters.CheckRecipes | AddModifierParameters.NullStartTarget);
            character.Attack(enemy); //1 damage physical, 2*2 damage with normal modifiers (init), 10 damage infection (init)
            enemy.Update(2.1f); //infection, 10 damage (time), 2*2

            Assert.Less(enemy.Stats.Health.CurrentHealth+1+2*2+2*2, initialHealthEnemy);
        }

        [Test]
        public void ComboModifierStats()
        {
            character.ChangeStat(StatType.Health, 10000);
            Assert.True(character.ContainsModifier(comboModifierPrototypes.Get("ComboGiantTest")));
        }

        [Test]
        public void ComboModifierCooldown()
        {
            var timedGiantComboModifier = comboModifierPrototypes.Get("ComboTimedGiantTest");
            character.ChangeStat(StatType.Health, 10000);
            Assert.True(character.ContainsModifier(timedGiantComboModifier));
            character.Update(PermanentComboModifierCooldown / 2);
            Assert.False(character.ContainsModifier(timedGiantComboModifier));
            character.ChangeStat(StatType.Health, 1); //ComboCheck (GiantTest can be added without cooldown check)
            Assert.False(character.ContainsModifier(timedGiantComboModifier));
            character.Update(PermanentComboModifierCooldown);
            character.ChangeStat(StatType.Health, 1); //ComboCheck
            Assert.True(character.ContainsModifier(timedGiantComboModifier));
        }
    }
}