using System;
using TMPro;
using UnityEngine;

public class VNUI : MonoBehaviour {
    public Action OnDialogueFinishedTyping { get; set; }
    public bool TypingFinished { get; private set; }

    [SerializeField] VNManager VNManager;
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TextMeshProUGUI nameTMP;
    [SerializeField] TextMeshProUGUI dialogueTMP;
    [SerializeField] float letterPerSecond = 50;

    VNDialogueSequence.DialogueBlock block;
    //text
    //portrait 1
    //portrait 2

    float t;

    public void FinishTyping() {
        OnDialogueFinishedTyping?.Invoke();
        dialogueTMP.text = block.text;
        TypingFinished = true;
    }

    void Update() {
        if (block != VNManager.CurrentDialogueBlock) {
            t = 0;
            TypingFinished = false;
            block = VNManager.CurrentDialogueBlock;
        }
        t += Time.deltaTime;

        if (block != null) {
            if (!dialoguePanel.gameObject.activeInHierarchy) {
                dialoguePanel.SetActive(true);
            }
            if (block.multipleCharacter) {
                nameTMP.text = block.nameToShow == "Character 1" ? block.character1.DisplayName : block.character2.DisplayName;
                block.character1.SetExpression(block.expression1, block.triggerExpressionAnimation1, -1);
                block.character2.SetExpression(block.expression2, block.triggerExpressionAnimation2, 1);
            }
            else {
                nameTMP.text = block.character.DisplayName;
                block.character.SetExpression(block.expression, block.triggerExpressionAnimation, 0);
            }
            if (dialogueTMP.text != block.text) {
                dialogueTMP.text = block.text.Substring(0, Mathf.Min(block.text.Length, (int)(letterPerSecond * t)));
            }
            if (!TypingFinished && dialogueTMP.text == block.text) {
                FinishTyping();
            }
        }
        else {
            if (dialoguePanel.gameObject.activeInHierarchy) {
                dialoguePanel.SetActive(false);
            }
        }
    }
}
