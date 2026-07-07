using Dz.SceneManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class GameSceneManager : SceneManager<Scenes> {
    public override Dictionary<Scenes, string> MainScenesPath { get; set; }
    public override Dictionary<Scenes, Type> MainScenesType { get; set; }

    //Main Menu

    //Exploration
    public Dictionary<int, List<Subscene>> ExplorationSubscenesAndPriority { get; set; }
    Dictionary<int, Subscene> explorationOpenedSubsceneAndPriority;
    //^note: ingin MainEvents sebagai prioritas 0 dan tuker2an open/closed dengan yang prioritas 0/MainEvents juga
    //       prioritas lainnya juga sama, prior 1 tukeran sama prior 1, 2 sama 2, dst
    //       tambahannya, harus selalu ada yang fokus berdasarkan prior. misal prior 4 ditutup, cek dari 3 -> 0, kalau ada prior yang kebuka, fokuskan
    //IMPROVE - nanti tambahannya mungkin scene diurutin mana yang kebuka duluan (kyk dulu pernah), tapi ujung2nya berdasarkan kebutuhan sih

    public override void Start() {
        base.Start();
        MainScenesPath = new Dictionary<Scenes, string>() {
            { Scenes.Exploration, "res://Assets/Game/Exploration/exploration.tscn" },
            { Scenes.MainMenu, "res://Assets/Game/MainMenu/MainMenu.tscn" },
        };
        MainScenesType = new Dictionary<Scenes, Type>() {
            { Scenes.Exploration, typeof(GameplayManager) },
            { Scenes.MainMenu, typeof(GameObject) },
        };
        explorationOpenedSubsceneAndPriority = new() {
            {0, null},
            {1, null},
            {2, null},
            {3, null},
            {4, null},
            {5, null}
        };
    }

    public override void OpenSubscene(Subscene subscene, bool focusOpenedSubscene = true, bool UnfocusOthersSubscene = true) {
        base.OpenSubscene(subscene, focusOpenedSubscene, UnfocusOthersSubscene);

        //cek masuk priority mana
        int priority = -1;
        foreach (int p in ExplorationSubscenesAndPriority.Keys) {
            if (ExplorationSubscenesAndPriority[p].Contains(subscene)) {
                priority = p;
            }
        }
        if (priority == -1) {
            Debug.Log($"{subscene.transform.parent.name} Gak masuk priority manapun (open subscene)");
            return;
        }
        if (priority == 0) {
            //Top UI & HUD, keep opened, keep focused
            return;
        }

        //kalau priority 1, tutup semua yang diatas 1, else(>1), tutup yang sama prioritynya
        if (priority == 1) {
            for (int i = 5; i > 1; i--) {
                if (explorationOpenedSubsceneAndPriority[i] != null) {
                    CloseSubscene(explorationOpenedSubsceneAndPriority[i]);
                }
            }
            explorationOpenedSubsceneAndPriority[1] = subscene;
        }
        else {
            if (explorationOpenedSubsceneAndPriority[priority] != null) {
                CloseSubscene(explorationOpenedSubsceneAndPriority[priority]);
            }
            explorationOpenedSubsceneAndPriority[1] = subscene;
        }

        //buka, unfocus 
        Debug.Log($"open subscene {subscene.Scene.name}");
        base.OpenSubscene(subscene);
    }

    public override void CloseSubscene(Subscene subscene) {
        base.CloseSubscene(subscene);
        //pas close, fokus ke subscene lain yang kebuka berdasarkan prioritas 5 -> 0
        //cek masuk priority mana
        for (int i = 5; i >= 0; i--) {
            if (explorationOpenedSubsceneAndPriority[i] != null) {
                Focus(explorationOpenedSubsceneAndPriority[i]);
                return;
            }
        }
    }
}

//main scenes
public enum Scenes {
    Exploration,
    MainMenu
}
