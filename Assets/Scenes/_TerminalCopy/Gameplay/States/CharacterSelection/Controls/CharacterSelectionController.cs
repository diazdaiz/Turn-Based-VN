using Dz.SelectionManagement;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionController : MonoBehaviour {
    [SerializeField] CharacterSelectionManager characterSelectionManager;
    [SerializeField] SelectionManager selectionManager;
    [SerializeField] List<Character> characterSelectionList;

    void Start() {
        selectionManager.Selections[0].OnConfirmed += SelectFoster;
        selectionManager.Selections[1].OnConfirmed += SelectMao;
        selectionManager.Selections[2].OnConfirmed += SelectNatori;
        selectionManager.Selections[3].OnConfirmed += SelectGhost;
    }

    public void SelectFoster() {
        characterSelectionManager.SelectCharacter(characterSelectionList[0]);
        characterSelectionList[0].GetComponent<Rigidbody>().isKinematic = false;
        characterSelectionList[1].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[2].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[3].GetComponent<Rigidbody>().isKinematic = true;
    }

    public void SelectMao() {
        characterSelectionManager.SelectCharacter(characterSelectionList[1]);
        characterSelectionList[0].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[1].GetComponent<Rigidbody>().isKinematic = false;
        characterSelectionList[2].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[3].GetComponent<Rigidbody>().isKinematic = true;

    }

    public void SelectNatori() {
        characterSelectionManager.SelectCharacter(characterSelectionList[2]);
        characterSelectionList[0].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[1].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[2].GetComponent<Rigidbody>().isKinematic = false;
        characterSelectionList[3].GetComponent<Rigidbody>().isKinematic = true;
    }

    public void SelectGhost() {
        characterSelectionManager.SelectCharacter(characterSelectionList[3]);
        characterSelectionList[0].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[1].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[2].GetComponent<Rigidbody>().isKinematic = true;
        characterSelectionList[3].GetComponent<Rigidbody>().isKinematic = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SelectFoster();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SelectMao();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SelectNatori();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SelectGhost();
        }
    }
}
