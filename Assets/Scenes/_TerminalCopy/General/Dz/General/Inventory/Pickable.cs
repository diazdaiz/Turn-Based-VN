
using UnityEngine;

public partial class Pickable : MonoBehaviour {
    //void DragMapUpdate(float dt) {
    //    Vector2 mPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

    //    foreach (Room room in map.rooms.Values) {
    //        if (room.isHovered) {
    //            return;
    //        }
    //    }

    //    if (Mouse.current.press.wasPressedThisFrame && map.dragArea.OverlapPoint(mPos)) {
    //        isDraggingMap = true;
    //        mapPosWhenBeginDragging = map.transform.position;
    //        mousePosWhenBeginDragging = mPos;
    //    }

    //    if (isDraggingMap) {
    //        //posisi mapnya
    //        Vector3 targetPos = mapPosWhenBeginDragging + new Vector2(0, mPos.y - mousePosWhenBeginDragging.y);
    //        //map.transform.position = targetPos;
    //        velocity = 15 * (targetPos - map.transform.position);

    //        if (Mouse.current.press.wasReleasedThisFrame) {
    //            isDraggingMap = false;
    //        }
    //    }
    //    velocity = Vector3.MoveTowards(velocity, Vector3.zero, (velocity - Vector3.zero).magnitude * 10f * dt);
    //    map.transform.position = map.transform.position + velocity * dt;
    //}
}
