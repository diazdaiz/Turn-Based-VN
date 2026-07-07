using Dz.Random;

public partial class DummyEnemyTwo : Enemy {

    public override void GenerateIntentions() {
        possibleIntentions = new() {
            new Intention.Debuff(new Status.Weak(combat.Hero, 1)),
            new Intention.Debuff(new Status.Vulnerable(combat.Hero, 1)),
            new Intention.Debuff(new Status.Frail(combat.Hero, 1))
        };
        int rng = Randomizer.Range(0, possibleIntentions.Count - 1);
        intention = possibleIntentions[rng];
    }
}
