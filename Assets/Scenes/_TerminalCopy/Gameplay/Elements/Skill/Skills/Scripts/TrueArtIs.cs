using System.Collections.Generic;

public partial class TrueArtIs : Skill {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.Artist(caster,1))
        };
    }
}