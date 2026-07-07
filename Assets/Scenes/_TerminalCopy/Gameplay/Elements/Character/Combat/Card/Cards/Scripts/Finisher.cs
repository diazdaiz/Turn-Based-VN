using System.Collections.Generic;
using static CardVisual;

public partial class Finisher : Card {
    int initialDamageValue = 6;
    int upgradedDamageValue = 8;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    int stack;

    private void Awake() {
        stack = 0;
        CombatAction.AfterTriggers[typeof(CombatAction.Attack)] += OnAttack;
        CombatAction.AfterTriggers[typeof(CombatAction.PlayerTurn)] += OnStartPlayerTurn;
        //onattack stack += 1
        //onstart turn reset
    }

    void OnAttack(CombatAction combatAction) {
        stack += 1;
    }

    void OnStartPlayerTurn(CombatAction combatAction) {
        stack = 0;
    }

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        List<CombatAction> combatActions = new List<CombatAction>();
        for (int i = 0; i < stack; i++) {
            combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
        }
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Deal {NumberText(initialDamageValue, damage)} damage for each Attack played this turn.";
    }

    private void OnDestroy() {

    }
}
