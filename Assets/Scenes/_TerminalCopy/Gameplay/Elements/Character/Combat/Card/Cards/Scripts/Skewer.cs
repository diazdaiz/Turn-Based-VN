using System.Collections.Generic;
using static CardVisual;

public partial class Skewer : Card {
    int initialDamageValue = 7;
    int upgradedDamageValue = 10;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        int energy = Combat.Energy;
        Combat.Energy = 0;
        List<CombatAction> combatActions = new List<CombatAction>();
        for (int i = 0; i < energy; i++) {
            combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
        }
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage X times.";
    }
}

