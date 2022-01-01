TODO RN:  
* Sort ideas by prio
* Being/BaseBeing protected?
* Proper LegalTarget(ing)
* More Applier shenanigans, not permanent modifiers, etc
  * Condition Applier (IConditionEffectComponent)
* More StackComponent Modifiers & Unit Tests
  * StackAdd Cooldown, and/or StackEffect Cooldown
  * Stack Modifiers
* Conditional modifiers
  * IBaseBeing instead of BaseBeing in events? 
* Make lifeSteal component part of actual baseProject.Being class instead
* Mutlitarget? Modifier, for AoE?
* TimeComp Slow (veno gale)
* Stats StatusTag (stats change)
* Improve modifier decision flow chart
* Unittest:
  * Value & intensity updates with time (10 linger = 1 second, 100 linger = 5 sec, 1000 linger = 20)±
  * Stat change removal after duration/remove effect
  * All mechanics & components
* Unity UI for controlling beings and their values, modifiers, timers, etc (check how entitas does it, can't because it's closed source...)
* UniqueId per new component?
* >Condition Apply (ex. (apply?) happens when player has less/more than 10 X(damage, health, etc)?)
    * On low health
* >Ressurection EffectComponent? Needs to be condition based though
* Editor Window for prototypes/modifiers
