//using System.Collections.Generic;
//using static CardVisual;

//public partial class LegSweep : Skill {
//    int initialWeakStack = 2;
//    int upgradedWeakStack = 3;
//    int WeakStack => IsUpgraded ? upgradedWeakStack : initialWeakStack;
//    int initialBlockValue = 11;
//    int upgradedBlockValue = 14;
//    int BlockValue => IsUpgraded ? upgradedBlockValue : initialBlockValue;


//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        return new() {
//            new CombatAction.ApplyStatus(target, new Status.Weak(target, WeakStack)),
//            new CombatAction.ApplyStatus(caster, new Status.Block(caster, Combat.CalculateBlockGain(target, BlockValue)))
//        };
//    }

//    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
//        int block = BlockValue;
//        if (caster != null) {
//            block = Combat.CalculateBlockGain(caster, block);
//        }
//        return $"Apply {NumberText(initialWeakStack, WeakStack)} {BrownText("Weak")}.\nGain {NumberText(initialBlockValue, block)} {BrownText("Block")}.";
//    }
//}

