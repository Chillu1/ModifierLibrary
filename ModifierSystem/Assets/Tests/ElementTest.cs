using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ElementTest : ModifierBaseTest
    {
        [Test]
        public void ElementResistanceComponent()
        {
            Assert.True(character.ElementalDamageResistances.HasValue(ElementType.Fire, 0));

            var elementModifier = modifierPrototypes.Get("ElementFireResistanceTest");
            character.AddModifier(elementModifier);

            Assert.AreEqual(BaseProject.Curves.DamageResistance.Evaluate(1000),
                character.ElementalDamageResistances.GetDamageMultiplier(ElementType.Fire), Delta);
        }

        [Test]
        public void DamageResistanceComponent()
        {
            Assert.True(character.DamageTypeDamageResistances.HasValue(DamageType.Physical, 0));

            var physicalModifier = modifierPrototypes.Get("DamagePhysicalResistanceTest");
            character.AddModifier(physicalModifier);

            Assert.AreEqual(BaseProject.Curves.DamageResistance.Evaluate(1000),
                character.DamageTypeDamageResistances.GetDamageMultiplier(DamageType.Physical), Delta);
        }
    }
}