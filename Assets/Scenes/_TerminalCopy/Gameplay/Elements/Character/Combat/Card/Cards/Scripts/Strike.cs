using System.Collections.Generic;
using static CardVisual;

public partial class Strike : Card {
    int initialDamageValue = 6;
    int upgradedDamageValue = 9;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damageValue)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage.";
    }
}
