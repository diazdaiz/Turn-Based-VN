using System.Collections.Generic;

public partial class AttackPotion : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.CreateCard(Card.GetRandom(Card.CardIdentity.Silent, true, false, false), Combat.HandPile)
        };
    }
}
