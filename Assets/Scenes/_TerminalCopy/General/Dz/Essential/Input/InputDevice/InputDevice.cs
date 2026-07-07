using System;
using System.Collections.Generic;
using UnityEngine;

public class InputDevice : MonoBehaviour {
    static Dictionary<KeyCode, Button> Buttons {
        get {
            if (buttons == null) {
                buttons = new();
                foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) {
                    buttons.Add(key, new(key));
                }
            }
            return buttons;
        }
    }

    static Dictionary<KeyCode, Button> buttons;

    public static bool IsJustPressed(KeyCode key) {
        return Buttons[key].IsJustPressed;
    }

    public static bool IsPressed(KeyCode key) {
        return Buttons[key].IsPressed;
    }

    public static bool IsJustReleased(KeyCode key) {
        return Buttons[key].IsJustReleased;
    }

    public virtual void Update() {
        foreach (Button button in Buttons.Values) {
            button.Update();
        }
    }
}
