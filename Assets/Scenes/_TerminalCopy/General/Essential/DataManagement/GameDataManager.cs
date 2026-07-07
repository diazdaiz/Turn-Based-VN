using Dz.DataManagement;
using UnityEngine;

public partial class GameDataManager : MonoBehaviour {
    public GameData GameData => dataManager.Data as GameData;

    DataManager<GameData> dataManager;
    GameData gameData;

    private void Awake() {
        gameData = new GameData();
        dataManager = new(gameData);
    }

    public void Save() {
        dataManager.Save();
    }

    public void Load() {
        dataManager.Load();
    }
}
