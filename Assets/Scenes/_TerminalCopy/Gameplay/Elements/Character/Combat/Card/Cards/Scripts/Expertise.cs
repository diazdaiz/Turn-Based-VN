using System.Collections.Generic;
using UnityEngine;
using static CardVisual;

public partial class Expertise : Card {
    int initialDrawTarget = 6;
    int upgradedDrawTarget = 7;
    int drawTarget => IsUpgraded ? upgradedDrawTarget : initialDrawTarget;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Draws(Mathf.Max(0, drawTarget - Combat.HandPile.Count + 1))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Draw cards until you have {NumberText(6, 7)} in your hand.";
    }
}

