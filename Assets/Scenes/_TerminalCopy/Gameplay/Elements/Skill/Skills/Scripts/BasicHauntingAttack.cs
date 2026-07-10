using System.Collections.Generic;

public class BasicHauntingAttack : Skill {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, 100)
            //new CombatAction.ApplyStatus(target, new Status.Weak(target,2))
        };
    }
}
