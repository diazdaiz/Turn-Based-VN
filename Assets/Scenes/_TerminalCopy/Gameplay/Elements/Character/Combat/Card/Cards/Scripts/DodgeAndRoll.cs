using System.Collections.Generic;
using static CardVisual;

public partial class DodgeAndRoll : Card {
    int initialBlockValue = 4;
    int upgradedBlockValue = 6;
    int blockValue => IsUpgraded ? upgradedBlockValue : initialBlockValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Block(target, Combat.CalculateBlockGain(target, blockValue))),
            new CombatAction.ApplyStatus(target, new Status.GainBlocksNextTurn(target, Combat.CalculateBlockGain(target, blockValue)))
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        int block = blockValue;
        if (caster != null) {
            block = Combat.CalculateBlockGain(caster, block);
        }
        return $"Gain {NumberText(initialBlockValue, block)} {BrownText("Block")}. Next turn, gain {NumberText(initialBlockValue, block)} {BrownText("Block")}.";
    }
}

