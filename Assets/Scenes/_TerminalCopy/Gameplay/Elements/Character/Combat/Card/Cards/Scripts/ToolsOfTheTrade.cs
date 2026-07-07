using System.Collections.Generic;

public partial class ToolsOfTheTrade : Card {
    int ToolsOfTheTradeStack = 1;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.ToolsOfTheTrade(caster, ToolsOfTheTradeStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"At the start of your turn, draw 1 card and discard 1 card.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 0 : 1;
    }
}

