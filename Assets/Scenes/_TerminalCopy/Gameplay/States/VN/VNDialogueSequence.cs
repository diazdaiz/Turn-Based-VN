using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class VNDialogueSequence : MonoBehaviour {
    public List<DialogueBlock> Sequence => sequence;
    [SerializeField] List<DialogueBlock> sequence;

    [System.Serializable]
    public class DialogueBlock {
        public bool multipleCharacter;
        //ch1
        [HideIf(nameof(multipleCharacter))][AllowNesting] public CharacterVN character;
        [HideIf(nameof(multipleCharacter))][AllowNesting] public CharacterVN.CharacterVNExpression expression;
        [HideIf(nameof(multipleCharacter))][AllowNesting] public bool triggerExpressionAnimation;
        //or
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public CharacterVN character1;
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public CharacterVN.CharacterVNExpression expression1;
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public bool triggerExpressionAnimation1;
        //ch2
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public CharacterVN character2;
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public CharacterVN.CharacterVNExpression expression2;
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public bool triggerExpressionAnimation2;

        //focus & name to show
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public bool focusCharacter1;
        [ShowIf(nameof(multipleCharacter))][AllowNesting] public bool focusCharacter2;
        [ShowIf(nameof(multipleCharacter))][Dropdown(nameof(NameToShow))] public string nameToShow;
        private List<string> NameToShow {
            get {
                return new List<string>() { "Character 1", "Character 2" };
            }
        }

        public AudioClip dub;
        [TextArea(3, 10)] public string text;
    }
}
