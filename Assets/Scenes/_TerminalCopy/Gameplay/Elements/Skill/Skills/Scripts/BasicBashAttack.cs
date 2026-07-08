using System.Collections.Generic;
using UnityEngine;

public partial class BasicBashAttack : Skill {
    [SerializeField] int damagePercentage = 120;

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damagePercentage),
        };
    }
}
