
using System.Collections.Generic;
using UnityEngine;

public partial class Deck : MonoBehaviour {
    [SerializeField] List<Skill> cards;
    public List<Skill> Cards => cards;
}
