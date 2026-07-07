using System.Collections.Generic;
using static CardVisual;

public partial class Shiv : Card {
    int initialDamageValue = 4;
    int upgradedDamageValue = 6;
    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        int damage = DamageValue;
        if (caster.Statuses.ContainsKey(typeof(Status.Accuracy))) {
            damage += caster.Statuses[typeof(Status.Accuracy)].Stack;
        }
        return new() {
            new CombatAction.Attack(caster, target, damage)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = DamageValue;
        if (caster != null) {
            if (caster.Statuses.ContainsKey(typeof(Status.Accuracy))) {
                damage += caster.Statuses[typeof(Status.Accuracy)].Stack;
            }
        }
        if (target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage.\n{BrownText("Exhaust")}.";
    }
}
