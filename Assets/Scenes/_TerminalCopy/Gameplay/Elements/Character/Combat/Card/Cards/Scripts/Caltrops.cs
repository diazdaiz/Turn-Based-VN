using System.Collections.Generic;
using static CardVisual;

public partial class Caltrops : Card {
    int initialCaltropsStack = 3;
    int upgradedCaltropsStack = 5;
    int CaltropsStack => IsUpgraded ? upgradedCaltropsStack : initialCaltropsStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.Caltrops(target, CaltropsStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Whenever you are attacked, deal {NumberText(initialCaltropsStack, CaltropsStack)} damage back.";
    }
}
