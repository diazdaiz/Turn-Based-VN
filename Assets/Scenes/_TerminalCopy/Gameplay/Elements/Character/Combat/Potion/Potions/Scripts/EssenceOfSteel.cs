using System.Collections.Generic;

public partial class EssenceOfSteel : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Plating(target, 4))
        };
    }
}
