using System.Collections.Generic;
using static CardVisual;

public class Unload : Card {
    int initialDamageValue = 14;
    int upgradedDamageValue = 18;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        List<Card> nonAttackCard = new();
        for (int i = 0; i < Combat.HandPile.Count; i++) {
            if (Combat.HandPile[i].Type != CardType.Attack) {
                nonAttackCard.Add(Combat.HandPile[i]);
            }
        }
        return new() {
            new CombatAction.Attack(caster, target, damageValue),
            new CombatAction.Discards(nonAttackCard)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage. Discard all non-Attack cards in your hand.";
    }
}

