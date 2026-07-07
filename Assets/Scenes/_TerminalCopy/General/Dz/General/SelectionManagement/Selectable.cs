using Dz.SelectionManagement;
using UnityEngine;

public partial class Selectable : Selection {
    static Selectable potentialSelected;
    static Selectable hoveredObject;

    public void Update() {
        Collider collider = GetComponentInChildren<Collider>();
        if (Mouse.HoveredObject == this) {
            if (hoveredObject == this) {

            }
            else if (hoveredObject != null) {
                hoveredObject.Unhover();
                hoveredObject = this;
                hoveredObject.Hover();
            }
            else if (hoveredObject == null) {
                hoveredObject = this;
                hoveredObject.Hover();
            }
        }
        if (Mouse.HoveredObject == null || !(Mouse.HoveredObject.GetComponent<Selectable>() != null)) {
            if (hoveredObject != null) {
                hoveredObject.Unhover();
                hoveredObject = null;
            }
        }

        if (hoveredObject != null) {
            if (Mouse.IsJustPressed(Mouse.Key.Left)) {
                potentialSelected = hoveredObject;
            }
        }

        if (hoveredObject != potentialSelected) {
            potentialSelected = null;
        }
        if (potentialSelected != this) {
            return;
        }
        if (potentialSelected != null) {
            if (Mouse.IsJustReleased(Mouse.Key.Left)) {
                Debug.Log("select");
                potentialSelected.Select();
                potentialSelected = null;
            }
        }
    }
}
