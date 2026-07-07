using UnityEngine;

namespace Dz.Random.Example {
    public partial class ItemRarityRandomizer : MonoBehaviour {
        // [SerializeField] SpriteRenderer showItemRandomResult; //dapetnya yg mana
        // [SerializeField] List<Item> legendaryItems;
        // [SerializeField] List<Item> rareItems;
        // [SerializeField] List<Item> commonItems;
        // List<Randomizable> randomizables;
        // float legendaryChance;
        // float rareChance;
        // float commonChance;
        // int legendaryCount;
        // int rareCount;
        // int commonCount;

        // void UpdateChance() {
        //     int baseAdder = Randomizer.Range(5, 10);
        //     Debug.Log("base adder: " + baseAdder.ToString());

        //     legendaryChance = baseAdder + Randomizer.Range(2f, 3f);
        //     rareChance = baseAdder * 3 + Randomizer.Range(3f, 10f);
        //     commonChance = 100f - (rareChance + legendaryChance);

        //     Debug.Log("legendary chance: " + legendaryChance.ToString());
        //     Debug.Log("rare chance: " + rareChance.ToString());
        //     Debug.Log("common chance: " + commonChance.ToString());

        //     randomizables = new List<Randomizable>() {
        //         new(legendaryItems, legendaryChance),
        //         new(rareItems, rareChance),
        //         new(commonItems, commonChance)
        //     };
        // }

        // private void Start() {
        //     UpdateChance();
        // }

        // private void Update() {
        //     if (Input.GetKeyDown(KeyCode.U)) {
        //         UpdateChance();
        //     }
        //     if (Input.GetKeyDown(KeyCode.R)) {
        //         Item randomItemResult = Randomizer.Randomize<Item>(Randomizer.Randomize<List<Item>>(randomizables));
        //         for (int i = 0; i < legendaryItems.Count; i++) {
        //             if (randomItemResult == legendaryItems[i]) {
        //                 showItemRandomResult.sprite = legendaryItems[i].Sprite;
        //                 showItemRandomResult.color = legendaryItems[i].GetComponent<SpriteRenderer>().color;
        //             }
        //         }
        //         for (int i = 0; i < rareItems.Count; i++) {
        //             if (randomItemResult == rareItems[i]) {
        //                 showItemRandomResult.sprite = rareItems[i].Sprite;
        //                 showItemRandomResult.color = rareItems[i].GetComponent<SpriteRenderer>().color;

        //             }
        //         }
        //         for (int i = 0; i < commonItems.Count; i++) {
        //             if (randomItemResult == commonItems[i]) {
        //                 showItemRandomResult.sprite = commonItems[i].Sprite;
        //                 showItemRandomResult.color = commonItems[i].GetComponent<SpriteRenderer>().color;
        //             }
        //         }
        //         Debug.Log(randomItemResult.name);
        //     }
        //     if (Input.GetKeyDown(KeyCode.T)) {
        //         //10000 kali
        //         legendaryCount = 0;
        //         rareCount = 0;
        //         commonCount = 0;
        //         for (int i = 0; i < 10000; i++) {
        //             Item randomItemResult = Randomizer.Randomize<Item>(Randomizer.Randomize<List<Item>>(randomizables));

        //             if (legendaryItems.Contains(randomItemResult)) {
        //                 legendaryCount += 1;
        //             }
        //             if (rareItems.Contains(randomItemResult)) {
        //                 rareCount += 1;
        //             }
        //             if (commonItems.Contains(randomItemResult)) {
        //                 commonCount += 1;
        //             }
        //         }

        //         Debug.Log("legendary count: " + legendaryCount.ToString());
        //         Debug.Log("rare count: " + rareCount.ToString());
        //         Debug.Log("common count: " + commonCount.ToString());
        //     }
        // }
    }
}