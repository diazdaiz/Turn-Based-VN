using System.Collections.Generic;
using UnityEngine;

namespace Dz.SelectionManagement {
    public partial class SelectionControl : MonoBehaviour {
        [SerializeField] SelectionManager selectionManager;
        [Header("Input")]
        [SerializeField] bool useKeyboard = true;
        [SerializeField] bool useMouse = true;

        [Header("Keyboard Navigation")]
        [SerializeField] KeyCode nextKey = KeyCode.DownArrow;
        [SerializeField] KeyCode previousKey = KeyCode.UpArrow;
        [SerializeField] KeyCode selectKey = KeyCode.Return;
        //kalau perlu confirm for action
        [SerializeField] KeyCode confirmKey = KeyCode.Space;

        [Header("Mouse Navigation")]
        //kalau perlu confirm for action

        List<Selection> selections {
            get {
                return selectionManager.Selections;
            }
        }

        Camera cachedMainCamera;
        Vector3 lastMousePos;

        Selection RaycastSelectionUnderMouse() {
            if (cachedMainCamera == null) cachedMainCamera = Camera.main;
            if (cachedMainCamera == null) return null;

            Vector3 screenPos = UnityEngine.Input.mousePosition;

            // 2D
            Vector3 worldPoint3D = cachedMainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10));
            //Debug.Log(screenPos);
            //Debug.Log(worldPoint2D);
            RaycastHit2D hit2d = Physics2D.Raycast(worldPoint3D, cachedMainCamera.transform.forward);
            if (hit2d.collider != null) {
                //Debug.Log(hit2d.collider);
                return hit2d.collider.GetComponentInParent<Selection>() ?? hit2d.collider.GetComponent<Selection>();
            }

            // 3D
            Ray ray = cachedMainCamera.ScreenPointToRay(screenPos);
            RaycastHit hit3d;
            if (Physics.Raycast(ray, out hit3d)) {
                return hit3d.collider.GetComponentInParent<Selection>() ?? hit3d.collider.GetComponent<Selection>();
            }

            return null;
        }

        int GetIndexOf(Selection selection) {
            if (selections == null || selection == null) return -1;
            for (int i = 0; i < selections.Count; i++) {
                if (selections[i] == selection) return i;
            }

            return -1;
        }

        void HandleKeyboard() {
            if (UnityEngine.Input.GetKeyDown(nextKey)) {
                selectionManager.HoverNext();
            }

            if (UnityEngine.Input.GetKeyDown(previousKey)) {
                selectionManager.HoverPrevious();
            }

            if (selectionManager.ConfirmForAction) {
                if (UnityEngine.Input.GetKeyDown(selectKey)) {
                    if (!selections[selectionManager.CurrentSelectionsIndex].IsSelected) {
                        selectionManager.Select(selectionManager.CurrentSelectionsIndex);
                    }
                    else if (selections[selectionManager.CurrentSelectionsIndex].IsSelected) {
                        selectionManager.Unselect(selectionManager.CurrentSelectionsIndex);
                    }
                }
                if (UnityEngine.Input.GetKeyDown(confirmKey)) {
                    selectionManager.ConfirmSelections();
                }
            }
            else {
                if (UnityEngine.Input.GetKeyDown(selectKey)) {
                    selectionManager.Select(selectionManager.CurrentSelectionsIndex);
                    selectionManager.ConfirmSelections();
                }
            }
        }

        void HandleMouse() {
            Vector3 screenPos = UnityEngine.Input.mousePosition;
            Vector3 worldPoint3D = cachedMainCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 10));

            Selection hitSelection = RaycastSelectionUnderMouse();

            if (UnityEngine.Input.GetMouseButtonDown(0)) {
                if (hitSelection != null) {
                    if (hitSelection == selectionManager.MouseConfirmSelection && selectionManager.ConfirmForAction) {
                        selectionManager.ConfirmSelections();
                        return;
                    }

                    int indexToBeSelected = GetIndexOf(hitSelection);
                    if (indexToBeSelected >= 0) {
                        if (selectionManager.ConfirmForAction) {
                            if (hitSelection.IsSelected) {
                                selectionManager.Unselect(indexToBeSelected);
                            }
                            else {
                                selectionManager.Select(indexToBeSelected);
                            }
                        }
                        else {
                            selectionManager.Select(indexToBeSelected);
                            selectionManager.ConfirmSelections();
                        }
                    }
                }
                else {
                    // clicked outside - optionally unselect when not multipleSelection
                    if (!selectionManager.MultipleSelection) {
                        //UnselectAll();
                    }
                }
            }

            if (lastMousePos == worldPoint3D) {
                return;
            }
            lastMousePos = worldPoint3D;

            if (hitSelection == null) {
                if (selectionManager.HoveredSelection != null) selectionManager.HoveredSelection.Unhover();
                selectionManager.HoveredSelection = null;
                return;
            }

            if (hitSelection != selectionManager.HoveredSelection) {
                // unhover previous

                if (selectionManager.HoveredSelection != null) selectionManager.HoveredSelection.Unhover();

                selectionManager.HoveredSelection = hitSelection;

                if (selectionManager.HoveredSelection != null) {
                    // update current index if selection exists in list
                    int idx = GetIndexOf(selectionManager.HoveredSelection);
                    if (idx >= 0) selectionManager.CurrentSelectionsIndex = idx;
                    selectionManager.HoveredSelection.Hover();
                }
            }
        }

        private void Awake() {
            cachedMainCamera = Camera.main;

            if (selectionManager == null) {
                selectionManager = GetComponent<SelectionManager>();
            }
        }

        private void Update() {
            if (useKeyboard) {
                HandleKeyboard();
            }

            if (useMouse) {
                HandleMouse();
            }
        }
    }
}
