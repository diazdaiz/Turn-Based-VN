using System.Collections.Generic;
using static CardVisual;

public partial class NoxiusFumes : Card {
    int initialNoxiusFumesStack = 2;
    int upgradedNoxiusFumesStack = 3;
    int NoxiusFumesStack => IsUpgraded ? upgradedNoxiusFumesStack : initialNoxiusFumesStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.NoxiusFumes(target, NoxiusFumesStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"At the start of your turn, apply {NumberText(initialNoxiusFumesStack, NoxiusFumesStack)} {BrownText("Poison")} to ALL enemies.";
    }
}

