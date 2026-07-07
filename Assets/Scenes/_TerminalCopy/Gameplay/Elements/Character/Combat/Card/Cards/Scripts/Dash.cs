using System.Collections.Generic;
using static CardVisual;

public partial class Dash : Card {
    int initialDamageValue = 10;
    int upgradedDamageValue = 13;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int initialBlockValue = 10;
    int upgradedBlockValue = 13;
    int blockValue => IsUpgraded ? upgradedBlockValue : initialBlockValue;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.Block(caster, Combat.CalculateBlockGain(caster, blockValue))),
            new CombatAction.Attack(caster, target, damageValue)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        int block = blockValue;
        if (caster != null) {
            block = Combat.CalculateBlockGain(caster, block);
        }
        return $"Gain {NumberText(initialDamageValue, damage)} Block. {NumberText(initialBlockValue, block)} damage.";
    }
}

