using System.Collections.Generic;

public class ShieldBash : Skill {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, 220),
            new CombatAction.ApplyStatus(target, new Status.Weak(target,1))
        };
    }
}
