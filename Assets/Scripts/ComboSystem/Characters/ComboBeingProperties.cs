using JetBrains.Annotations;

namespace ComboSystem
{
    public class ComboBeingProperties : BaseProject.BeingProperties
    {
        [CanBeNull]
        public ModifierHolder ModifierHolder;
    }
}