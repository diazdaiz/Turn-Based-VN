using System.Collections.Generic;

public partial class PhantasmalKiller : Card {
    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.ApplyStatus(target, new Status.PhantasmalKiller(target, Combat.CalculateBlockGain(target, 1)))
        };
    }

    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
        return $"On your next turn, your Attacks deal double damage.";
    }

    protected override int GetEnergyForActivation() {
        return IsUpgraded ? 0 : 1;
    }
}

