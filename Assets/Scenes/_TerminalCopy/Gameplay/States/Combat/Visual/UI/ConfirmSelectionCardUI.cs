
using UnityEngine;

public partial class ConfirmSelectionCardUI : MonoBehaviour {
    // [Export] Collider2D confirmCollider;
    // CombatManager combat => CombatManager.Instance;

    // private void Awake() {
    //     if (confirmCollider == null) {
    //         confirmCollider = GetComponent<Collider2D>();
    //     }
    // }

    // void Update() {
    //     if (combat.CurrentAction == null) {
    //         return;
    //     }
    //     CombatAction.CardSelection cardSelection = combat.CurrentAction[^1] as CombatAction.CardSelection;
    //     if (cardSelection == null) {
    //         return;
    //     }
    //     if (Mouse.current.press.wasPressedThisFrame && confirmCollider.OverlapPoint(Mouse.current.GlobalPosition())) {
    //         cardSelection.Confirm();
    //     }
    // }
}
