using System.Collections;
using UnityEngine;

public class CharacterVRoidModelAnimationManager : MonoBehaviour {
    [SerializeField] CharacterMovement movement;
    [SerializeField] Animator animator;
    Transform modelTransform;

    enum MovementState { Idle, Walk, Run }
    MovementState state;

    Vector3 initialLocalPosition;
    Quaternion initialLocalRotation;

    private void Awake() {
        modelTransform = transform.GetChild(0);
    }

    private void Start() {
        initialLocalPosition = modelTransform.localPosition;
        initialLocalRotation = modelTransform.localRotation;
    }

    void Update() {
        if (movement.Speed == 0 && state != MovementState.Idle) {
            state = MovementState.Idle;
            animator.CrossFade("Standard Idle", 0.15f);
            StartCoroutine(FixModelTransform());
        }
        if (movement.Speed == movement.WalkSpeed && state != MovementState.Walk) {
            state = MovementState.Walk;
            animator.CrossFade("Standard Walk", 0.25f);
            StartCoroutine(FixModelTransform());

        }
        if (movement.Speed == movement.RunSpeed && state != MovementState.Run) {
            state = MovementState.Run;
            animator.CrossFade("Standard Run", 0.25f);
            StartCoroutine(FixModelTransform(true));
        }
        float targetAngle = Mathf.Atan2(movement.Direction.y, movement.Direction.x) * 180 / Mathf.PI;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(0f, targetAngle, 0f)), 180f / 0.6f * Time.deltaTime);


    }

    //ada bug dimana saat ganti state, model menjauh dari initial local position & rotation jika apply root motion diaktifkan,
    //makanya tiap ganti state coba dibalikin ke initial transformnya
    IEnumerator FixModelTransform(bool run = false) {
        float t = 0f;
        while (t < 0.2) {
            t += Time.deltaTime;
            modelTransform.localPosition = Vector3.MoveTowards(modelTransform.localPosition, initialLocalPosition, 3f * Time.deltaTime);
            modelTransform.localRotation = Quaternion.RotateTowards(modelTransform.localRotation, Quaternion.Euler(0, run ? -13f : 0, 0f), 90f * Time.deltaTime);
            yield return null;
        }
    }
}
