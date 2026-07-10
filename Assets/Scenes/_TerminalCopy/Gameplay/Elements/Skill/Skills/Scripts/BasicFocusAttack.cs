using System.Collections.Generic;
using UnityEngine;

public class BasicFocusAttack : Skill {
    public static Dictionary<CharacterCombat, CharacterCombat> CharacterFocusingOtherCharacter;

    [SerializeField] int damagePercentage = 100;

    private void Start() {
        CharacterFocusingOtherCharacter = new();
    }

    public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        int stack = 0;
        CombatAction.RemoveStatus removeStatus = null;
        if (CharacterFocusingOtherCharacter.ContainsKey(caster)) {
            removeStatus = new CombatAction.RemoveStatus(CharacterFocusingOtherCharacter[caster], typeof(Status.Focused));
        }
        if (caster.Statuses.ContainsKey(typeof(Status.Artist))) {
            stack = caster.Statuses[typeof(Status.Artist)].Stack;
        }
        return new() {
            new CombatAction.Attack(caster, target, damagePercentage)
        };
    }
}
