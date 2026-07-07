using UnityEngine;

namespace Dz.SelectionManagement {
    public partial class SelectionManager : MonoBehaviour {
        // public List<Selection> Selections => selections;
        // public Selection HoveredSelection {
        //     get {
        //         return hoveredSelection;
        //     }
        //     set {
        //         hoveredSelection = value;
        //     }
        // }
        // public bool ConfirmForAction => confirmForAction;
        // public bool MultipleSelection => multipleSelection;
        // public int CurrentSelectionsIndex {
        //     get {
        //         return currentSelectionsIndex;
        //     }
        //     set {
        //         currentSelectionsIndex = value;
        //     }
        // }
        // public Selection MouseConfirmSelection => mouseConfirmSelection;

        // [Header("Selections")]
        // [SerializeField] List<Selection> selections;
        // [SerializeField] bool skipHoverDisabledSelection = true;
        // [SerializeField] bool loopHoverWhenOutOfBounds = true;

        // [Header("Action Behavior")]
        // [SerializeField] bool confirmForAction = false;
        // //multiple selection harus confirm
        // [EnableIf(nameof(confirmForAction))]
        // [SerializeField] bool multipleSelection = false;
        // [SerializeField] Selection mouseConfirmSelection;

        // int currentSelectionsIndex = 0;
        // Selection hoveredSelection;

        // public void HoverNext() {
        //     if (selections == null || selections.Count == 0) return;

        //     int next = FindNextEnabledIndex(currentSelectionsIndex, 1, loopHoverWhenOutOfBounds);
        //     if (next == -1) {
        //         Debug.LogWarning("SelectionManager.HoverNext: no enabled selection found.");
        //         return;
        //     }

        //     Hover(next);
        // }

        // public void HoverPrevious() {
        //     if (selections == null || selections.Count == 0) return;

        //     int prev = FindNextEnabledIndex(currentSelectionsIndex, -1, loopHoverWhenOutOfBounds);
        //     if (prev == -1) {
        //         Debug.LogWarning("SelectionManager.HoverPrevious: no enabled selection found.");
        //         return;
        //     }

        //     Hover(prev);
        // }

        // public void Select(int index) {
        //     if (selections == null || index < 0 || index >= selections.Count) return;

        //     Selection selection = selections[index];

        //     if (selection == null) return;
        //     if (!selection.IsEnabled && skipHoverDisabledSelection) return;

        //     if (multipleSelection) {
        //         if (!selection.IsSelected) {
        //             selection.Select();
        //         }
        //         else {
        //             selection.Unselect();
        //         }
        //     }
        //     else {
        //         for (int i = 0; i < selections.Count; i++) {
        //             if (i == index) continue;
        //             if (selections[i].IsSelected) selections[i].Unselect();
        //         }

        //         selection.Select();
        //         currentSelectionsIndex = index;
        //     }

        //     if (!confirmForAction) {
        //         if (multipleSelection) {
        //             if (selection.IsSelected) selection.Confirm();
        //         }
        //         else {
        //             selection.Confirm();
        //         }
        //     }
        // }

        // public void Unselect(int index) {
        //     if (selections != null && selections[index] != null && selections[index].IsSelected) selections[index].Unselect();
        // }

        // public void ConfirmSelections() {
        //     if (selections == null || selections.Count == 0) return;

        //     for (int i = 0; i < selections.Count; i++) {
        //         Selection selection = selections[i];
        //         if (selection != null && selection.IsSelected) {
        //             selection.Confirm();
        //             selection.Unselect();
        //             Debug.Log("Confirming " + selection.gameObject.name);
        //         }
        //     }
        // }

        // void Hover(int index) {
        //     if (selections == null || index < 0 || index >= selections.Count) return;

        //     // Unhover previous
        //     if (hoveredSelection != null && hoveredSelection != selections[index]) {
        //         hoveredSelection.Unhover();
        //     }

        //     hoveredSelection = selections[index];
        //     if (hoveredSelection != null) {
        //         hoveredSelection.Hover();
        //         currentSelectionsIndex = index;
        //     }
        // }

        // int FindNextEnabledIndex(int startIndex, int direction, bool allowLoop) {
        //     // direction: +1 forward, -1 backward
        //     if (selections == null || selections.Count == 0) return -1;

        //     int len = selections.Count;
        //     int idx = startIndex;
        //     // start from next position
        //     idx += direction;

        //     for (int tested = 0; tested < len; tested++) {
        //         if (idx < 0) {
        //             if (allowLoop) idx = len - 1;
        //             else return -1;
        //         }
        //         else if (idx >= len) {
        //             if (allowLoop) idx = 0;
        //             else return -1;
        //         }

        //         var candidate = selections[idx];
        //         if (candidate != null) {
        //             if (!skipHoverDisabledSelection || candidate.IsEnabled) {
        //                 return idx;
        //             }
        //         }

        //         idx += direction;
        //     }

        //     return -1;
        // }

        // private void Start() {
        //     if (selections == null || selections.Count <= 0) return;
        //     currentSelectionsIndex = FindNextEnabledIndex(-1, 1, true);
        //     Hover(currentSelectionsIndex);
        //     if (MouseConfirmSelection != null) {
        //         MouseConfirmSelection.OnSelected += ConfirmSelections;
        //     }
        // }
    }
}
