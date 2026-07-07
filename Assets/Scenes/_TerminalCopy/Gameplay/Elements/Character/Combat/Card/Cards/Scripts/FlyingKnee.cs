using System.Collections.Generic;
using static CardVisual;

public partial class FlyingKnee : Card {
    int initialDamageValue = 8;
    int upgradedDamageValue = 11;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damageValue),
            new CombatAction.ApplyStatus(caster, new Status.GainEnergiesNextTurn(caster, 1))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage.\nNext turn, gain 1 Energy.";
    }
}
