using System.Collections.Generic;
using static CardVisual;

public partial class Doppelganger : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        if (Combat.Energy + (IsUpgraded ? 1 : 0) <= 0) {
            return null;
        }
        return new() {
            new CombatAction.ApplyStatus(target, new Status.DrawCardsNextTurn(target, Combat.Energy + (IsUpgraded ? 1 : 0))),
            new CombatAction.AddEnergy(-Combat.Energy)
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        string plusOne = IsUpgraded ? $"+{NumberText(0, 1)}" : "";
        return $"Next turn, draw X{plusOne} cards and gain X{plusOne} Energy.\n{BrownText("Exhaust")}.";
    }
}

