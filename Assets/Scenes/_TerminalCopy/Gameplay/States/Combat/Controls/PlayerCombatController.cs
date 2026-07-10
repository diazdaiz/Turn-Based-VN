using UnityEngine;

public class PlayerCombatController : MonoBehaviour {
    CombatManager Combat => CombatManager.Instance;
    Skill skill;
    CharacterCombat character;

    void Update() {
        if (character != Combat.CurrentCharacterTurn.Character) {
            character = Combat.CurrentCharacterTurn.Character;
        }

        if (!Combat.PlayerTeam.Contains(character)) {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            skill = character.Skills[0];
            Debug.Log($"selecting skill {character.Skills[0].name}");
        }
        else if (Input.GetKeyDown(KeyCode.W)) {
            skill = character.Skills[1];
            Debug.Log($"selecting skill {character.Skills[1].name}");
        }
        else if (Input.GetKeyDown(KeyCode.E)) {
            skill = character.Skills[2];
            Debug.Log($"selecting skill {character.Skills[2].name}");
        }
        else if (Input.GetKeyDown(KeyCode.R)) {
            skill = character.Skills[3];
            Debug.Log($"selecting skill {character.Skills[3].name}");
        }

        if (skill == null) {
            return;
        }


        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            Combat.CurrentCharacterTurn.BreakTask(new() { new CombatAction.PlaySkill(character, Combat.EnemyTeam[0], skill), new CombatAction.Delay(2.5f) });
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Combat.CurrentCharacterTurn.BreakTask(new() { new CombatAction.PlaySkill(character, Combat.EnemyTeam[1], skill), new CombatAction.Delay(2.5f) });
        }
    }
}
