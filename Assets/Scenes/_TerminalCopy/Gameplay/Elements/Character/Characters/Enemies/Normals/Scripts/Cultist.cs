public partial class Cultist : Enemy {
    public override void GenerateIntentions() {
        base.GenerateIntentions();
        if (!Statuses.ContainsKey(typeof(Status.Ritual))) {
            intention = new Intention.Buff(new Status.Ritual(this, 3));
        }
        else {
            intention = new Intention.Attack(3);
        }
    }
}
