using System.Collections.Generic;
using static CardVisual;

public partial class Nightmare : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() { new CombatAction.CardsSelection(Combat.HandPile, 1, true, (cards) => {
            Combat.DoAfter(new CombatAction.ApplyStatus(target, new Status.Nightmare(target, cards)), typeof(CombatAction.CardsSelection));
            Combat.DoAfter(new CombatAction.MoveCards(cards, Combat.HandPile), typeof(CombatAction.ApplyStatus));
        }) };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        return $"Choose a card, next turn, add 3 copies of that card into your hand.\n{BrownText("Exhaust")}.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 2 : 3;
    }
}

