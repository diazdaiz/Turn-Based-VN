using System.Collections.Generic;
using static CardVisual;

public partial class AllOutAttack : Card {
    int initialDamageValue = 10;
    int upgradedDamageValue = 14;
    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
        List<Card> cards = new List<Card>(Combat.HandPile);
        cards.Remove(this);

        if (cards.Count > 0) {
            return new(){
                new CombatAction.Attack(caster, targets, DamageValue),
                new CombatAction.Discards(cards[Dz.Random.Randomizer.Range(0, cards.Count - 1)])
            };
        }
        else {
            return new() {
                new CombatAction.Attack(caster, targets, DamageValue)
            };
        }
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = DamageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage to ALL enemies.\nDiscard 1 card at random.";
    }
}
