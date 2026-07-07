using System.Collections.Generic;
using static CardVisual;

public partial class DaggerSpray : Card {
    int initialDamageValue = 4;
    int upgradedDamageValue = 6;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
        List<CombatAction> combatActions = new List<CombatAction>();
        for (int i = 0; i < 2; i++) {
            combatActions.Add(new CombatAction.Attack(caster, new List<CharacterCombat>(Combat.Enemies), damageValue));
        }
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Deal {BrownText($"{damageValue}")} damage to ALL enemies twice.";
    }
}

