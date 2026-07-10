using UnityEngine;

public class CharacterMovement : MonoBehaviour {
    public Vector2 Direction { get; private set; }
    public float Speed { get; private set; }
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    [SerializeField] float walkSpeed = 2.4f;
    [SerializeField] float runSpeed = 5.5f;
    Rigidbody rb;

    private void Awake() {
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Start() {
        float angle = transform.parent.eulerAngles.y;
        Direction = new Vector2(Mathf.Cos(Mathf.PI * angle / 180f), Mathf.Sin(Mathf.PI * angle / 180f));
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
