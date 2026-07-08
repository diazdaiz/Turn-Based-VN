//using System.Collections.Generic;
//using static CardVisual;

//public partial class AThousandCuts : Skill {
//    int initialDamageStack = 1;
//    int upgradedDamageStack = 2;
//    int damageStack => IsUpgraded ? upgradedDamageStack : initialDamageStack;

//    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
//        return new() {
//            new CombatAction.ApplyStatus(target, new Status.AThousandCuts(caster, stack: damageStack))
//        };
//    }

//    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
//        return $"Whenever you play a card, deal {NumberText(initialDamageStack, damageStack)} damage to ALL enemies.";
//    }
//}
