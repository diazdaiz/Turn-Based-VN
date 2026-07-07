using System.Collections.Generic;
using static CardVisual;

public partial class Fleschettes : Card {
    int initialDamageValue = 4;
    int upgradedDamageValue = 6;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        List<CombatAction> combatActions = new List<CombatAction>();

        for (int i = 0; i < Combat.HandPile.Count; i++) {
            if (Combat.HandPile[i].Type == CardType.Skill) {
                combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
            }
        }
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage for each Skill in your hand.";
    }
}

