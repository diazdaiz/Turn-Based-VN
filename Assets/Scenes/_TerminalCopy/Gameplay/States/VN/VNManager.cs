using System.Collections.Generic;
using UnityEngine;

public class VNManager : MonoBehaviour {
    public List<VNDialogueSequence.DialogueBlock> Log { get; private set; }
    public int CurrentIndex;
    VNDialogueSequence dialogueSequence;

    public void Run(VNDialogueSequence dialogueSequence) {
        this.dialogueSequence = dialogueSequence;
        CurrentIndex = 0;
    }

    public void Continue() {

    }

    public void FastForward() {
        //for
    }
}
