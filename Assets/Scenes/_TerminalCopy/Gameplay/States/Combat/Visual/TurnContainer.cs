using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnContainer : MonoBehaviour {
    CombatManager Combat => CombatManager.Instance;
    Dictionary<CombatAction.CharacterTurn, GameObject> turnsVisualizer;
    Dictionary<CombatAction.CharacterTurn, TextMeshProUGUI> textsVisualizer;
    GameObject TurnVisualizerTemplate {
        get {
            if (turnVisualizerTemplate == null) {
                turnVisualizerTemplate = transform.GetChild(0).gameObject;
            }
            return turnVisualizerTemplate;
        }
    }
    GameObject turnVisualizerTemplate;

    private void Start() {
        turnsVisualizer = new();
        textsVisualizer = new();
    }

    void Refresh() {
        foreach (GameObject item in turnsVisualizer.Values) {
            Destroy(item);
        }
        turnsVisualizer = new();
        textsVisualizer = new();
    }

    void Update() {
        if (Combat.CharacterTurnsInOrder == null) {
            return;
        }
        Refresh();

        for (int i = 0; i < Combat.CharacterTurnsInOrder.Count; i++) {
            CombatAction.CharacterTurn characterTurn = Combat.CharacterTurnsInOrder[i];
            if (!turnsVisualizer.ContainsKey(characterTurn)) {
                GameObject newTurnVisualizer = Instantiate(TurnVisualizerTemplate, transform);
                Image image = newTurnVisualizer.transform.GetChild(0).gameObject.GetComponent<Image>();
                TextMeshProUGUI text = newTurnVisualizer.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
                image.sprite = characterTurn.Character.Portrait;
                text.text = characterTurn.Character.ActionValue.ToString();
                turnsVisualizer.Add(characterTurn, newTurnVisualizer);
                textsVisualizer.Add(characterTurn, text);
            }
        }

        int smallest = 99999;
        for (int i = 0; i < Combat.CharacterTurnsInOrder.Count; i++) {
            int av = Combat.CharacterTurnsInOrder[i].Character.ActionValue;
            if (av < smallest) smallest = av;
        }

        foreach (CombatAction.CharacterTurn characterTurn in turnsVisualizer.Keys) {
            for (int i = 0; i < Combat.CharacterTurnsInOrder.Count; i++) {
                if (turnsVisualizer.ContainsKey(characterTurn)) {
                    turnsVisualizer[characterTurn].SetActive(true);
                    textsVisualizer[characterTurn].text = (characterTurn.Character.ActionValue - smallest).ToString();
                }
            }
        }
    }
}
