using System.Collections.Generic;
using static CardVisual;

public partial class MasterfulStab : Card {
    int initialDamageValue = 12;
    int upgradedDamageValue = 16;
    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
    int cost = 0;

    private void Awake() {
        CombatAction.AfterTriggers[typeof(CombatAction.Damage)] += OnDamaged;
    }

    void OnDamaged(CombatAction combatAction) {
        CombatAction.Damage damagedCA = (CombatAction.Damage)combatAction;
        if (damagedCA.Receiver != Combat.Hero) {
            return;
        }
        cost += 1;
    }

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return new() {
            new CombatAction.Attack(caster, target, DamageValue)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        int damage = DamageValue;
        if (caster != null && target != null) {
            damage = Combat.CalculateAttack(caster, target, damage);
        }
        return $"Costs 1 additional Energy for each time you lose HP this combat.\nDeal {NumberText(initialDamageValue, damage)} damage.";
    }

    protected override int GetEnergyForActivation() {
        return cost;
    }

    private void OnDestroy() {
        CombatAction.AfterTriggers[typeof(CombatAction.Damage)] -= OnDamaged;
    }
}

