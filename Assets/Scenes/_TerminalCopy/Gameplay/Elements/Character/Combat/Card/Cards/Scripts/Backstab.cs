using System.Collections.Generic;
using static CardVisual;

public partial class Backstab : Card {
    int initialDamageValue = 11;
    int upgradedDamageValue = 15;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damageValue)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"{BrownText("Innate")}.\nDeal {NumberText(initialDamageValue, damage)} damage.\n{BrownText("Exhaust")}.";
    }
}
