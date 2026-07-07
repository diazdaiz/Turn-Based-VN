using System.Collections.Generic;

public partial class DexterityPotion : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Dexterity(target, 2))
        };
    }
}
