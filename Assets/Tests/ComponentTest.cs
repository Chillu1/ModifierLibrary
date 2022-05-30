using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using BaseProject;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace ModifierSystem.Tests
{
    //TargetComponent
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class ComponentTest : ModifierBaseTest
    {
        // [Test]
        // public void TargetNotValidAlly()
        // {
        //     var damageModifierApplier = modifierPrototypes.GetItem("IceBoltTestApplier");
        //     character.AddModifier(damageModifierApplier, AddModifierParameters.NullStartTarget);
        //     //We can attack our own allies, but we shouldn't apply modifiers that aren't for our allies
        //     character.Attack(ally);
        //     Assert.AreEqual(initialHealthAlly - initialDamageAlly, ally.Stats.Health.CurrentHealth, Delta);
        // }

        [Test]
        public void RemoveComponentContains()
        {
            var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            enemy.Update(9.9f);

            var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
            Assert.True(enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void RemoveComponentEffect()
        {
            var doTModifierApplier = modifierPrototypes.Get("SpiderPoisonTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            enemy.Update(10.1f);

            var doTModifier = modifierPrototypes.Get("SpiderPoisonTest");
            Assert.False(enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void RemoveComponentLingerEffect()
        {
            var doTModifierApplier = modifierPrototypes.Get("IceBoltTestApplier");
            character.AddModifier(doTModifierApplier, AddModifierParameters.NullStartTarget);
            character.Attack(enemy);
            enemy.Update(0.4f);
            var doTModifier = modifierPrototypes.Get("IceBoltTest");
            Assert.True(enemy.ContainsModifier(doTModifier));
            enemy.Update(0.2f);
            Assert.False(enemy.ContainsModifier(doTModifier));
        }

        [Test]
        public void HealStatBasedEffect()
        {
            Assert.True(ally.Stats.Health.IsFull);
            var healModifier = modifierPrototypes.Get("HealStatBasedTest");
            character.ChangeStat(StatType.Heal, 5);
            character.AddModifier(healModifier);
            enemy.ChangeDamageStat(new DamageData(4, DamageType.Physical));
            enemy.Attack(ally);
            character.Heal(ally);

            Assert.True(ally.Stats.Health.IsFull);
        }

        [Test]
        public void HealthCost()
        {
            //On attack, we should try to apply the modifier, check for cost, then apply, then take the cost.
            var healthCostModifier = modifierPrototypes.Get("IceBoltHealthCostTestApplier");
            character.AddModifier(healthCostModifier, AddModifierParameters.DefaultOffensive);

            Assert.True(character.Stats.Health.IsFull);
            character.Attack(enemy);

            Assert.AreEqual(initialHealthCharacter - 10, character.Stats.Health.CurrentHealth, Delta); //cost component took away 10 health
            Assert.AreNotEqual(initialHealthEnemy, enemy.Stats.Health.CurrentHealth); //enemy took damage
        }

        [Test]
        public void HealthCostNotLethal()
        {
            var healthCostModifier = modifierPrototypes.Get("IceBoltHealthCostTestApplier");
            character.AddModifier(healthCostModifier, AddModifierParameters.DefaultOffensive);

            Assert.True(character.Stats.Health.IsFull);
            for (int i = 0; i < 7; i++)
            {
                character.Attack(enemy);
            }

            Assert.False(character.Stats.Health.IsDead);
        }

        [Test]
        public void ManaCost()
        {
            var healthCostModifier = modifierPrototypes.Get("IceBoltManaCostTestApplier");
            character.AddModifier(healthCostModifier, AddModifierParameters.DefaultOffensive);

            Assert.True(character.Stats.Mana.IsFull);
            character.CastModifier(enemy, "IceBoltManaCostTestApplier");

            Assert.AreEqual(initialManaCharacter - 10, character.Stats.Mana.CurrentMana, Delta); //cost component used up 10 mana
            Assert.False(character.Stats.Mana.IsFull);
        }
    }
}