using System.Collections.Generic;
using static CardVisual;

public partial class DaggerThrow : Card {
    int initialDamageValue = 9;
    int upgradedDamageValue = 12;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damageValue),
            new CombatAction.Draws(1),
            new CombatAction.CardsSelection(Combat.HandPile, 1, true, (cards) => { Combat.DoAfter(new CombatAction.Discards(cards), typeof(CombatAction.CardsSelection)); })
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage.\nDraw 1 card.\nDiscard 1 card.";
    }
}

