using System.Collections.Generic;
using static CardVisual;

public partial class Envenom : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.Envenom(target, 1))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Whenever an attack deals unblocked damage, apply 1 {BrownText("Poison")}.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 1 : 2;
    }
}

