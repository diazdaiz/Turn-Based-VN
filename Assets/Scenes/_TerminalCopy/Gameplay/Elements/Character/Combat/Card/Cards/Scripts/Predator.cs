using System.Collections.Generic;
using static CardVisual;

public partial class Predator : Card {
    int initialDamageValue = 15;
    int upgradedDamageValue = 20;
    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int DrawAmount = 2;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, DamageValue),
            new CombatAction.ApplyStatus(caster, new Status.Predator(caster, DrawAmount))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = DamageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage. Draw 2 more cards next turn.";
    }
}

