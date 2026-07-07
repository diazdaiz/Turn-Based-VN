using System.Collections.Generic;
using UnityEngine;
using static CardVisual;

public partial class InfiniteBlades : Card {
    [SerializeField] Card shivCard;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.InfiniteBlades(target, 1, shivCard))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        string innate = IsUpgraded ? BrownText("Innate") + ".\n" : "";
        return $"{innate}At the start of your turn, add a {BrownText("Shiv")} to your hand.";
    }
}

