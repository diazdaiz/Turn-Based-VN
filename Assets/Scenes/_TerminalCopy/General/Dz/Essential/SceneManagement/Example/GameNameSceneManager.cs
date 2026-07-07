using System.Collections.Generic;

namespace Dz.SceneManagement.Example {
    public partial class GameNameSceneManager : SceneManager<Scenes> {
        //Untuk sekarang, Instancenya dibuat singleton dulu, nanti ditaruh di GameManager (Dz.Game)
        public static GameNameSceneManager Instance { get; private set; }

        public override Dictionary<Scenes, string> MainScenesPath { get; set; }

        private void Awake() {
            if (Instance == null) {
                Instance = this;
                // DontDestroyOnLoad(this);
                MainScenesPath = new Dictionary<Scenes, string>() {
                    {Scenes.ES1, "Dz/Unity Essential/SceneManagement/Example/ES1/ES1" },
                    {Scenes.ES2, "Dz/Unity Essential/SceneManagement/Example/ES2/ES2" }
                };
            }
            else {
                Destroy(this);
            }
        }
    }

    public enum Scenes {
        ES1,
        ES2
    }
}