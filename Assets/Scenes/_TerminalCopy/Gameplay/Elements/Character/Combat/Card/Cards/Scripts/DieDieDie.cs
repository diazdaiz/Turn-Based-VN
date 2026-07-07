using System.Collections.Generic;
using static CardVisual;

public partial class DieDieDie : Card {
    int initialDamageValue = 13;
    int upgradedDamageValue = 17;
    int damageValue => IsUpgraded ? upgradedDamageValue : initialDamageValue;

    public override List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
        return new() {
            new CombatAction.Attack(caster, new List<CharacterCombat>(Combat.Enemies), damageValue)
        };
    }

    public override string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return $"Deal {BrownText($"{damageValue}")} damage to ALL enemies.\n{BrownText("Exhaust")}.";
    }
}

