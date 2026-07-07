using System.Collections.Generic;
using static CardVisual;

public partial class PiercingWail : Card {
    int initialStrengthStack = -6;
    int upgradedStrengthStack = -8;
    int StrengthStack => IsUpgraded ? upgradedStrengthStack : initialStrengthStack;


    public override List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
        List<CombatAction> combatActions = new List<CombatAction>();
        for (int i = 0; i < targets.Count; i++) {
            combatActions.Add(new CombatAction.ApplyStatus(targets[i], new Status.Strength(targets[i], StrengthStack)));
            combatActions.Add(new CombatAction.ApplyStatus(targets[i], new Status.Shackled(targets[i], -StrengthStack)));
        }
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        int block = StrengthStack;
        if (caster != null) {
            block = Combat.CalculateBlockGain(caster, block);
        }
        return $"ALL enemies lose {NumberText(-initialStrengthStack, -StrengthStack)} Strength for 1 turn.\n{BrownText("Exhaust")}.";
    }
}

