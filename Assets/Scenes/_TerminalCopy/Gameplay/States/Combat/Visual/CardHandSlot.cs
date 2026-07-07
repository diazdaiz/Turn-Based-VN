using UnityEngine;

public partial class CardHandSlot : MonoBehaviour {
    GameObject rotationObject;
    GameObject positionObject;
    // public Quaternion targetRotation => rotationObject.rotation;
    // public Vector3 targetPosition => positionObject.position;

    public void Set(int index, int handCardsCount) {
        // if (handCardsCount <= 0) {
        //     rotationObject.eulerAngles = new Vector3(0, 0, 0);
        //     return;
        // }
        // //9 => 12 sampai -12; 0 => 0 sampai 0
        // rotationObject.transform.eulerAngles = new Vector3(0, 0, ((9f - index) / 9f) * 12f + (index / 9f) * (-12f));
        // positionObject.position = new Vector3(positionObject.position.x, positionObject.position.y, -0.5f * index);
    }

    private void Awake() {
        rotationObject = transform.GetChild(0).gameObject;
        positionObject = transform.GetChild(0).GetChild(0).gameObject;
    }

    void Update() {

    }
}
