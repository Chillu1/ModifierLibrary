using BaseProject;

namespace ModifierSystem
{
    public class ElementalRecipe
    {
        public ElementalType ElementalType;
        public double Intensity;

        public ElementalRecipe(ElementalType elementalType, double intensity)
        {
            ElementalType = elementalType;
            Intensity = intensity;
        }
    }
}