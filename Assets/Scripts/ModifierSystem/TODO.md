TODO RN:  
* Sort ideas by prio
* >Move ModifierControl away from Being? Already one?
* Conditional modifiers
* ToDeleted Modifier line.40 ModifierController
* Mutlitarget? Modifier, for AoE?
* Modifier with all possible tags
* TimeComp Slow (veno gale)
* Unity UI for controlling values, timers, etc (entitas)
* Unittest:
  * Value & intensity updates with time (10 linger = 1 second, 100 linger = 5 sec, 1000 linger = 20)±
  * Stat change removal after duration/remove effect
  * All mechanics & components
* UniqueId per new component?
* ConditionalApply (OnAttack, OnDeath, etc) (ex. explode on death, deal damage revenge, on death)

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