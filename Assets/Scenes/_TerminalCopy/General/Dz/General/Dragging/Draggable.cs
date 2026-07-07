using UnityEngine;

public partial class Draggable : MonoBehaviour {
    public bool Dragged => draggedObject == this;

    [SerializeField] GameObject targetObjectToBeDrag;
    [SerializeField] bool lockX;
    [SerializeField] bool lockY;
    [SerializeField] bool lockZ;
    [SerializeField] bool useAcceleration = true;
    [SerializeField] float aScale = 10f;

    Draggable draggedObject;
    Vector3 dragObjectPosWhenBeginDragging;
    Vector3 mousePosWhenBeginDragging;
    Vector3 a;
    Vector3 v;
    Vector3 dp;

    public void Start() {
        a = new();
        v = new();
        dp = new();
    }

    public void Update() {
        if (Mouse.IsJustPressed(Mouse.Key.Left)) {
            if (Mouse.HoveredObject == this) {
                draggedObject = this;
                dragObjectPosWhenBeginDragging = transform.position;
                mousePosWhenBeginDragging = Mouse.Pos;
            }
        }

        Vector3 targetPos = targetObjectToBeDrag.transform.position;
        if (draggedObject != null) {
            //posisi mapnya
            float translateX = lockX ? 0 : Mouse.Pos.x - mousePosWhenBeginDragging.x;
            float translateY = lockY ? 0 : Mouse.Pos.y - mousePosWhenBeginDragging.y;
            float translateZ = lockZ ? 0 : Mouse.Pos.z - mousePosWhenBeginDragging.z;

            targetPos = dragObjectPosWhenBeginDragging + new Vector3(translateX, translateY, translateZ);
            // draggedObject.GlobalPosition = targetPos;

            if (Mouse.IsJustReleased(Mouse.Key.Left)) {
                draggedObject = null;
            }
            v = 15 * (targetPos - targetObjectToBeDrag.transform.position);
        }

        float dt = Time.deltaTime;
        if (useAcceleration) {
            a = (Vector3.zero - v) * aScale;
            v = v + a * dt;
            dp = new Vector3(lockX ? 0f : v.x, lockY ? 0f : v.y, lockZ ? 0f : v.z) * dt;
            targetObjectToBeDrag.transform.position += dp;
        }
        else {
            targetObjectToBeDrag.transform.position = targetPos;
        }
    }
}
