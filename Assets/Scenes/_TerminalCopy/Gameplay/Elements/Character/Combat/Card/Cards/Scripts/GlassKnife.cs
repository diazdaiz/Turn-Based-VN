using System.Collections.Generic;
using static CardVisual;

public partial class GlassKnife : Card {
    int initialDamageValue = 8;
    int upgradedDamageValue = 12;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int usage = 0;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        List<CombatAction> combatActions = new List<CombatAction>();
        //wait, harusnya calculate damagenya sama kah? disini?
        for (int i = 0; i < 2; i++) {
            combatActions.Add(new CombatAction.Attack(caster, target, damageValue - usage * 2));
        }
        usage += 1;
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage twice. Decrease the damage of this card by 2 this combat.";
    }
}

