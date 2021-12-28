TODO RN:  
* Sort ideas by prio
* Conditional modifiers
  * IBaseBeing instead of BaseBeing in events? 
  * TargetComponent should take care of who's getting what, not the effect components?
  * OnKill Modifier (ex. increase physical damage by X on kill, increase physical damage by X for 5 seconds on kill)
  * Validate effect component
  * EffectCleanup (apply)
  * OnHit
* Mutlitarget? Modifier, for AoE?
* TimeComp Slow (veno gale)
* Stats StatusTag (stats change)
* Unittest:
  * Value & intensity updates with time (10 linger = 1 second, 100 linger = 5 sec, 1000 linger = 20)±
  * Stat change removal after duration/remove effect
  * All mechanics & components
* Unity UI for controlling beings and their values, modifiers, timers, etc (check how entitas does it, can't because it's closed source...)
* UniqueId per new component?
* Condition Apply (ex. happens when player has more than 10 damage?)
* Editor Window for prototypes/modifiers

Automatic tags:
    XEffectComponent inside TimeComponent with resetOnFinished = effect
    //DamageComponent inside TimeComponent with resetOnFinished = DoT
    Heal
    Stun/Slow-effect,etc. On Init = StatusType.X
    Unit test: Correct tags

Later:  
    ConditionalApply, OnAttack, OnCast, OnDeath  
    SimpleAttackBuff  
    AddTarget (when?) (init & apply need it)  
    Remove after linger time from modController  
    SimpleStatBuff  
    StackedDoTDurationModifier