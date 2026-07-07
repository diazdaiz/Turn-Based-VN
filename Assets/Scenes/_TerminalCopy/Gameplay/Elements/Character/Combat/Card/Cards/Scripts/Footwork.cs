using System.Collections.Generic;
using static CardVisual;

public partial class Footwork : Card {
    int initialDexterityStack = 2;
    int upgradedDexterityStack = 3;
    int DexterityStack => IsUpgraded ? upgradedDexterityStack : initialDexterityStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.Dexterity(target, DexterityStack))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Gain {NumberText(initialDexterityStack, DexterityStack)} {BrownText("Dexterity")}.";
    }
}

