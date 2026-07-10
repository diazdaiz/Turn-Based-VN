using UnityEngine;

public class RoamManager : MonoBehaviour {
    [SerializeField] CharacterRoamController roamController;
    //codingan sementara untuk mempercepat langsung ke dialogue & fight
    [SerializeField] bool skipRoam;

    public void Run(Character character) {
        roamController.Character = character;

        //codingan sementara untuk mempercepat langsung ke dialogue & fight
        if (skipRoam) {
            roamController.SetCamera(character.transform.eulerAngles.y + 180f);
            FindAnyObjectByType<VNUI>(FindObjectsInactive.Include).gameObject.SetActive(true);
            FindAnyObjectByType<VNController>(FindObjectsInactive.Include).gameObject.SetActive(true);

            VNManager vNManager = FindAnyObjectByType<VNManager>(FindObjectsInactive.Include);
            VNDialogueSequence dialogueSequence = FindAnyObjectByType<VNDialogueSequence>(FindObjectsInactive.Include);
            vNManager.Run(dialogueSequence);
            return;
        }

        roamController.gameObject.SetActive(true);
    }
}
