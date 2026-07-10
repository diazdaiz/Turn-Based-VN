using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class VNManager : MonoBehaviour {
    public List<VNDialogueSequence.DialogueBlock> Log { get; private set; }
    public int CurrentIndex { get; private set; }
    public VNDialogueSequence.DialogueBlock CurrentDialogueBlock { get; private set; }

    VNDialogueSequence dialogueSequence;
    [SerializeField] bool test;
    [ShowIf(nameof(test))][SerializeField] VNDialogueSequence sequence;

    private void Start() {
        Log = new();
        if (test) {
            Run(sequence);
        }
    }

    public void Run(VNDialogueSequence dialogueSequence) {
        this.dialogueSequence = dialogueSequence;
        CurrentIndex = 0;
        CurrentDialogueBlock = dialogueSequence.Sequence[CurrentIndex];
    }

    public void Continue() {
        if (CurrentIndex + 1 >= dialogueSequence.Sequence.Count) {
            Finish();
            return;
        }
        Log.Add(dialogueSequence.Sequence[CurrentIndex]);
        CurrentIndex += 1;
        CurrentDialogueBlock = dialogueSequence.Sequence[CurrentIndex];
    }

    public void FastForward() {
        Log = new(dialogueSequence.Sequence);
        if (CurrentIndex + 1 >= dialogueSequence.Sequence.Count) {
            Finish();
        }
        else {
            for (int i = CurrentIndex + 1; i < dialogueSequence.Sequence.Count; i++) {
                Log.Add(dialogueSequence.Sequence[i]);
            }
            Finish();
        }
    }

    public void Finish() {
        //codingan sementara biar langsung ke combat
        FindAnyObjectByType<VNController>(FindObjectsInactive.Include).gameObject.SetActive(false);
        FindAnyObjectByType<VNUI>(FindObjectsInactive.Include).gameObject.SetActive(false);


        FindAnyObjectByType<PlayerCombatController>(FindObjectsInactive.Include).gameObject.SetActive(true);
        FindAnyObjectByType<EnemyCombatController>(FindObjectsInactive.Include).gameObject.SetActive(true);
        CombatManager combatManager = FindAnyObjectByType<CombatManager>(FindObjectsInactive.Include);
        Character character = FindAnyObjectByType<CharacterRoamController>(FindObjectsInactive.Include).Character;
        Character mao = GameObject.Find("Niziiro Mao").GetComponent<Character>();
        Character foster = GameObject.Find("Ren Foster").GetComponent<Character>();
        Character jin = GameObject.Find("Jin Natori").GetComponent<Character>();
        Character ghost = GameObject.Find("Ghost").GetComponent<Character>();

        if (character == mao) {
            combatManager.SetCombatInitialProperties(new() { mao.Combat, foster.Combat }, new() { jin.Combat, ghost.Combat }, false);
        }
        if (character == foster) {
            combatManager.SetCombatInitialProperties(new() { foster.Combat, mao.Combat }, new() { jin.Combat, ghost.Combat }, false);
        }
        if (character == jin) {
            combatManager.SetCombatInitialProperties(new() { jin.Combat, ghost.Combat }, new() { foster.Combat, mao.Combat }, true);
        }
        if (character == ghost) {
            combatManager.SetCombatInitialProperties(new() { ghost.Combat, jin.Combat }, new() { foster.Combat, mao.Combat }, true);
        }

        combatManager.StartCombat();
    }
}
