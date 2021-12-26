TODO RN:    
* Remove StatusEffect (update LegalActions)
* Mutlitarget? Modifier, for AoE?
* Modifier with all possible tags
* Sort ideas by prio
* Conditional modifiers
* Normal singular stun modifier
* TimeComp Stun every X
* TimeComp Slow (veno gale)
* Unity UI for controlling values, timers, etc (entitas)
* ComboModifier:
  * Stat based
  * Cooldown
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