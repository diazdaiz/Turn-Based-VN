//using System.Collections.Generic;
//using static CardVisual;

//public partial class Bane : Skill {
//    int damageValue = 7;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        List<CombatAction> combatActions = new();

//        combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
//        if (target.Statuses.ContainsKey(typeof(Status.Poison))) {
//            combatActions.Add(new CombatAction.Attack(caster, target, damageValue));
//        }
//        return combatActions;
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        int damage = damageValue;
//        if (caster != null && target != null) {
//            damage = Combat.CalculateAttack(caster, target, damage);
//        }
//        return $"Deal {NumberText(damage, damage)} damage.\nIf the enemy is  Poisoned, deal {NumberText(damage, damage)} damage again.";
//    }
//}


////Deal 7(10) damage. If the enemy is  Poisoned, deal 7(10) damage again.
