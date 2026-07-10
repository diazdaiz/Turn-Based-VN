using UnityEngine;

public class EnemyCombatController : MonoBehaviour {
    CombatManager Combat => CombatManager.Instance;
    Skill skill;
    CharacterCombat character;
    bool hasStart = false;

    void Update() {
        if (character != Combat.CurrentCharacterTurn.Character) {
            character = Combat.CurrentCharacterTurn.Character;
            hasStart = false;
        }

        if (!Combat.EnemyTeam.Contains(character)) {
            return;
        }
        Combat.CurrentCharacterTurn.FinishWhenLastTaskInTaskSequenceCompleted = true;
        Combat.CurrentCharacterTurn.CancelWhenTaskInTaskSequenceCanceled = true;

        skill = character.Skills[0];

        if (skill == null) {
            return;
        }
        if (hasStart) {
            return;
        }

        hasStart = true;
        int playerTeamIndex = Random.Range(0, 2);
        Combat.CurrentCharacterTurn.BreakTask(new() { new CombatAction.PlaySkill(character, Combat.PlayerTeam[0], skill), new CombatAction.Delay(2.5f) });
        Debug.Log(Combat.CurrentCharacterTurn.Character.transform.parent.name);
    }
}
