using UnityEngine;

public class DontDestroyCustomObject : MonoBehaviour {
    [SerializeField] GameObject customObject;

    private void Awake() {
        DontDestroyOnLoad(customObject);
    }

    private void Update() {
        if (!customObject.activeInHierarchy) {
            customObject.SetActive(true);
        }
    }
}
