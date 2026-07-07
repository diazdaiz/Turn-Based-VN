using System.Collections.Generic;
using UnityEngine;

public partial class RelicsContainer : MonoBehaviour {
    List<Relic> Relics => GameplayManager.Instance.Combat.Relics;

    public void Update() {
        for (int i = 0; i < Relics.Count; i++) {
            //Relics[i].game = true;
            //Relics[i].Position = Vector3.Right * 0.8f * i;
        }
    }
}
