using UnityEngine;

public partial class Character : MonoBehaviour {
    public CharacterMovement Movement { get; set; }
    public CharacterCombat Combat { get; set; }
    public CharacterVN VN { get; set; }
    public CharacterState State { get; set; }

    public enum CharacterState { Roam, VN, Combat }

    private void Awake() {
        Movement = GetComponentInChildren<CharacterMovement>();
        Combat = GetComponentInChildren<CharacterCombat>();
        VN = GetComponentInChildren<CharacterVN>();
    }
}
