using TMPro;
using UnityEngine;

public class VNLogBlockUI : MonoBehaviour {
    public VNDialogueSequence.DialogueBlock Block { get; set; }
    public string DisplayName {
        get {
            string displayName = "";
            if (Block.multipleCharacter) {
                displayName = Block.nameToShow == "Character 1" ? Block.character1.DisplayName : Block.character2.DisplayName;
            }
            else {
                displayName = Block.character.DisplayName;
            }
            return displayName;
        }
    }
    public string DialogueText => Block.text;

    [SerializeField] TextMeshProUGUI tmp;

    void Update() {
        tmp.text = $"{DisplayName}: {DialogueText}";
    }
}
