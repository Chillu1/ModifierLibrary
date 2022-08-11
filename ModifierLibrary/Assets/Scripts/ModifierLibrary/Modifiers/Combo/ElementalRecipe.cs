using UnitLibrary;

namespace ModifierLibrary
{
	public class ElementalRecipe
	{
		public readonly ElementType ElementType;
		public readonly double Intensity;

		public ElementalRecipe(ElementType elementType, double intensity)
		{
			ElementType = elementType;
			Intensity = intensity;
		}
	}
}