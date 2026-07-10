using System.Collections;
using UnityEngine;

public class VNController : MonoBehaviour {
    [SerializeField] VNManager VNManager;
    [SerializeField] VNUI VNUI;
    [SerializeField] bool autoContinueWhenDoneTyping;
    [SerializeField] float autoAfterSecond = 3;
    [SerializeField] VNLogUI VNLogUI;

    private void Start() {
        VNUI.OnDialogueFinishedTyping += OnVNUIFinishedTyping;
    }

    void Update() {
        if (VNManager.CurrentDialogueBlock == null) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            VNLogUI.gameObject.SetActive(!VNLogUI.gameObject.activeInHierarchy);
        }
        if (VNLogUI.gameObject.activeInHierarchy) {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (!VNUI.TypingFinished) {
                VNUI.FinishTyping();
            }
            else {
                VNManager.Continue();
            }
        }
    }

    void OnVNUIFinishedTyping() {
        if (autoContinueWhenDoneTyping) {
            StartCoroutine(ContinueAfterSecond());
        }
    }

    IEnumerator ContinueAfterSecond() {
        float t = 0;
        VNDialogueSequence.DialogueBlock block = VNManager.CurrentDialogueBlock;
        while (t < autoAfterSecond) {
            t += Time.deltaTime;
            yield return null;
        }
        if (block == VNManager.CurrentDialogueBlock) {
            VNManager.Continue();
        }
    }
}
