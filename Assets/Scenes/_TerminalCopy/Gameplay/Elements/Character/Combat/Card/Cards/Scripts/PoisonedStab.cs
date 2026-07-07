using System.Collections.Generic;
using static CardVisual;

public partial class PoisonedStab : Card {
    int initialDamageValue = 6;
    int upgradedDamageValue = 8;
    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int initialPoisonStack = 3;
    int upgradedPoisonStack = 4;
    int PoisonStack => IsUpgraded ? upgradedPoisonStack : initialPoisonStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, DamageValue),
            new CombatAction.ApplyStatus(target, new Status.Poison(target, PoisonStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = DamageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage. Apply {NumberText(initialPoisonStack, PoisonStack)} {BrownText("Poison")}.";
    }
}

