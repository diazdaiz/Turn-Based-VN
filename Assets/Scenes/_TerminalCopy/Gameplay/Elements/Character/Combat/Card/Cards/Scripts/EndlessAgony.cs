using System.Collections.Generic;
using static CardVisual;

public partial class EndlessAgony : Card {
    int initialDamageValue = 4;
    int upgradedDamageValue = 6;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    private void Awake() {
        CombatAction.AfterTriggers[typeof(CombatAction.Draw)] += OnAfterDraw;
    }

    void OnAfterDraw(CombatAction combatAction) {
        CombatAction.Draw draw = (CombatAction.Draw)combatAction;
        if (draw == null) {
            return;
        }
        if (draw.Card.GetType() == typeof(EndlessAgony)) {
            Combat.DoAfter(new CombatAction.CreateCard(draw.Card, Combat.HandPile), draw);
        }
    }

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, damageValue)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Whenever you draw this card, add a copy of it to your hand. Deal {NumberText(initialDamageValue, damage)} damage.\n{BrownText("Exhaust")}.";
    }

    private void OnDestroy() {
        CombatAction.AfterTriggers[typeof(CombatAction.Draw)] -= OnAfterDraw;
    }
}

