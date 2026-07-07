
using System.Collections.Generic;
using UnityEngine;

public partial class Deck : MonoBehaviour {
    [SerializeField] List<Card> cards;
    public List<Card> Cards => cards;
}
