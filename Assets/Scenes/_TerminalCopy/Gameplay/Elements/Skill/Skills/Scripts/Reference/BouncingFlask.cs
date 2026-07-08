//using System.Collections.Generic;
//using static CardVisual;

//public partial class BouncingFlask : Skill {
//    int BounceAmount = 3;

//    public override List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
//        List<CombatAction> combatActions = new List<CombatAction>();
//        for (int i = 0; i < BounceAmount; i++) {
//            int index = Dz.Random.Randomizer.Range(0, targets.Count - 1);
//            combatActions.Add(new CombatAction.ApplyStatus(targets[index], new Status.Poison(targets[index], 3), caster));
//        }
//        return combatActions;
//    }

//    public override string GetDescription(CharacterCombat caster, CharacterCombat target) {
//        return $"Apply 3 {BrownText("Poison")} to a random enemy {NumberText(BounceAmount, BounceAmount)} times.";
//    }
//}
