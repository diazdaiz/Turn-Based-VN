using UnityEngine;

[DefaultExecutionOrder(-1)]
public partial class Game : MonoBehaviour {
    public static GameDataManager Data { get; private set; }
    public static AudioManager Audio { get; private set; }
    public static GameSceneManager Scene { get; private set; }

    private void Awake() {
        Data = GetComponentInChildren<GameDataManager>();
        Audio = GetComponent<AudioManager>();
        Scene = GetComponent<GameSceneManager>();
        Display.displays[1].Activate();
    }
}
