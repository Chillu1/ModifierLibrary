using JetBrains.Annotations;

namespace ComboSystem
{
    public class ModifierParams//TODO RenameMe
    {
        public Modifier modifier;
        public AddModifierParameters addModifierProperties = AddModifierParameters.Default;
        public ActivationCondition activateCondition = default;
    }
    public sealed class ModifierHolder
    {
        [NotNull]
        public ModifierParams[] modifiers;

        /// <summary>
        ///     Single modifier constructor
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="properties"></param>
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