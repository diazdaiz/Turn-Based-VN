using System.Collections.Generic;
using static CardVisual;

public partial class Adrenaline : Card {
    int initialEnergyGain = 1;
    int upgradedEnergyGain = 2;
    int EnergyGain => IsUpgraded ? upgradedEnergyGain : initialEnergyGain;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.AddEnergy(EnergyGain),
            new CombatAction.Draws(2)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Gain {NumberText(initialEnergyGain, EnergyGain)} {BrownText("Energy")}.\nDraw 2 cards.\n{BrownText("Exhaust")}";
    }
}
