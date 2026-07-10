using System.Collections;
using UnityEngine;

public class CharacterVRoidModelManager : MonoBehaviour {
    [SerializeField] CharacterMovement movement;
    [SerializeField] CharacterCombat combat;
    [SerializeField] Animator animator;
    Transform modelTransform;

    enum MovementState { Idle, Walk, Run }
    MovementState movementState;

    Vector3 initialLocalPosition;
    Quaternion initialLocalRotation;
    bool inCombat = false;

    private void Awake() {
        modelTransform = transform.GetChild(0);
    }

    private void Start() {
        initialLocalPosition = modelTransform.localPosition;
        initialLocalRotation = modelTransform.localRotation;


        //tiap turn mulainya beda2, jadi engga disini dibuat combat idlenya
        //combat.OnTurnStart += CombatIdle;
        combat.OnAttack += Punch;
        combat.OnTakeDamage += Damaged;
        combat.OnDeath += Death;
    }

    void Update() {
        if (CombatManager.Instance.IsCombating) {
            CombatUpdate();
        }
        else {
            MovementUpdate();
        }
    }

    //ini codingan temporary, harusnya character ngeliat state gameplaynya, dan jadinya berdasarkan itu, tidak cukup waktu untuk nulis ini
    void CombatUpdate() {
        if (CombatManager.Instance.IsCombating && !inCombat) {
            inCombat = true;
            animator.CrossFade("Combat Idle", 0.15f);
        }
    }

    void CombatIdle(CombatAction combatAction) {
        //animator.CrossFade("Standard Idle", 0.15f);
    }

    void Punch(CombatAction combatAction) {
        animator.CrossFade("Cross Punch", 0.15f);

    }

    void Death(CombatAction combatAction) {
        StartCoroutine(DeathAfter(0.8f));
    }

    IEnumerator DeathAfter(float seconds) {
        float timer = 0f;
        while (timer < seconds) {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.CrossFade("Death", 0.15f);

    }

    void Damaged(CombatAction combatAction) {
        StartCoroutine(DamagedAfter(0.6f));

    }

    IEnumerator DamagedAfter(float seconds) {
        float timer = 0f;
        while (timer < seconds) {
            timer += Time.deltaTime;
            yield return null;
        }
        animator.CrossFade("Taking Punch", 0.15f);

    }

    void MovementUpdate() {
        if (movement.Speed == 0 && movementState != MovementState.Idle) {
            movementState = MovementState.Idle;
            animator.CrossFade("Standard Idle", 0.15f);
            StartCoroutine(FixModelTransform());
        }
        if (movement.Speed == movement.WalkSpeed && movementState != MovementState.Walk) {
            movementState = MovementState.Walk;
            animator.CrossFade("Standard Walk", 0.25f);
            StartCoroutine(FixModelTransform());

        }
        if (movement.Speed == movement.RunSpeed && movementState != MovementState.Run) {
            movementState = MovementState.Run;
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
