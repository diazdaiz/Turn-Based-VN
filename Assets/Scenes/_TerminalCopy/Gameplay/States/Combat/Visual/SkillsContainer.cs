using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsContainer : MonoBehaviour {
    CombatManager Combat => CombatManager.Instance;
    Dictionary<Skill, GameObject> skillsVisualizer;
    GameObject SkillVisualizerTemplate {
        get {
            if (skillVisualizerTemplate == null) {
                skillVisualizerTemplate = transform.GetChild(0).gameObject;
            }
            return skillVisualizerTemplate;
        }
    }
    GameObject skillVisualizerTemplate;

    private void Start() {
        skillsVisualizer = new();
    }

    void Update() {
        if (Combat.CurrentCharacterTurn == null) {
            return;
        }

        CharacterCombat character = Combat.CurrentCharacterTurn.Character;
        for (int i = 0; i < character.Skills.Count; i++) {
            Skill skill = character.Skills[i];
            if (!skillsVisualizer.ContainsKey(skill)) {
                GameObject newSkillVisualizer = Instantiate(SkillVisualizerTemplate, transform);
                Image image = newSkillVisualizer.transform.GetChild(0).gameObject.GetComponent<Image>();
                image.sprite = skill.GetComponent<SpriteRenderer>().sprite;
                skillsVisualizer.Add(skill, newSkillVisualizer);
            }
        }

        foreach (Skill skill in skillsVisualizer.Keys) {
            for (int i = 0; i < character.Skills.Count; i++) {
                if (character.Skills.Contains(skill)) {
                    skillsVisualizer[skill].SetActive(true);
                }
                else {
                    skillsVisualizer[skill].SetActive(false);
                }
            }
        }
    }
}
