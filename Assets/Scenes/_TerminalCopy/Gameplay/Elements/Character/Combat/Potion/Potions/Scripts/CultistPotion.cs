using System.Collections.Generic;

public partial class CultistPotion : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Ritual(target, 1))
        };
    }
}
