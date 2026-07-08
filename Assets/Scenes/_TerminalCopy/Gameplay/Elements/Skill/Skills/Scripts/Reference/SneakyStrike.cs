//using System.Collections.Generic;
//using static CardVisual;

//public partial class SneakyStrike : Skill {
//    int initialDamageValue = 12;
//    int upgradedDamageValue = 16;
//    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

//    bool addEnergy;

//    public void Start() {
//        addEnergy = false;
//        CombatAction.AfterTriggers[typeof(CombatAction.Discards)] += OnDiscard;
//        CombatAction.AfterTriggers[typeof(CombatAction.PlayerTurn)] += OnStartPlayerTurn;
//    }

//    void OnDiscard(CombatAction combatAction) {
//        addEnergy = true;
//    }

//    void OnStartPlayerTurn(CombatAction combatAction) {
//        addEnergy = false;
//    }

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        List<CombatAction> combatActions = new List<CombatAction>();
//        combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
//        if (addEnergy) {
//            combatActions.Add(new CombatAction.AddEnergy(2));
//        }

//        return combatActions;
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        int damage = damageValue;
//        if (caster != null && target != null) {
//            damage = Combat.CalculateAttack(caster, target, damage);
//        }
//        return $"Deal {NumberText(initialDamageValue, damage)} damage. If you have discarded a card this turn, gain 2 Energy.";
//    }

//    private void OnDestroy() {
//        CombatAction.AfterTriggers[typeof(CombatAction.Discards)] -= OnDiscard;
//        CombatAction.AfterTriggers[typeof(CombatAction.PlayerTurn)] -= OnStartPlayerTurn;
//    }
//}

