public partial class CultistMask : Relic {
    public override void Subscribe() {
        base.Subscribe();
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] += Activate;
    }

    public override void Activate(CombatAction combatAction) {
        base.Activate(combatAction);
        Combat.DoOnLast(new CombatAction.ApplyStatus(Combat.Hero, new Status.NoAttack(Combat.Hero, 1)), combatAction.GetType());
        Combat.DoOnLast(new CombatAction.ApplyStatus(Combat.Hero, new Status.Ritual(Combat.Hero, 1)), combatAction.GetType());
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] -= Activate;
    }
}