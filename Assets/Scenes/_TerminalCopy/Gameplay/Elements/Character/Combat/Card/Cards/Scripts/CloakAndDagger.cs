using System.Collections.Generic;
using UnityEngine;
using static CardVisual;

public partial class CloakAndDagger : Card {
    int blockValue = 6;
    int initialShivsCount = 1;
    int upgradedShivsCount = 2;
    int ShivCount => IsUpgraded ? upgradedShivsCount : initialShivsCount;
    [SerializeField] Card shivCard;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new(){
            new CombatAction.ApplyStatus(target, new Status.Block(target, Combat.CalculateBlockGain(target, blockValue))),
            new CombatAction.CreateCards(shivCard, ShivCount, Combat.HandPile)
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        int block = blockValue;
        if (caster != null) {
            block = Combat.CalculateBlockGain(caster, block);
        }
        string s = IsUpgraded ? "s" : "";
        return $"Gain {NumberText(blockValue, block)} {BrownText("Block")}.\nAdd {NumberText(initialShivsCount, ShivCount)} {BrownText("Shiv" + s)} to your hand.";
    }
}
