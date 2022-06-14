using BaseProject;

namespace ModifierSystem
{
    public class ElementalRecipe
    {
        public ElementType elementType;
        public double Intensity;

        public ElementalRecipe(ElementType elementType, double intensity)
        {
            this.elementType = elementType;
            Intensity = intensity;
        }
    }
}