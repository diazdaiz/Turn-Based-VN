using System.Collections.Generic;
using static CardVisual;

public partial class Accuracy : Card {
    int initialAccuracyStack = 4;
    int upgradedAccuracyStack = 6;
    int AccuracyStack => IsUpgraded ? upgradedAccuracyStack : initialAccuracyStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.Accuracy(target, AccuracyStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"{BrownText("Shivs")} deal {NumberText(initialAccuracyStack, AccuracyStack)} additional damage.";
    }
}
