using System;
using BaseProject;

namespace ModifierSystem
{
    /// <summary>
    ///     ComboModifier does everything that normal modifier can, but is activated on specific conditions
    /// </summary>
    public class ComboModifier : IModifier, IEntity<string>, ICloneable, IEventCopy<ComboModifier>
    {
        public string Id => Modifier.Id;

        private Modifier Modifier { get; }
        private object Conditions { get; }
        private double Cooldown { get; }


        public ComboModifier(Modifier modifier, object conditions, double cooldown)
        {
            Conditions = conditions;
            Cooldown = cooldown;
            Modifier = modifier;
        }

        public bool ValidatePrototypeSetup()
        {
            bool success = Modifier.ValidatePrototypeSetup();

            if (Id.Contains("Applier") || Modifier.ApplierModifier)
            {
                Log.Error("ComboModifier can't be an applier modifier, right?", "modifiers");
                success = false;
            }

            return success;
        }

        public void CopyEvents(ComboModifier prototype)
        {
            Modifier.CopyEvents(prototype.Modifier);
        }

        object IModifier.Clone()
        {
            return MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return MemberwiseClone();
        }
    }
}