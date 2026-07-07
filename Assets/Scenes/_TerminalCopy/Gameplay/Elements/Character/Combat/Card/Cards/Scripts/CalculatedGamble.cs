using System.Collections.Generic;
using static CardVisual;

public partial class CalculatedGamble : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        List<Card> cards = new List<Card>(Combat.HandPile);
        cards.Remove(this);

        return new() {
            new CombatAction.Discards(cards),
            new CombatAction.Draws(cards.Count)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        string exhaust = IsUpgraded ? "" : ("\n" + BrownText("Exhaust") + ".");
        return $"Discard your hand, then draw that many cards.{exhaust}";
    }

    protected override bool GetExhaust() {
        return !IsUpgraded;
    }
}
