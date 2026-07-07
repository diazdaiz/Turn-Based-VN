using System.Collections.Generic;

public partial class AncientPotion : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Artifact(target, 1))
        };
    }
}
