//using System.Collections.Generic;
//using static CardVisual;

//public partial class Strike : Skill {
//    int damageValue = 6;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        return new() {
//            new CombatAction.Attack(caster, target, damageValue)
//        };
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        int damage = damageValue;
//        if (caster != null) {
//            damage = Combat.CalculateAttack(caster, target, damage);
//        }
//        return $"Deal {NumberText(damage, damage)} damage.";
//    }
//}
