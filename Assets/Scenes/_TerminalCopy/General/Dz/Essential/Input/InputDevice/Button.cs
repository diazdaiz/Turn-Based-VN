using UnityEngine;

class Button {
    bool lastPressedUpdate = false;
    public bool IsPressedLastFrame { get; set; } = false;
    public bool IsJustPressed { get; set; } = false;
    public bool IsPressed { get; set; } = false;
    public bool IsJustReleased { get; set; } = false;

    KeyCode keboardButton;

    public Button(KeyCode keboardButton) {
        this.keboardButton = keboardButton;
    }

    public void Update() {
        IsPressedLastFrame = lastPressedUpdate;

        bool isPressedThisFrame = Input.GetKey(keboardButton);
        IsPressed = isPressedThisFrame;

        if (!IsPressedLastFrame && isPressedThisFrame) {
            IsJustPressed = true;
        }
        else {
            IsJustPressed = false;
        }

        if (IsPressedLastFrame && !isPressedThisFrame) {
            IsJustReleased = true;
        }
        else {
            IsJustReleased = false;
        }

        lastPressedUpdate = isPressedThisFrame;
    }
}