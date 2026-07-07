using System.Collections.Generic;
using static CardVisual;

public partial class Alchemize : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.CreatePotion()
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Obtain a random potion.\n{BrownText("Exhaust")}.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 0 : 1;
    }
}
