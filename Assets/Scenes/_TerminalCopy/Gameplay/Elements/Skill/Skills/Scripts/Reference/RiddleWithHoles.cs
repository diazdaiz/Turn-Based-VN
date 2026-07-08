//using System.Collections.Generic;
//using static CardVisual;

//public partial class RiddleWithHoles : Skill {
//    int initialDamageValue = 3;
//    int upgradedDamageValue = 4;
//    int DamageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;
//    int AttackAmount => 5;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        List<CombatAction> combatActions = new List<CombatAction>();
//        for (int i = 0; i < AttackAmount; i++) {
//            combatActions.Add(new CombatAction.Attack(caster, target, DamageValue));
//        }
//        return combatActions;
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        int damage = DamageValue;
//        if (caster != null && target != null) {
//            damage = Combat.CalculateAttack(caster, target, damage);
//        }
//        return $"Deal {NumberText(initialDamageValue, damage)} damage 5 times";
//    }
//}



