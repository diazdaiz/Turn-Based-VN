using System.Collections.Generic;
using static CardVisual;

public partial class Malaise : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Strength(target, -Combat.Energy - (IsUpgraded ? 1 : 0))),
            new CombatAction.ApplyStatus(target, new Status.Weak(target, Combat.Energy + (IsUpgraded ? 1 : 0))),
            new CombatAction.AddEnergy(-Combat.Energy)
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        string plusOne = IsUpgraded ? $"+{NumberText(0, 1)}" : "";
        return $"Enemy loses X{plusOne} {BrownText("Strength")}.\nApply X{plusOne} {BrownText("Weak")}.\n{BrownText("Exhaust")}.";
    }
}

