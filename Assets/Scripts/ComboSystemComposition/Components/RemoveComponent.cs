using System;
using ComboSystem;

namespace ComboSystemComposition
{
    public class RemoveComponent : EffectComponent, IRemoveComponent
    {
        private ModifierController _modifierController;//TODO TEMP
        private readonly double _lingerTime;//TODO Linger
        private Modifier _modifier;

        public RemoveComponent(Modifier modifier, double lingerTime = 0.5d) : base(null)
        {
            _modifier = modifier;
            _lingerTime = lingerTime;
        }

        public void Remove()
        {
            //_modifierController.RemoveModifier(_modifier);
        }

        public void CleanUp()
        {

        }
    }
}