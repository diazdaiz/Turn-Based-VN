using System.Collections.Generic;
using UnityEngine;

public partial class Mouse : InputDevice {
    public static GameObject HoveredObject {
        get {
            if (DraggedObject != null) {
                return DraggedObject;
            }
            return GetGameObjectFromRay();
        }
    }
    public static GameObject DraggedObject { get; set; }
    public static Vector3 Pos {
        get {
            return GetRayHitPosition();
        }
    }

    public enum Key { Left, Middle, Right, WheelDown, WheelUp }
    static Dictionary<Key, Button> Buttons {
        get {
            if (buttons == null) {
                buttons = new() {
                    {Key.Left, new(KeyCode.Mouse0) },
                    {Key.Middle, new(KeyCode.Mouse2) },
                    {Key.Right, new(KeyCode.Mouse1) },
                    {Key.WheelUp, new(KeyCode.WheelUp) },
                    {Key.WheelDown, new(KeyCode.WheelDown) },
                };
            }
            return buttons;
        }
    }

    static Mouse instance;
    static Dictionary<Key, Button> buttons;

    public static bool IsJustPressed(Key key) {
        return Buttons[key].IsJustPressed;
    }

    public static bool IsPressed(Key key) {
        return Buttons[key].IsPressed;
    }

    public static bool IsJustReleased(Key key) {
        return Buttons[key].IsJustReleased;
    }

    public static GameObject GetGameObjectFromRay(string tag = "", LayerMask layerMask = default) {
        List<GameObject> targets = GetGameObjectsFromRay(tag, layerMask);
        return targets.Count > 0 ? targets[0] : null;
    }

    public static List<GameObject> GetGameObjectsFromRay(string tag = "", LayerMask layerMask = default) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit[] hits = layerMask == default
            ? Physics.RaycastAll(ray)
            : Physics.RaycastAll(ray, Mathf.Infinity, layerMask);

        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        List<GameObject> targets = new();

        foreach (RaycastHit hit in hits) {
            GameObject obj = hit.collider.gameObject;

            if (!string.IsNullOrEmpty(tag) && !obj.CompareTag(tag))
                continue;

            targets.Add(obj);
        }

        return targets;
    }

    public static Vector3 GetRayHitPosition(float maxDistance = Mathf.Infinity, LayerMask layerMask = default) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if ((int)layerMask == 0) {
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
                return hit.point;
        }
        else {
            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, layerMask))
                return hit.point;
        }

        return ray.origin + ray.direction * maxDistance;
    }

    public static Vector2 GetScreenPosition() {
        return Input.mousePosition;
    }
}