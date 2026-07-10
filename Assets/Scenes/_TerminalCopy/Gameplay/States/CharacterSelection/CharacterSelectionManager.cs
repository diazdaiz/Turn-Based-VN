using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour {
    [SerializeField] RoamManager roamManager;
    [SerializeField] CharacterSelectionController characterSelectionController;
    [SerializeField] GameObject characterSelectionCanvas;

    public void SelectCharacter(Character character) {
        roamManager.Run(character);
        characterSelectionController.gameObject.SetActive(false);
        characterSelectionCanvas.gameObject.SetActive(false);
    }

    void Update() {

    }
}
