using System.Collections.Generic;

public partial class PowerPotion : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.CreateCard(Card.GetRandom(Card.CardIdentity.Silent, false, false, true), Combat.HandPile)
        };
    }
}
