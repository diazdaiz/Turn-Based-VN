using System.Collections.Generic;
using UnityEngine;

public partial class BasicBurnAttack : Skill {
    [SerializeField] int damagePercentage = 100;
    [SerializeField] int burnStack = 50;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damagePercentage),
            new CombatAction.GainSkillPoint(1)
        };
    }
}
