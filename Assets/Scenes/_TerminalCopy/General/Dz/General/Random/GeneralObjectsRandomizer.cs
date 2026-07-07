using Dz.Random;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObjectsRandomizer : MonoBehaviour {
    [SerializeField] protected List<ItemsWeight> itemsWeights;
    [SerializeField] protected RandomizerType randomizerType;
    protected enum RandomizerType { Normal, Pool, PoolRefresh }
    // NOTE
    // Normal: ga ngehilangin item dari pool setelah diambil
    // Pool: ngehilangin item setelah diambil
    // Pool Refresh: Item di refresh saat pool sudah habis

    protected List<LocalItemsWeight> currentItemsWeights;

    protected class LocalItemsWeight {
        public float Weight { get; set; }
        public List<GameObject> Items { get; set; }
    }

    // float total = 10000f;
    // float rareCount = 0;
    // float uncommonCount = 0;
    // float commonCount = 0;

    public virtual void SetCurrentItemWeight() {
        currentItemsWeights = new();
        for (int i = 0; i < itemsWeights.Count; i++) {
            currentItemsWeights.Add(new());
            currentItemsWeights[i].Weight = itemsWeights[i].Weight;
            currentItemsWeights[i].Items = new();
            for (int j = 0; j < itemsWeights[i].Items.Count; j++) {
                currentItemsWeights[i].Items.Add(itemsWeights[i].Items[j]);
            }
        }
    }

    public virtual object Get() {
        List<Randomizable> randomizables = new();
        object GetObjectFromItemsWeights(List<LocalItemsWeight> itemsWeights) {
            for (int i = 0; i < itemsWeights.Count; i++) {
                for (int j = 0; j < itemsWeights[i].Items.Count; j++) {
                    randomizables.Add(new(itemsWeights[i].Items[j], itemsWeights[i].Weight / itemsWeights[i].Items.Count));
                }
            }

            object obj = Randomizer.Randomize<GameObject>(randomizables);
            // if ((obj as Node).GetType() == typeof(Akabeko) || (obj as Node).GetType() == typeof(Anchor)) {
            //     rareCount += 1;
            // }
            // else if ((obj as Node).GetType() == typeof(AncientTeaSet) || (obj as Node).GetType() == typeof(ArtOfWar) || (obj as Node).GetType() == typeof(BagOfMarbles)) {
            //     uncommonCount += 1;
            // }
            // else {
            //     commonCount += 1;
            // }
            return obj;
        }

        if (randomizerType == RandomizerType.Normal) {
            SetCurrentItemWeight();
            return GetObjectFromItemsWeights(currentItemsWeights);
        }
        else if (randomizerType == RandomizerType.Pool) {
            //set pool
            Debug.Log("masuk 1");
            if (currentItemsWeights == null) {
                SetCurrentItemWeight();
            }

            //klau pool abis, return null
            int itemsCount = 0;
            for (int i = 0; i < currentItemsWeights.Count; i++) {
                itemsCount += currentItemsWeights[i].Items.Count;
            }
            if (itemsCount == 0) {
                Debug.Log("return disini");
                return null;
            }

            //dapetin itemnya
            GameObject obj = (GameObject)GetObjectFromItemsWeights(currentItemsWeights);

            //remove item dari pool
            for (int i = 0; i < currentItemsWeights.Count; i++) {
                if (currentItemsWeights[i].Items.Contains(obj)) {
                    currentItemsWeights[i].Items.Remove(obj);
                    break;
                }
            }
            Debug.Log("nyame 2");

            //return
            return obj;
        }
        else if (randomizerType == RandomizerType.PoolRefresh) {
            //set pool
            if (currentItemsWeights == null) {
                SetCurrentItemWeight();
            }

            //klau pool abis, set itemnya lagi
            int itemsCount = 0;
            for (int i = 0; i < currentItemsWeights.Count; i++) {
                itemsCount += currentItemsWeights[i].Items.Count;
            }
            if (itemsCount == 0) {
                SetCurrentItemWeight();
            }

            //dapetin itemnya
            GameObject obj = (GameObject)GetObjectFromItemsWeights(currentItemsWeights);

            //remove item dari pool
            for (int i = 0; i < currentItemsWeights.Count; i++) {
                if (currentItemsWeights[i].Items.Contains(obj)) {
                    currentItemsWeights[i].Items.Remove(obj);
                    break;
                }
            }

            //return
            return obj;
        }
        return null;
    }

    public virtual void RefreshPool() {
        SetCurrentItemWeight();
    }

    public virtual void Start() {
        // for (int i = 0; i < 11; i++) {
        //     Node node = (Get() as Node);
        //     if (node != null) {
        //         Debug.Log(node.Name);
        //     }
        //     else {
        //         Debug.Log("null");
        //     }
        // }
        // Debug.Log("===============================");
        // for (int i = 0; i < (int)total; i++) {
        //     Get();
        // }
        // Debug.Log(rareCount / total);
        // Debug.Log(uncommonCount / total);
        // Debug.Log(commonCount / total);
    }
}
