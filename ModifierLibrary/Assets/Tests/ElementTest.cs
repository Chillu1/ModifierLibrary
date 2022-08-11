using UnitLibrary;
using NUnit.Framework;

namespace ModifierLibrary.Tests
{
	public class ElementTest : ModifierBaseTest
	{
		[Test]
		public void ElementResistanceComponent()
		{
			Assert.True(character.ElementalDamageResistances.ValueEquals(ElementType.Fire, 0));

			var elementModifier = modifierPrototypes.Get("ElementFireResistanceTest");
			character.AddModifier(elementModifier);

			Assert.True(character.ElementalDamageResistances.ValueEquals(ElementType.Fire, 1000));
		}

		[Test]
		public void DamageResistanceComponent()
		{
			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 0));

			var physicalModifier = modifierPrototypes.Get("DamagePhysicalResistanceTest");
			character.AddModifier(physicalModifier);

			Assert.True(character.DamageTypeDamageResistances.ValueEquals(DamageType.Physical, 1000));
		}
	}
}