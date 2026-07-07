using UnityEngine;

public partial class Character : MonoBehaviour {
    public CharacterMovement Movement { get; set; }
    public CharacterCombat Combat { get; set; }
    public CharacterVN VN { get; set; }

    private void Awake() {
        Movement = GetComponentInChildren<CharacterMovement>();
        Combat = GetComponentInChildren<CharacterCombat>();
        VN = GetComponentInChildren<CharacterVN>();
    }

    public virtual void OnDestroy() {

    }
}
