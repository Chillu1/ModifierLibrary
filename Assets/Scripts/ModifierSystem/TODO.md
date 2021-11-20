TODO RN:    
> Linger makes it so "fast spells" dont do their effect, because modifier is already present  
>    Solution: make a class for status effects in Being, instead of having a linger for combos.
>              But then we can't combo based of present modifier IDs
>    Solution 2: //If we didnt stack or refresh, then apply internal modifier effect again? Any issues? We could limit this with a flag/component
                if(!stacked && !refreshed)
                    internalModifier.Init();//Problem comes here, since the effect might not actually be in Init()
 
_Refactor "unitType"_  
    Self, Ally, Enemy.  Dynamically added based on what unit type you are?
_SelfHeal modifier_
    Cast specific applier (by name), on ally/enemy
Basic Unit tests of components 
ConditionalApply (OnAttack, OnDeath, etc)

Heal -> Lifesteal (heal based on damage dealt)    
Unit test of all mechanics & components  
  
TODO:  
ApplierModifier  
Figure out order of operations:  
Attack  
GetApplierMods  
SetTarget of ApplierMods  
Apply (effect) applier Mods  
Set target before applying (& Effect())  
Later:  
ConditionalApply, OnAttack, OnCast, OnDeath  
SimpleAttackBuff  
AddTarget (when?) (init & apply need it)  
Remove after linger time from modController  
SimpleStatBuff  
StackedDoTDurationModifier  
Unity UI for controlling values, timers, etc
  
Basic example inheritance:  
DamageAttackModifier:  
InitUseModifier (Apply on init)  
-> SingleUseModifier (Gets removed after linger time)  
-> DamageAttackModifier (Actual effect) Target.DealDamage(damage)  
  
  
Component based system (mixing components to a new modifier, like a recipe):  
Components needed:  
https://www.reddit.com/r/gamedev/comments/1bm5xs/programmers_how_dowould_you_implement_a/c9848mc/  
Effect  
Target (makes sure Target is valid)  
Component types:  
ApplyComponent  
Simple apply, no rules, just call effect  
Conditional apply, when effect is triggered & a conditional value is true  
DurationComponent  
Remove after duration  
Effect after duration?  
StackComponent  
RefreshComponent?  
RemoveComponent  
TimeComponent  
  
        Component recipe:  
            DoT  
  
  
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
    TimeComponent -> RemoveComponent  
  
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
  
Modifier ideas:  
X can be: damage, speed, stat, health, mana, etc  
  
Different types of lifesteal:  
Based on damage dealt (done)  
Based on damage dealt before resistance reduction (done)  
Based on enemy's current/max health %  
  
  
When lower than X health do Y  
Heal damage instead of getting damaged for 3 seconds  
Stack More damage every instance  
More damage every missing X (mana, health, lower level than hero)  
Take control over enemy creep for X seconds  
When attacked do X  
When attacked by X damage do Y, then reset damage count  
Cant attack, cant be attacked. (Nightmare)  
Reduced regen (healing, mana, etc)  
More float X or X% with missing health/mana/etc (huskar)  
Thorns  
On kill enemies "Explode" dealing damage (can be comboed with, explode, dealing hp as dmg?) Would be a cool combo modifier  
The more damage you deal to yourself, the stronger your attacks are  
Every X attack do Y  
Every X cast do Y  
For every nearby enemy, do more X  
More X for every missing hp/mana on enemies/allies  
Backstab  
Bristleback  
Remove 30% of enemies health, but do the same to your hero  
IO link (healing, mana)  
Asborb X damage  
Counterattack % on attacked  
Lifesteal  
Lifesteal %current health  
Reduce forwards physical attacks (mars)  
More X at day/night  
For every kill gain X permanent stat  
Fast attacks, 70% lower damage  
Physical damage block  
Lower enemies physical damage %  
Reduce enemy stat(s) for X seconds  
Linked beings, if one dies, so does the other. Buff each other  
Glaive bounces  
Mana shield  
Health is mana, mana is health  
Invis?  
Morph into enemy, gain 1-2 of his spells (might be bad)  
Mana burn  
Reflect one damage instance  
Disarm  
Crit  
On attack X chance to spawn illu  
Link (razor)  
Souls (SF)  
Steal attributes/stats for X duration on X  
Spectre E  
Refractions (TA)  
Every hit deal more damage (Ursa)  
Lower regen health/mana/etc by 70-90% with X duration  
More damage based on distance  
Binds two targets, every target spell gets applied to both.  
Warlock binds Q  
  
  
Combo mod ideas:  
Remove all base resistances & armor  
Stop "time" for X seconds  
When close to dying, shadow grave, duration based on skill cooldown.