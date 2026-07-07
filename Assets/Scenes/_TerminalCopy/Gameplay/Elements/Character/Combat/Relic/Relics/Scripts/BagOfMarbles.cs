public partial class BagOfMarbles : Relic {
    public override void Subscribe() {
        base.Subscribe();
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] += Activate;
    }

    public override void Activate(CombatAction combatAction) {
        base.Activate(combatAction);
        for (int i = 0; i < Combat.Enemies.Count; i++) {
            Enemy enemy = Combat.Enemies[i];
            Combat.DoOnLast(new CombatAction.ApplyStatus(enemy, new Status.Vulnerable(enemy, 1)), combatAction);
        }
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] -= Activate;
    }
}
