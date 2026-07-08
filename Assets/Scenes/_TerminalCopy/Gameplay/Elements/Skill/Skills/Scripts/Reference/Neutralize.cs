//using System.Collections.Generic;
//using static CardVisual;

//public partial class Neutralize : Skill {
//    int damageValue = 3;
//    int weakTurn = 1;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        return new() {
//            new CombatAction.Attack(caster, target, damageValue),
//            new CombatAction.ApplyStatus(target, new Status.Weak(target, weakTurn))
//        };
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        int damage = damageValue;
//        if (caster != null) {
//            damage = Combat.CalculateAttack(caster, target, damage);
//        }
//        return $"Deal {NumberText(damageValue, damage)} damage.\nApply {NumberText(weakTurn, weakTurn)} {BrownText("Weak")}.";
//    }
//}
