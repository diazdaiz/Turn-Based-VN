using System.Collections.Generic;
using UnityEngine;

public partial class ItemsWeight : MonoBehaviour {
    public float Weight {
        get {
            return weight;
        }
        set {
            weight = value;
        }
    }
    public List<GameObject> Items {
        get {
            return items;
        }
        set {
            items = value;
        }
    }

    [SerializeField] float weight;
    [SerializeField] List<GameObject> items;
}
