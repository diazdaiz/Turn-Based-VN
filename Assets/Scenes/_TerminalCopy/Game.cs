using UnityEngine;

public partial class Game : MonoBehaviour {
    public static Game Instance { get; private set; }

    public static GameDataManager Data { get; private set; }
    public static AudioManager Audio { get; private set; }
    public static GameSceneManager Scene { get; private set; }
    public static Camera Camera;

    [SerializeField] GameDataManager data;
    [SerializeField] new AudioManager audio;
    [SerializeField] GameSceneManager scene;
    [SerializeField] new Camera camera;

    //bool showStats;

    public void Start() {
        Data = data;
        Audio = audio;
        Scene = scene;
        Camera = Camera.main;

        Scene.OpenMainScene(Scenes.Exploration);
        //showStats = true;
    }

    //public void Update() {
    //    if (Keyboard.IsJustPressed(Key.F2)) {
    //        showStats = !showStats;
    //    }
    //    if (showStats) {
    //        ShowStats((float)delta);
    //    }
    //}

    //public void ShowStats(float delta) {
    //    var _time = Time.GetTicksMsec() / 1000.0f;
    //    var box_pos = new Vector3(0, Mathf.Sin(_time * 4f), 0);
    //    var line_begin = new Vector3(-1, Mathf.Sin(_time * 4f), 0);
    //    var line_end = new Vector3(1, Mathf.Cos(_time * 4f), 0);

    //    // DebugDraw3D.DrawBox(box_pos, Quaternion.Identity, new Vector3(1, 2, 1), new Color(0, 1, 0));
    //    // DebugDraw3D.DrawLine(line_begin, line_end, new Color(1, 1, 0));
    //    //DebugDraw2D.SetText("Info", " press f2 to hide/show", priority: -1);
    //    //DebugDraw2D.SetText("Time", _time.ToString());
    //    //DebugDraw2D.SetText("Frames drawn", Engine.GetFramesDrawn().ToString());
    //    //DebugDraw2D.SetText("FPS", Engine.GetFramesPerSecond().ToString());
    //    //DebugDraw2D.SetText("delta", delta.ToString());
    //}
}
