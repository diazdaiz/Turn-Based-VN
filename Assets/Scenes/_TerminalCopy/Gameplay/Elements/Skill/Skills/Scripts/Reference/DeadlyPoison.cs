//using System.Collections.Generic;
//using static CardVisual;

//public partial class DeadlyPoison : Skill {
//    int initialPoisonStack = 5;
//    int upgradedPoisonStack = 7;
//    int PoisonStack => IsUpgraded ? upgradedPoisonStack : initialPoisonStack;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        return new() {
//            new CombatAction.ApplyStatus(target, new Status.Poison(target, PoisonStack), caster)
//        };
//    }

//    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
//        return $"Apply {NumberText(initialPoisonStack, PoisonStack)} {BrownText("Poison")}.";
//    }
//}

