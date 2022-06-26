# Notes

## Temp Notes

Temp DamageResistance
    Add Res
    Wait 1s
    Remove Res
    Remove Mod
Temporary damage buff

EffectComp needs to be encapsulated in CheckComp
ApplierEffectComp can be too, but we can also check in Check in modifier instead

CheckComponent should not have effect when?:
OnApplyModifiers?

ComboModifier:
    Min ComboReqs
    Dynamic Damage/Effect, based on Min amount of ComboReq

CastComponent: Holds cooldown, mana. Gets fed: target, cd reduction, mana reduction.
CostCOmponent: Mana, health, w/e.
CastingController: ?
Modifier: Updates CastComponent
ModifierController: Updates all modifiers

Casting
Modifier should hold the Cooldown, Mana usage data.
We should feed the CD/mana reduction on TryApply.

Automatic casting of spells, mana/cooldown?
CastComponent? Prob not. Modifiers shouldn't internally know anything about themself being cast?
Just have data, and cooldown in modifier, and handle casting in being/modifier controller?

Types of appliers:
Attack Only, Cast only, Cast & Attack? 

Stack cooldown? Either on effect or on stack, but it's prob not needed & semi-complex to make proper