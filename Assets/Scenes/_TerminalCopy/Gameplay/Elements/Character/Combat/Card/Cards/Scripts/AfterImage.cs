using System.Collections.Generic;
using static CardVisual;

public partial class AfterImage : Card {
    int afterImageStack = 1;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.AfterImage(target, afterImageStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        string innate = IsUpgraded ? $"{BrownText("Innate")}.\n" : "";
        return $"{innate}Whenever you play a card, gain 1 {BrownText("Block")}.";
    }

    protected override bool GetInnate() {
        return IsUpgraded;
    }
}
