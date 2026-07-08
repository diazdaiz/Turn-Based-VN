using System.Collections.Generic;

public partial class SkillsRandomizer : GeneralObjectsRandomizer {
    public static SkillsRandomizer Instance { get; set; }
    bool attacks = true;
    bool skills = true;
    bool powers = true;
    bool refreshPool = true;
    List<float> rarityChances = null;

    public Skill Get(bool attacks = true, bool skills = true, bool powers = true, bool refreshPool = true, List<float> rarityChances = null) {
        this.attacks = attacks;
        this.skills = skills;
        this.powers = powers;
        this.refreshPool = refreshPool;
        this.rarityChances = rarityChances;
        if (refreshPool) {
            SetCurrentItemWeight();
        }
        Skill skill = Instantiate(base.Get() as Skill).GetComponent<Skill>();
        return skill;
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
                Skill skill = itemsWeights[i].Items[j].GetComponent<Skill>();
                if (!attacks && skill.Type == Skill.SkillType.Attack) {
                    continue;
                }
                if (!skills && skill.Type == Skill.SkillType.Passive) {
                    continue;
                }
                if (!powers && skill.Type == Skill.SkillType.NonAttack) {
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