using System.Collections.Generic;
using UnityEngine;

public class VNDialogueSequence : MonoBehaviour {
    public List<DialogueBlock> Sequence => sequence;
    [SerializeField] List<DialogueBlock> sequence;

    [System.Serializable]
    public class DialogueBlock {
        public CharacterVN character;
        public CharacterVN.CharacterVNExpression expression; //nanti correspond ke gmn 
        public AudioClip dub;
        public string text;
    }
}
