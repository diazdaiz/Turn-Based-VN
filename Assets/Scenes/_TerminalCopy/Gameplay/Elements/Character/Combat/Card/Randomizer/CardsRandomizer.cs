using System.Collections.Generic;

public partial class CardsRandomizer : GeneralObjectsRandomizer {
    public static CardsRandomizer Instance { get; set; }
    Card.CardIdentity identity;
    bool attacks = true;
    bool skills = true;
    bool powers = true;
    bool refreshPool = true;
    List<float> rarityChances = null;
    float upgradeChance = 0;

    public Card Get(Card.CardIdentity identity, bool attacks = true, bool skills = true, bool powers = true, bool refreshPool = true, List<float> rarityChances = null, float upgradeChance = 0) {
        this.identity = identity;
        this.attacks = attacks;
        this.skills = skills;
        this.powers = powers;
        this.refreshPool = refreshPool;
        this.rarityChances = rarityChances;
        this.upgradeChance = upgradeChance;
        if (refreshPool) {
            SetCurrentItemWeight();
        }
        Card card = Instantiate(base.Get() as Card).GetComponent<Card>();
        card.IsUpgraded = Dz.Random.Randomizer.Range(0f, 1f) < upgradeChance;
        return card;
    }

    public override void SetCurrentItemWeight() {
        currentItemsWeights = new();
        for (int i = 0; i < itemsWeights.Count; i++) {
            currentItemsWeights.Add(new());
            //karena cuman 3 rarity
            if (rarityChances != null) {
                currentItemsWeights[i].Weight = rarityChances[i];
            }
            else {
                currentItemsWeights[i].Weight = itemsWeights[i].Weight;
            }
            currentItemsWeights[i].Items = new();
            for (int j = 0; j < itemsWeights[i].Items.Count; j++) {
                Card card = itemsWeights[i].Items[j].GetComponent<Card>();
                if (card.Identity != identity) {
                    continue;
                }
                if (!attacks && card.Type == Card.CardType.Attack) {
                    continue;
                }
                if (!skills && card.Type == Card.CardType.Skill) {
                    continue;
                }
                if (!powers && card.Type == Card.CardType.Power) {
                    continue;
                }

                currentItemsWeights[i].Items.Add(itemsWeights[i].Items[j]);
            }
        }
    }

    public override void Start() {
        Instance = this;
        RefreshPool();
    }
}