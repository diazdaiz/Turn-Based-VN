//using System.Collections.Generic;

//public partial class Catalyst : Skill {
//    int initialMultiplier = 2;
//    int upgradedMultiplier = 3;
//    int Multiplier => IsUpgraded ? upgradedMultiplier : initialMultiplier;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        if (target.Statuses.ContainsKey(typeof(Status.Poison))) {
//            return new() {
//                new CombatAction.ApplyStatus(target, new Status.Poison(target, target.Statuses[typeof(Status.Poison)].Stack * (Multiplier - 1)))
//            };
//        }
//        else {
//            return new() { };
//        }
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        string mult = IsUpgraded ? "Triple" : "Double";
//        return $"{mult} an enemy's Poison.\nExhaust.";
//    }
//}
