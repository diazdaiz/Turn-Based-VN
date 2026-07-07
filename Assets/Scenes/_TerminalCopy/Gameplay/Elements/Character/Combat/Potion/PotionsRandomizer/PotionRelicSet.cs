

using System.Collections.Generic;
using UnityEngine;

public partial class PotionRelicSet : MonoBehaviour {
    public Potion.PotionRarity Rarity => rarity;
    public float Weight => weight;
    public List<Potion> Potions => relics;

    [SerializeField] Potion.PotionRarity rarity;
    [SerializeField] float weight;
    [SerializeField] List<Potion> relics;
}