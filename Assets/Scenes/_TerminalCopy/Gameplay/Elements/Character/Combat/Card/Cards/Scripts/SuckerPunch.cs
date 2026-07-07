using System.Collections.Generic;
using static CardVisual;

public partial class SuckerPunch : Card {
    int initialDamageValue = 7;
    int upgradedDamageValue = 9;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int initialWeakTurn = 1;
    int upgradedWeakTurn = 2;
    int weakTurn => IsUpgraded ? upgradedWeakTurn : initialWeakTurn;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damageValue),
            new CombatAction.ApplyStatus(target, new Status.Weak(target, weakTurn))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage.\nApply {NumberText(initialWeakTurn, weakTurn)} {BrownText("Weak")}.";
    }
}
