using System.Collections.Generic;
using static CardVisual;

public partial class CripplingCloud : Card {
    int initialPoisonStack = 4;
    int upgradedPoisonStack = 7;
    int PoisonStack => IsUpgraded ? upgradedPoisonStack : initialPoisonStack;
    int WeakStack => 2;

    public override List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
        List<CombatAction> combatActions = new List<CombatAction>();
        for (int i = 0; i < targets.Count; i++) {
            combatActions.Add(new CombatAction.ApplyStatus(targets[i], new Status.Poison(targets[i], PoisonStack)));
            combatActions.Add(new CombatAction.ApplyStatus(targets[i], new Status.Weak(targets[i], WeakStack)));
        }
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        return $"Apply {NumberText(initialPoisonStack, PoisonStack)} {BrownText("Poison")} and 2 {BrownText("Weak")} to ALL enemies.\n{BrownText("Exhaust")}.";
    }
}

