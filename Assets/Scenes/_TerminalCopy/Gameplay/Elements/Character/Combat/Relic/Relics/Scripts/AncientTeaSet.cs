public partial class AncientTeaSet : Relic {
    public override void Subscribe() {
        base.Subscribe();
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] += Activate;
    }

    public override void Activate(CombatAction combatAction) {
        //base.Activate(combatAction);
        //MapEvent previousMapEvent = ExplorationManager.Instance.Map.PreviousSelectedRoom;
        //if (combatAction is CombatAction.PlayerTurn && previousMapEvent != null && previousMapEvent.eventType == MapEvent.EventType.Campfire) {
        //    Combat.DoOnLast(new CombatAction.AddEnergy(2), combatAction);
        //}
        //CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] -= Activate;
    }
    //2 energy setelah rest site
}
