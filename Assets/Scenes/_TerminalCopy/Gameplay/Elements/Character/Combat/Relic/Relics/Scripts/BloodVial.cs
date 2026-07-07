public partial class BloodVial : Relic {
    public override void Subscribe() {
        base.Subscribe();
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] += Activate;
    }

    public override void Activate(CombatAction combatAction) {
        base.Activate(combatAction);
        Combat.Hero.HP += 2;
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] -= Activate;
    }
}
