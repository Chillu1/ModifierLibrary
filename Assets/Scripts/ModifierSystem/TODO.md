TODO RN:  
* Sort ideas by prio
* Conditional modifiers
  * IBaseBeing instead of BaseBeing in events? 
  * >>Some unit tests for fringe cases condition event
     * >>OnHeal, heal yourself flat
     * >>OnHeal, heal yourself 20%
* >Make lifeSteal component part of actual baseProject.Being class instead 
* >Have a go at stackComponent again
* Mutlitarget? Modifier, for AoE?
* TimeComp Slow (veno gale)
* Stats StatusTag (stats change)
* Unittest:
  * Value & intensity updates with time (10 linger = 1 second, 100 linger = 5 sec, 1000 linger = 20)±
  * Stat change removal after duration/remove effect
  * All mechanics & components
* Unity UI for controlling beings and their values, modifiers, timers, etc (check how entitas does it, can't because it's closed source...)
* UniqueId per new component?
* Condition Apply (ex. (apply?) happens when player has more than 10 damage?)
* Ressurection
* Editor Window for prototypes/modifiers

Automatic tags:
    XEffectComponent inside TimeComponent with resetOnFinished = effect
    //DamageComponent inside TimeComponent with resetOnFinished = DoT
    Heal
    Stun/Slow-effect,etc. On Init = StatusType.X
    Unit test: Correct tags

Later:    
    AddTarget (when?) (init & apply need it)  
    StackedDoTDurationModifier