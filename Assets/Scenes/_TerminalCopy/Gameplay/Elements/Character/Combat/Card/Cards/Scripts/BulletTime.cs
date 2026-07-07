using System.Collections.Generic;

public partial class BulletTime : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(caster, new Status.BulletTime(target, Combat.HandPile))
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"You cannot draw additional cards this turn. Reduce the cost of all cards in your hand to 0 this turn.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 2 : 3;
    }
}
