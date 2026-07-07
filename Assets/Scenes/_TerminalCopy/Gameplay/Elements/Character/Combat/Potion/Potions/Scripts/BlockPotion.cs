using System.Collections.Generic;

public partial class BlockPotion : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Block(target, 12))
        };
    }
}
