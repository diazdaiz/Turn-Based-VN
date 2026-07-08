using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public Vector2 Direction { get; private set; } = new Vector2(-1f, 0f);
    public float Speed { get; private set; }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    [SerializeField] float walkSpeed = 2.4f;
    [SerializeField] float runSpeed = 5.5f;
    Rigidbody rb;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody>();
    }

    public void Move(Vector2 dir, bool running) {
        if (dir.magnitude == 0) {
            Speed = 0f;
            return;
        }
        // x depan belakang, y kiri kanan
        Direction = dir.normalized;
        Speed = (running ? runSpeed : walkSpeed);
        rb.AddForce(Physics.gravity * 3f);
        rb.linearVelocity = new Vector3(dir.y, 0, dir.x).normalized * Speed + new Vector3(0f, rb.linearVelocity.y, 0f);
    }
}
