using BaseProject;
using NUnit.Framework;

namespace ModifierSystem.Tests
{
    public class ElementTest : ModifierBaseTest
    {
        [Test]
        public void ElementResistanceComponent()
        {
            Assert.True(character.ElementalDamageResistances.IsValue(ElementType.Fire, 0));

            var elementModifier = modifierPrototypes.Get("ElementFireResistanceTest");
            character.AddModifier(elementModifier);

            Assert.True(character.ElementalDamageResistances.IsValue(ElementType.Fire, 1000));
        }

        [Test]
        public void DamageResistanceComponent()
        {
            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 0));

            var physicalModifier = modifierPrototypes.Get("DamagePhysicalResistanceTest");
            character.AddModifier(physicalModifier);

            Assert.True(character.DamageTypeDamageResistances.IsValue(DamageType.Physical, 1000));
        }
    }
}