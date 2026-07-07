using System.Collections.Generic;
using static CardVisual;

public partial class EscapePlan : Card {
    int initialBlockValue = 3;
    int upgradedBlockValue = 5;
    int blockValue => IsUpgraded ? upgradedBlockValue : initialBlockValue;
    int drawAmount = 1;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        //subscribe
        if (Combat.HandPile.Count < 10) {
            CombatAction.AfterTriggers[typeof(CombatAction.Draw)] += OnDrawCard;
        }
        return new() {
            new CombatAction.Draws(drawAmount)
        };
    }

    public void OnDrawCard(CombatAction combatAction) {
        if (Combat.HandPile[^1].Type == Card.CardType.Skill) {
            Combat.DoAfter(new CombatAction.ApplyStatus(Combat.Hero, new Status.Block(Combat.Hero, Combat.CalculateBlockGain(Combat.Hero, blockValue))), typeof(CombatAction.Draws));
        }
        CombatAction.AfterTriggers[typeof(CombatAction.Draw)] -= OnDrawCard;
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        int block = blockValue;
        if (caster != null) {
            block = Combat.CalculateBlockGain(caster, block);
        }
        return $"Draw 1 card. If you draw a Skill, gain {NumberText(initialBlockValue, block)} {BrownText("Block")}.";
    }
}


//Gain 5(8)  Block. Draw 2 cards.
