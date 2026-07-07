using UnityEngine;

namespace Dz.DataManagement.Example {
    public partial class GameDataManager : MonoBehaviour {
        // public GameData GameData => dataManager.Data as GameData;
        // int number {
        //     get {
        //         return GameData.progressData.number;
        //     }
        //     set {
        //         GameData.progressData.number = value;
        //     }
        // }

        // [SerializeField] TMPro.TextMeshProUGUI textMeshPro;

        // DataManager<GameData> dataManager;
        // GameData gameData;

        // private void Awake() {
        //     gameData = new GameData();
        //     dataManager = new(gameData);
        //     //Load();
        // }

        // public void Save() {
        //     dataManager.Save();
        // }

        // public void Load() {
        //     dataManager.Load();
        // }

        // private void Update() {
        //     if (Input.GetKeyDown(KeyCode.S)) {
        //         Save();
        //     }
        //     if (Input.GetKeyDown(KeyCode.L)) {
        //         Load();
        //     }
        //     if (Input.GetKeyDown(KeyCode.UpArrow)) {
        //         number += 1;
        //     }
        //     if (Input.GetKeyDown(KeyCode.DownArrow)) {
        //         number -= 1;
        //     }
        //     textMeshPro.text = number.ToString();
        // }
    }
}