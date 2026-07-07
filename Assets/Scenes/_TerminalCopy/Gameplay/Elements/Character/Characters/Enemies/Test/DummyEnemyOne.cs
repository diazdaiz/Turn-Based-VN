public partial class DummyEnemyOne : Enemy {
    // [Export] Enemy summonEnemy;

    // public override void Awake() {
    //     base.Awake();

    //     initialStatuses = new() {
    //         { typeof(Status.Regenerate) ,new Status.Regenerate(this, 80, true) }
    //     };
    // }

    // public override void GenerateIntentions() {
    //     possibleIntentions = new() {
    //         new Intention.Attack(4),
    //         new Intention.Debuff(new Status.Weak(combat.Hero, 1)),
    //         new Intention.Debuff(new Status.Vulnerable(combat.Hero, 1))
    //     };
    //     bool summon = true;
    //     for (int i = 0; i < combat.Enemies.Count; i++) {
    //         if (combat.Enemies[i].GetType() == typeof(DummyEnemyThree)) {
    //             summon = false;
    //             break;
    //         }
    //     }
    //     if (summon) {
    //         intention = new Intention.Summon(new() { summonEnemy.GetComponent<Enemy>() }, new() { new(0f, 0f) });
    //     }
    //     else {
    //         base.GenerateIntentions();
    //     }
    // }
}
