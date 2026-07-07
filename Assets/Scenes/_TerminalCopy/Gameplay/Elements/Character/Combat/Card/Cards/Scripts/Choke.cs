using System.Collections.Generic;
using static CardVisual;

public partial class Choke : Card {
    int initialChokeStack = 3;
    int upgradedChokeStack = 5;
    int ChokeStack => IsUpgraded ? upgradedChokeStack : initialChokeStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        //attack
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Choke(target, ChokeStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Deal 12 damage. Whenever you play a card this turn, the targeted enemy loses {NumberText(initialChokeStack, ChokeStack)} HP.";
    }
}
