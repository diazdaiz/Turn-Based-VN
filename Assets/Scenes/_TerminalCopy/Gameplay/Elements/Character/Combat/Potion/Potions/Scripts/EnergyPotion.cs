using System.Collections.Generic;

public partial class EnergyPotion : Potion {
    public override List<CombatAction> Activate(CharacterCombat target) {
        return new() {
            new CombatAction.AddEnergy(2)
        };
    }
}
