using System.Collections.Generic;
using static CardVisual;

public partial class CorpseExplosion : Card {
    int initialPoisonStack = 6;
    int upgradedPoisonStack = 9;
    int PoisonStack => IsUpgraded ? upgradedPoisonStack : initialPoisonStack;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.Poison(target, Combat.CalculateBlockGain(target, PoisonStack))),
            new CombatAction.ApplyStatus(target, new Status.CorpseExplosion(target, 1))
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        return $"Apply {NumberText(initialPoisonStack, PoisonStack)} {BrownText("Poison")}.\nWhen the enemy dies, deal damage equal to its max HP to ALL enemies.";
    }
}