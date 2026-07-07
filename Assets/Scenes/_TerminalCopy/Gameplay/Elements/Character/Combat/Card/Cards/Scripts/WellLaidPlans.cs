using System.Collections.Generic;
using static CardVisual;

public class WellLaidPlans : Card {
    int initialWellLaidPlansStack = 1;
    int upgradedWellLaidPlansStack = 2;
    int WellLaidPlansStack => IsUpgraded ? upgradedWellLaidPlansStack : initialWellLaidPlansStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.WellLaidPlans(target, WellLaidPlansStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        string s = IsUpgraded ? "s" : "";
        return $"At the end of your turn, {BrownText("Retain")} up to {NumberText(initialWellLaidPlansStack, WellLaidPlansStack)} card{s}.";
    }
}


