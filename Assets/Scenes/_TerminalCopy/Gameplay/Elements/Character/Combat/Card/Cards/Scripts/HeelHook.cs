using System.Collections.Generic;
using static CardVisual;

public partial class HeelHook : Card {
    int initialDamageValue = 5;
    int upgradedDamageValue = 8;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        List<CombatAction> combatActions = new List<CombatAction>();

        combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
        if (target.Statuses.ContainsKey(typeof(Status.Weak))) {
            combatActions.Add(new CombatAction.AddEnergy(1));
            combatActions.Add(new CombatAction.Draws(1));
        }

        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage. If the enemy is {BrownText("Weak")}, Gain 1 Energy and draw 1 card.";
    }
}

