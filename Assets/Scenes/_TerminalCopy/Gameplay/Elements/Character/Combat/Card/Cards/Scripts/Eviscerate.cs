using System.Collections.Generic;
using UnityEngine;
using static CardVisual;

public partial class Eviscerate : Card {
    int initialDamageValue = 7;
    int upgradedDamageValue = 9;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int initialEnergyActivationValue => 3;
    int stack;

    public void Start() {
        stack = 0;
        CombatAction.AfterTriggers[typeof(CombatAction.Discard)] += OnDiscard;
        CombatAction.BeforeTriggers[typeof(CombatAction.PlayerTurn)] += OnStartPlayerTurn;
    }

    void OnDiscard(CombatAction combatAction) {
        stack += 1;
    }

    void OnStartPlayerTurn(CombatAction combatAction) {
        stack = 0;
    }

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        List<CombatAction> combatActions = new();
        for (int i = 0; i < 3; i++) {
            combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
        }
        return combatActions;
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = damageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Costs 1 less Energy for each card discarded this turn. Deal {NumberText(initialDamageValue, damage)} damage three times.";
    }

    protected override int GetEnergyForActivation() {
        return Mathf.Max(0, initialEnergyActivationValue - stack);
    }

    public void OnDestroy() {
        CombatAction.AfterTriggers[typeof(CombatAction.Discard)] -= OnDiscard;
        CombatAction.BeforeTriggers[typeof(CombatAction.PlayerTurn)] -= OnStartPlayerTurn;
    }
}


