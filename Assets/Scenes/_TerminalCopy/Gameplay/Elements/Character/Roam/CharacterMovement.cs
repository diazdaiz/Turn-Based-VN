using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    [SerializeField] float walkSpeed = 3f;
    [SerializeField] float runSpeed = 5.5f;
    Rigidbody rb;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody>();
    }

    public void Move(Vector2 dir, bool running) {
        if (dir.magnitude == 0) {
            return;
        }
        // z depan belakang, x kiri kanan
        rb.AddForce(Physics.gravity * 3);
        rb.linearVelocity = new Vector3(dir.y, 0, dir.x).normalized * (running ? runSpeed : walkSpeed) + new Vector3(0f, rb.linearVelocity.y, 0f);
    }
}
