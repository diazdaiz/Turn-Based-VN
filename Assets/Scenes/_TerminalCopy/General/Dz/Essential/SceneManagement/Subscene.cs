using UnityEngine;

namespace Dz.SceneManagement {
    //[GlobalClass]
    public partial class Subscene : MonoBehaviour {
        public GameObject Scene {
            get {
                if (scene == null) {
                    scene = transform.parent.gameObject;
                }
                return scene;
            }
        }
        public GameObject Control {
            get {
                if (control == null) {
                    control = transform.GetChild(0).gameObject;
                }
                return control;
            }
        }
        public bool IsOpened {
            get {
                return isOpened;
            }
            set {
                if (keepOpened) {
                    isOpened = true;
                    return;
                }
                isOpened = value;
            }
        }
        public bool KeepOpened {
            get {
                return keepOpened;
            }
            set {
                keepOpened = value;
            }
        }
        public bool IsFocused {
            get {
                return isFocused;
            }
            set {
                if (keepFocused) {
                    isFocused = true;
                    return;
                }
                isFocused = value;
            }
        }
        public bool KeepFocused {
            get {
                return keepFocused;
            }
            set {
                keepFocused = value;
            }
        }

        [SerializeField] bool isOpened = false;
        [SerializeField] bool keepOpened = false;
        [SerializeField] bool isFocused = false;
        [SerializeField] bool keepFocused = false;

        GameObject scene;
        GameObject control;

        public void Awake() {
            if (scene == null) {
                scene = transform.parent.gameObject;
            }
            if (control == null) {
                control = transform.GetChild(0).gameObject;
            }
        }

        public void Update() {
            if (IsFocused! && !Control.activeInHierarchy) {
                Control.SetActive(true);
            }
            if (!IsFocused && Control.activeInHierarchy) {
                Control.SetActive(false);
            }

            if (Scene is GameObject gameObject) {
                if (IsOpened && !gameObject.activeInHierarchy) {
                    gameObject.SetActive(true);
                }
                if (!IsOpened && gameObject.activeInHierarchy) {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}