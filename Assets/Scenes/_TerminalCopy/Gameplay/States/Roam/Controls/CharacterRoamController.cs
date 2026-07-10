using UnityEngine;

public class CharacterRoamController : MonoBehaviour {
    public Character Character {
        get {
            return character;
        }
        set {
            if (value.gameObject.activeInHierarchy) {
                character = value;
            }
            else {
                Character newCharacter = Instantiate(character);
                newCharacter.gameObject.SetActive(true);
                character = newCharacter;
            }
        }
    }
    float MouseSensitivity => mouseSensitivity * mouseSensitivityDefaultMultiplier;
    CharacterMovement Movement => character.Movement;

    [SerializeField] Character character;
    [SerializeField] Transform TPVCameraHolder; //ThirdPersonViewCameraHolder
    [SerializeField] Transform TPVCameraAxis; //ThirdPersonViewCameraAxis
    [SerializeField] float mouseSensitivity = 1f;

    float mouseSensitivityDefaultMultiplier = 0.22f;
    Camera TPVCamera; //ThirdPersonViewCamera
    bool cursorLockAndHide = true;

    private void Awake() {
        TPVCameraAxis = TPVCameraHolder.GetChild(0).transform;
        TPVCamera = TPVCameraHolder.GetChild(0).GetChild(0).GetComponent<Camera>();
    }

    private void Update() {
        MovementUpdate();
    }

    //codingan sementara untuk mempercepat langsung ke dialogue & fight
    public void SetCamera(float angle) {
        TPVCameraHolder.position = character.transform.position;
        TPVCameraAxis.transform.rotation = Quaternion.Euler(TPVCameraAxis.transform.eulerAngles.x, angle, TPVCameraAxis.transform.eulerAngles.z);
    }

    void MovementUpdate() {
        //Note: awalnya mau bisa di-disable, tapi engga perlu deng
        //if (Input.GetKeyDown(KeyCode.Escape)) {
        //    cursorLockAndHide = !cursorLockAndHide;
        //}
        Cursor.lockState = cursorLockAndHide ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = cursorLockAndHide;
        if (cursorLockAndHide) {
            TPVCameraHolder.position = character.transform.position;
            TPVCameraHolder.rotation = Quaternion.identity;
            TPVCameraAxis.transform.rotation = Quaternion.Euler(TPVCameraAxis.transform.rotation.eulerAngles.x + Input.mousePositionDelta.y * MouseSensitivity, TPVCameraAxis.transform.rotation.eulerAngles.y + Input.mousePositionDelta.x * MouseSensitivity, 0f);
        }
        //z ==jadi==> depan belakang (x), x ==jadi==> kiri kanan (y)
        Vector2 CameraToCharacterdir = new Vector2(character.transform.position.z, character.transform.position.x) - new Vector2(TPVCamera.transform.position.z, TPVCamera.transform.position.x);
        Vector2 dir = new(0, 0);
        dir += Input.GetKey(KeyCode.W) ? RotatedDir(CameraToCharacterdir, 0f) : Vector2.zero;
        dir += Input.GetKey(KeyCode.A) ? RotatedDir(CameraToCharacterdir, -90 * Mathf.PI / 180) : Vector2.zero;
        dir += Input.GetKey(KeyCode.S) ? RotatedDir(CameraToCharacterdir, -180 * Mathf.PI / 180) : Vector2.zero;
        dir += Input.GetKey(KeyCode.D) ? RotatedDir(CameraToCharacterdir, -270 * Mathf.PI / 180) : Vector2.zero;
        Movement.Move(dir, Input.GetKey(KeyCode.LeftShift));
    }

    void InteractionUpdate() {
        //
    }

    Vector2 RotatedDir(Vector2 dir, float tetha) {
        return new Vector2(Mathf.Cos(tetha) * dir.x - Mathf.Sin(tetha) * dir.y, Mathf.Sin(tetha) * dir.x + Mathf.Cos(tetha) * dir.y);
    }
}
