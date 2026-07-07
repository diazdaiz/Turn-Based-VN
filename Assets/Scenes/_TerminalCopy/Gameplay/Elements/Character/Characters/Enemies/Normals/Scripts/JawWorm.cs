using System;
using System.Collections.Generic;

public partial class JawWorm : Enemy {
    bool firstIntention = true;
    List<Type> lastThreeIntention;

    public override void Start() {
        lastThreeIntention = new();
    }

    public override void GenerateIntentions() {
        base.GenerateIntentions();
        if (firstIntention) {
            intention = new Intention.Attack(11);
            firstIntention = false;
        }
        else {
            Dictionary<Type, Dz.Random.Randomizable> possibleIntentions = new() {
                { typeof(Intention.Block), new(new Intention.Block(new Status.Strength(this, 3), 6, 6), 45f) },
                { typeof(Intention.AttackBlock), new(new Intention.AttackBlock(5,5,7,7), 30f) },
                { typeof(Intention.Attack), new(new Intention.Attack(11), 25f) }
            };

            //ilangin
            if (lastThreeIntention.Count >= 2) {
                if (lastThreeIntention[0].GetType() == typeof(Intention.Block) && lastThreeIntention[1].GetType() == typeof(Intention.Block)) {
                    possibleIntentions.Remove(typeof(Intention.Block));
                }
                if (lastThreeIntention[0].GetType() == typeof(Intention.Attack) && lastThreeIntention[1].GetType() == typeof(Intention.Attack)) {
                    possibleIntentions.Remove(typeof(Intention.Attack));
                }
            }

            if (lastThreeIntention.Count > 2) {
                if (lastThreeIntention[0].GetType() == typeof(Intention.AttackBlock) && lastThreeIntention[1].GetType() == typeof(Intention.AttackBlock) && lastThreeIntention[2].GetType() == typeof(Intention.AttackBlock)) {
                    possibleIntentions.Remove(typeof(Intention.AttackBlock));
                }
            }

            List<Dz.Random.Randomizable> randomizables = new();
            foreach (Type type in possibleIntentions.Keys) {
                randomizables.Add(possibleIntentions[type]);
            }

            intention = Dz.Random.Randomizer.Randomize<Intention>(randomizables);

            lastThreeIntention.Insert(0, intention.GetType());
            while (lastThreeIntention.Count > 3) {
                lastThreeIntention.RemoveAt(3);
            }
        }
    }
}
