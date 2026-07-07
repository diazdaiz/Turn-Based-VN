using System.Collections.Generic;

public partial class BlessingOfTheForge : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.UpgradeCards(Combat.HandPile)
        };
    }
}
