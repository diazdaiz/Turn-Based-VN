using System.Collections.Generic;

//[GlobalClass]
public partial class Enemy : CharacterCombat {
    public Intention intention;
    public List<Intention> possibleIntentions;

    // /// <summary>
    // /// by default, chance semua possible Intentionsnya sama
    // /// </summary>
    public virtual void GenerateIntentions() {
        intention = new();
        if (possibleIntentions == null) {
            possibleIntentions = new List<Intention>() {
                new Intention.Attack(1)
            };
        }
        if (possibleIntentions.Count > 0) {
            int rng = Dz.Random.Randomizer.Range(0, possibleIntentions.Count);
            intention = possibleIntentions[rng];
        }
    }

    public virtual List<CombatAction> ActivateIntention() {
        if (intention.GetType() == typeof(Intention.Attack)) {
            return intention.Activate(this, combat.Hero);
        }
        else if (intention.GetType() == typeof(Intention.Buff)) {
            Intention.Buff intentionBuff = (Intention.Buff)intention;
            if (intentionBuff.target != null) {
                return intention.Activate(this, intentionBuff.target);
            }
            else {
                return intention.Activate(this, this);
            }
        }
        else if (intention.GetType() == typeof(Intention.Debuff)) {
            return intention.Activate(this, combat.Hero);
        }
        else if (intention.GetType() == typeof(Intention.Block)) {
            return intention.Activate(this, this);
        }
        else if (intention.GetType() == typeof(Intention.AttackBlock)) {
            return intention.Activate(this, combat.Hero);
        }
        else if (intention.GetType() == typeof(Intention.Summon)) {
            return intention.Activate(this, this);
        }
        // Debug.LogError("gk dapet intention");
        return null;
    }

    public override void Start() {
        //unity initialize
    }

    public override void OnDestroy() {

    }
}
