using BaseProject;
using JetBrains.Annotations;

namespace ModifierSystem
{
    public class ComboRecipe
    {
        [CanBeNull]
        public string[] Id;
        [CanBeNull]
        public ElementalRecipe[] ElementalRecipe;
        [CanBeNull]
        public Stat[] Stat;

        public ComboRecipe()
        {
        }
        public ComboRecipe(string[] id)
        {
            Id = id;
        }
        public ComboRecipe(ElementalRecipe[] elementalRecipe)
        {
            ElementalRecipe = elementalRecipe;
        }
        public ComboRecipe(Stat[] stat)
        {
            Stat = stat;
        }
        public ComboRecipe(string[] id, ElementalRecipe[] elementalRecipe)
        {
            Id = id;
            ElementalRecipe = elementalRecipe;
        }
    }
}