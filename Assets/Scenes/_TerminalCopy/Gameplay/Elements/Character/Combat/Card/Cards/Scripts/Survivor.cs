using System.Collections.Generic;
using static CardVisual;

public partial class Survivor : Card {
    int initialBlockValue = 8;
    int upgradedBlockValue = 11;
    int blockValue => IsUpgraded ? upgradedBlockValue : initialBlockValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.CardsSelection(Combat.HandPile, 1, true, (cards) => { Combat.DoAfter(new CombatAction.Discards(cards), typeof(CombatAction.CardsSelection)); }),
            new CombatAction.ApplyStatus(target, new Status.Block(target, Combat.CalculateBlockGain(target, blockValue)))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int block = blockValue;
        if (caster != null) {
            block = Combat.CalculateBlockGain(caster, block);
        }
        return $"Gain {NumberText(initialBlockValue, block)} {BrownText("Block")}.\nDiscard 1 card.";
    }
}
