using System.Collections.Generic;
using UnityEngine;

public partial class Explosiooooon : Skill {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        int stack = 0;
        if (caster.Statuses.ContainsKey(typeof(Status.Artist))) {
            stack = caster.Statuses[typeof(Status.Artist)].Stack;
        }
        return new() {
            new CombatAction.Attack(caster, new List<CharacterCombat>(Combat.EnemyTeam), 50 * (int)Mathf.Pow(2,stack))
        };
    }
}