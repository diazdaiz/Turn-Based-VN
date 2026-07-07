using System.Collections.Generic;
using static CardVisual;

public partial class Terror : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Vulnerable(target, 99))
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        return $"Apply 99 {BrownText("Vulnerable")}.\n{BrownText("Exhaust")}.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 0 : 1;
    }
}

