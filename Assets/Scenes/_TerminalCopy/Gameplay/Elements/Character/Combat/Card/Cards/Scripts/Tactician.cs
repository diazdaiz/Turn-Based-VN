using System.Collections.Generic;
using static CardVisual;

public partial class Tactician : Card {
    int initialEnergyGain = 1;
    int upgradedEnergyGain = 2;
    int EnergyGain => IsUpgraded ? upgradedEnergyGain : initialEnergyGain;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.AddEnergy(EnergyGain)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"{BrownText("Unplayable")}.\nIf this card is discarded from your hand, gain {NumberText(initialEnergyGain, EnergyGain)} Energy.";
    }
}


