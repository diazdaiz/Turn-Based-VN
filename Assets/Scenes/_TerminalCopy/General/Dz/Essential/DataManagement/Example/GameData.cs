using System.Collections.Generic;

namespace Dz.DataManagement.Example {
    [System.Serializable]
    public class GameData : Dz.DataManagement.Data {
        public GameData() {
            progressDatas = new();
            lastUsedProgressDatasIndex = 0;
            progressData = new();
            settingData = new();
        }

        public List<ProgressData> progressDatas; //bila ingin ada beberapa slot
        public int lastUsedProgressDatasIndex; //misal untuk continue
                                               //or
        public ProgressData progressData; // kalau cukup hanya 1 progress data

        public SettingData settingData;
    }
}