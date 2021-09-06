using JetBrains.Annotations;

namespace ComboSystem
{
    public class ModifierParams//TODO RenameMe
    {
        public Modifier modifier;
        public AddModifierParameters addModifierProperties = AddModifierParameters.Default;
    }
    public sealed class ModifierHolder
    {
        [NotNull]
        public ModifierParams[] modifiers;

        public ModifierHolder(Modifier modifier, AddModifierParameters properties = AddModifierParameters.Default)
        {
            modifiers = new[] { new ModifierParams() { modifier = modifier, addModifierProperties = properties } };
        }

        public ModifierHolder(ModifierParams[] modifiers)
        {
            this.modifiers = modifiers;
        }
    }
}