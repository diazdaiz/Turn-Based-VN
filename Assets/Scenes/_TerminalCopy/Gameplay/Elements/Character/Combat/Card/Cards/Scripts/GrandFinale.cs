using System.Collections.Generic;
using static CardVisual;

public partial class GrandFinale : Card {
    int initialDamageValue = 50;
    int upgradedDamageValue = 60;
    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, DamageValue)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = DamageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Can only be played if there are no cards in your draw pile. Deal {NumberText(initialDamageValue, damage)} damage to ALL enemies.";
    }

    protected override bool GetCanBePlayedFromHand() {
        return Combat.DrawPile.Count == 0;
    }
}

