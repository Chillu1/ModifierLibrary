TODO RN:  
* Sort ideas by prio
* Fix diagram needing init & apply. Instead it needs init & apply or Stack
* More Applier shenanigans, not permanent modifiers, etc
* More StackComponent Modifiers & Unit Tests
  * StackAdd Cooldown, and/or StackEffect Cooldown
  * Stack Modifiers
    * >Apply another modifier on 5 stacks. Small problem? Its an applier modifier, so it will be used as one for enemies as well
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
* Condition Apply (ex. (apply?) happens when player has more than 10 damage?)
* >Ressurection EffectComponent?
* Editor Window for prototypes/modifiers
