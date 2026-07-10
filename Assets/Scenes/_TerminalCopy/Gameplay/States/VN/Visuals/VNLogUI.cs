using System.Collections.Generic;
using UnityEngine;

public class VNLogUI : MonoBehaviour {
    [SerializeField] VNManager VNManager;
    VNLogBlockUI logBlockUI;
    Dictionary<VNDialogueSequence.DialogueBlock, VNLogBlockUI> logBlockUIs;

    private void Awake() {
        logBlockUI = transform.GetChild(0).GetComponent<VNLogBlockUI>();
    }

    void Start() {
        logBlockUIs = new();
    }

    void Update() {
        for (int i = 0; i < VNManager.Log.Count; i++) {
            if (!logBlockUIs.ContainsKey(VNManager.Log[i])) {
                VNLogBlockUI newLog = Instantiate(logBlockUI, transform);
                newLog.Block = VNManager.Log[i];
                newLog.gameObject.SetActive(true);

                logBlockUIs.Add(VNManager.Log[i], newLog);
            }
        }
    }
}
