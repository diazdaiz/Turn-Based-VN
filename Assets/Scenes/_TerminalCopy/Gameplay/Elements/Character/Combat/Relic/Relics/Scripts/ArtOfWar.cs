public partial class ArtOfWar : Relic {
    bool activate = false;
    public override void Subscribe() {
        base.Subscribe();
        activate = false;
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] += Activate;
        CombatAction.OnTriggersFirst[typeof(CombatAction.PlayerTurn)] += PrepareForActivation;
        CombatAction.AfterTriggers[typeof(CombatAction.PlayCard)] += ResetActivationPreparation;
    }

    public void ResetActivationPreparation(CombatAction combatAction) {
        if (combatAction is CombatAction.PlayCard playCard && CombatAction.PlayCard.Card.Type != Card.CardType.Skill) {
            activate = false;
        }
    }

    public void PrepareForActivation(CombatAction combatAction) {
        activate = true;
    }

    public override void Activate(CombatAction combatAction) {
        base.Activate(combatAction);
        if (activate) {
            Combat.DoOnLast(new CombatAction.AddEnergy(1), combatAction);
        }
    }
}
