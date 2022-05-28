# TODO:  
- [ ] **Cast Applier**
  - [ ] New target = reset cast timer
- [ ] Cost Component
  - [ ] Specify cost on attack or cast?
- [ ] StatusTags refactor
- [ ] Move ComboSystemOutside dir, changed notes, todo, design dir outside unity project
- [ ] UnitType => Ally/Enemy. TargetType? Ally/Enemy/Self
- [ ] IEventCopy not needed?
- [ ] Refactor ModifierPrototypesBase all modifiers setup
- [ ] DamageData refactor
- [ ] ElementalDamage refactor
- [ ] Modifiers that need to be casted. With cooldown/mana?

# #Lower prio

- [ ] Sort ideas by prio
- [ ] >Status effect to legal actions? (Replace?)
- [ ] >Try to make an actual playable "game" out of it. Ex. a hero fighting against spawning enemies
- [ ] Counter attack (onhit event? Or built-in)
- [ ] Stack effect HasFlag instead of switch
- [ ] ComboAppliers?
- [ ] >Ursa E (stackbased)
    Target is null on applier, but we're trying to setup a conditional
    We apply modifier through attacking & onHit
- [ ] >Recipe system for modifiers (Easier to create, without know the ins & outs)
    Add non-time remove (conditional)
    "Pointer" to what we should refresh, effect, or removeeffect
    SetRemovable ICleanUpComponent
- [ ] Shield absorption (Abaddon W) (Subscribing to takeDmg event, then getting data (dmg taken) from that event?)
- [ ] >Better naming for Condition stuff
- [ ] Proper LegalTarget(ing)
- [ ] More Applier shenanigans, not permanent modifiers, etc
  - [ ] Condition Applier (IConditionEffectComponent)
- [ ] More StackComponent Modifiers & Unit Tests
  - [ ] >Stack Modifiers
- [ ] Conditional modifiers
  - [ ] IBaseBeing instead of BaseBeing in events? 
- [ ] Make lifeSteal component part of actual baseProject.Being class instead
- [ ] Mutlitarget? Modifier, for AoE?
- [ ] TimeComp Slow (veno gale)
- [ ] Stats StatusTag (stats change)
- [ ] Improve & update modifier decision flow chart (generator)
- [ ] Unittest:
  - [ ] Value & intensity updates with time (10 linger = 1 second, 100 linger = 5 sec, 1000 linger = 20)±
  - [ ] Stat change removal after duration/remove effect
  - [ ] All mechanics & components
- [ ] Unity UI for controlling beings and their values, modifiers, timers, etc (check how entitas does it, can't because it's closed source...)
- [ ] UniqueId per new component?
- [ ] >Condition Apply (ex. (apply?) happens when player has less/more than 10 X(damage, health, etc)?)
  > ConditionCheck Has Stat  
  > Condition without ConditionEvents (only applies on X) how do?  
  >  OnDealDamage, check health stat for owner. If lower than X, apply effect  
  >>  If health on IsLow, add 50 damage, if not, remove that 50 damage  
     Conditional towards enemy(acter)  
     Value Based   
- [ ] >Ressurection EffectComponent? Needs to be condition based though
- [ ] Editor Window for prototypes/modifiers