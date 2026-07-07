using System.Collections.Generic;
using static CardVisual;

public partial class QuickSlash : Card {
    int initialDamageValue = 8;
    int upgradedDamageValue = 12;
    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int DrawAmount => 1;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, DamageValue),
            new CombatAction.Draws(DrawAmount)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = DamageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage.\nDraw 1 Card.";
    }
}

