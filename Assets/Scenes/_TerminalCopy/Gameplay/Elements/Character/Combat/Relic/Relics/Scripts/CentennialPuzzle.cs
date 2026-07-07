public partial class CentennialPuzzle : Relic {
    public override void Subscribe() {
        base.Subscribe();
        CombatAction.AfterTriggers[typeof(CombatAction.Damage)] += Activate;
    }

    public override void Activate(CombatAction combatAction) {
        base.Activate(combatAction);
        if (combatAction is CombatAction.Damage damage && damage.Receiver == Combat.Hero) {
            Combat.DoAfter(new CombatAction.Draws(3), combatAction);
            CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] -= Activate;
        }
    }
}
