using System.Collections.Generic;
using UnityEngine;

public partial class HandPlacement : MonoBehaviour {
    // public GameObject slotsContainer;
    // public GameObject cardsContainer;
    List<CardHandSlot> slots;
    //int cardsCount = 0;

    CombatManager combat => CombatManager.Instance;

    private void Start() {
        //slots = new List<CardHandSlot>();
        //for (int i = 0; i < slotsContainer.transform.childCount; i++) {
        //    slots.Add(slotsContainer.transform.GetChild(i).GetComponent<CardHandSlot>());
        //}
    }

    void Update() {
        //if (combat.handCards.Count != cardsCount) {
        //    cardsCount = combat.handCards.Count;
        //    for (int i = 0; i < cardsCount; i++) {
        //        slots[i].Set(i, cardsCount);
        //    }
        //}
        //for (int i = 0; i < combat.handCards.Count; i++) {
        //    Transform transform = combat.handCards[i].transform;
        //    //current
        //    float cx = transform.position.x;
        //    float cy = transform.position.y;
        //    //target
        //    float tx = slots[i].targetPosition.x;
        //    float ty = slots[i].targetPosition.y;
        //    Vector2 vec2 = Vector2.MoveTowards(new Vector2(cx, cy), new Vector2(tx, ty), (new Vector2(tx, ty) - new Vector2(cx, cy)).magnitude * Time.deltaTime * 7f);
        //    transform.position = new Vector3(vec2.x, vec2.y, slots[i].targetPosition.z);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, slots[i].targetRotation, 25f * Time.deltaTime);
        //}
    }
}
