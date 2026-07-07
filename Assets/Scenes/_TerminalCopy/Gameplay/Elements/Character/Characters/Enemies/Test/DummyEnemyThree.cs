public partial class DummyEnemyThree : Enemy {
    // public override void Awake() {
    //     base.Awake();
    //     //initialStatuses = new() {
    //     //    { typeof(Status.Regenerate) ,new Status.Regenerate(this, 1000, false) }
    //     //};
    // }

    // public override void GenerateIntentions() {
    //     Enemy enemy = this;
    //     for (int i = 0; i < combat.Enemies.Count; i++) {
    //         Debug.Log(combat.Enemies[i].GetType());
    //         if (combat.Enemies[i].GetType() == typeof(DummyEnemyOne)) {
    //             enemy = combat.Enemies[i];
    //             Debug.Log("set enemy jadi enemy 1");
    //             break;
    //         }
    //     }
    //     intention = new Intention.Buff(new Status.Block(enemy, combat.CalculateBlockGain(this, 20)), enemy);
    // }
}
