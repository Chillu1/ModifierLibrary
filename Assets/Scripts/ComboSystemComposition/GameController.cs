using System.Globalization;
using ComboSystem;
using JetBrains.Annotations;

namespace ComboSystemComposition
{
    /*
     Process (lifecycle):
        Modifier tries to be added to entity collection
            We check for duplicates, refresh, stack, etc.
        When added, apply may be called (ex. on init buff)
        Update all updatable components in collection (ex. TimeComponent)
        Apply components check for validity of apply
            if passed: Trigger EffectComponents
                Either triggers an effect, or removes modifier after duration passed

    Technical lifecycle:
        ModifierManager.AddModifier(modifier)
            ModifierManager.CheckDuplicate(modifier)
                Stack or Refresh
        If targetself, setTarget(self)
        modifier.Init()
            May modifier.Apply()
        Update TimeComponent
            May modifier.Apply()
        After linger/duration remove ModifierManage.RemoveModifier(modifier)

    InitComponent -> ApplyComponent -> EffectComponent
    TimeComponent -> ApplyComponent -> EffectComponent
    RemoveComponent

     Structure
            Dump components not knowing what they do (time(duration), single use, time(interval))
            EffectComponents
               Have an effect: stun, statchange, deal damage
            ApplyComponents
               Trigger effect: after duration, on interval, condition, in apply
            TimeComponents
               Trigger effect: after duration/ on interval
    */

    /*
     ModifierSet (Aspect of the cat)
        AddStatModifier (Speed buff)
        AddStatModifier (AttackSpeed buff)
        DurationModifier (RemoveComponent, removed ModifierSet)
     */

    //RefreshedDoTIntervalDurationModifier
    /*
      Workings:
        Added to collection/entity instantly
        After X Interval, does Y Damage
        After Z Duration, gets removed from collection/entity
        if  modifier added again to collection/entity
            refresh modifier _timer
    */

    public class GameController
    {
        public ModifierPrototypes ModifierPrototypes { get; private set; }

        public void Start()
        {
            ModifierPrototypes = new ModifierPrototypes();
        }
    }
}