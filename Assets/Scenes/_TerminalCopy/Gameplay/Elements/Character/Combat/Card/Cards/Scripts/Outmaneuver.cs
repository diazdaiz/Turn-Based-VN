using System.Collections.Generic;
using static CardVisual;

public partial class Outmaneuver : Card {
    int initialOutmaneuverStack = 2;
    int upgradedOutmaneuverStack = 3;
    int OutmaneuverStack => IsUpgraded ? upgradedOutmaneuverStack : initialOutmaneuverStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.GainEnergiesNextTurn(target, OutmaneuverStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Next turn, gain {NumberText(initialOutmaneuverStack, OutmaneuverStack)} Energy.";
    }
}

