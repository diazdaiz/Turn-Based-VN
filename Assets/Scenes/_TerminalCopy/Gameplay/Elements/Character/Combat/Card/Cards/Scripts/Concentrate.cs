using System.Collections.Generic;
using static CardVisual;

public partial class Concentrate : Card {
    int energyGain = 2;
    int initialDiscardAmount = 3;
    int upgradedDiscardAmount = 2;
    int DiscardAmount => IsUpgraded ? upgradedDiscardAmount : initialDiscardAmount;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.CardsSelection(Combat.HandPile, DiscardAmount, true, (cards) => { Combat.DoAfter(new CombatAction.Discards(cards), typeof(CombatAction.CardsSelection)); }),
            new CombatAction.AddEnergy(energyGain)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Discard {NumberText(initialDiscardAmount, DiscardAmount, true)} cards. Gain 2 Energy.";
    }
}

