using System.Collections.Generic;
using static CardVisual;

public class WraithForm : Card {
    int initialIntangibleStack = 2;
    int upgradedIntangibleStack = 3;
    int IntangibleStack => IsUpgraded ? upgradedIntangibleStack : initialIntangibleStack;
    int WraithFormStack => 1;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.WraithForm(target, WraithFormStack)),
            new CombatAction.ApplyStatus(caster, new Status.Intangible(target, IntangibleStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Gain {NumberText(initialIntangibleStack, IntangibleStack)} Intangible. At the end of your turn, lose 1 Dexterity.";
    }
}