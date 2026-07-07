using System.Collections.Generic;
using static CardVisual;

public partial class Backflip : Card {
    int initialBlockValue = 5;
    int upgradedBlockValue = 8;
    int blockValue => IsUpgraded ? upgradedBlockValue : initialBlockValue;
    int drawAmount = 2;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Block(target, Combat.CalculateBlockGain(target, blockValue))),
            new CombatAction.Draws(drawAmount)
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        int block = blockValue;
        if (caster != null) {
            block = Combat.CalculateBlockGain(caster, block);
        }
        return $"Gain {NumberText(initialBlockValue, block)} {BrownText("Block")}.\nDraw 2 cards.";
    }
}