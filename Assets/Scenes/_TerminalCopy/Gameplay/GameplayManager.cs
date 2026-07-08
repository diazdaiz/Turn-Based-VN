using UnityEngine;

public partial class GameplayManager : MonoBehaviour {
    public static GameplayManager Instance { get; private set; }

    public void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }
}
