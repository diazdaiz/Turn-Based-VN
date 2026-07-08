//using System.Collections.Generic;
//using static CardVisual;

//public partial class Burst : Skill {
//    int initialStack = 1;
//    int upgradedStack = 2;
//    int Stack => IsUpgraded ? upgradedStack : initialStack;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        return new() {
//            new CombatAction.ApplyStatus(caster, new Status.Burst(target, Stack, this))
//        };
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        string skill = "Skill is";
//        if (IsUpgraded) {
//            skill = $"{NumberText(initialStack, Stack)} Skills are";
//        }
//        return $"This turn, your next {skill} played twice";
//    }
//}
