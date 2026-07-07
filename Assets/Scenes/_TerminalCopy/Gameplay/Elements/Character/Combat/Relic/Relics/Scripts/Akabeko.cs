public partial class Akabeko : Relic {
    public override void Subscribe() {
        base.Subscribe();
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] += Activate;
    }

    public override void Activate(CombatAction combatAction) {
        base.Activate(combatAction);
        Combat.DoOnLast(new CombatAction.ApplyStatus(Combat.Hero, new Status.Vigor(Combat.Hero, 8)), combatAction);
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] -= Activate;
    }
}
