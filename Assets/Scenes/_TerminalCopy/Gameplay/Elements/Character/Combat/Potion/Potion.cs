using System.Collections.Generic;
using UnityEngine;

public partial class Potion : MonoBehaviour {
    public CombatManager Combat => CombatManager.Instance;
    public PotionRarity Rarity => rarity;
    public PotionActivation Activation => activation;
    public PotionTarget Target => target;
    public bool CanBeActivated => GetCanBeActivated();

    [SerializeField] PotionRarity rarity;
    [SerializeField] PotionActivation activation;
    [SerializeField] PotionTarget target;

    public enum PotionRarity { Common, Uncommon, Rare }
    public enum PotionActivation { Throw, Use }
    public enum PotionTarget { Player, SingleEnemy, Enemies, Miscellaneous }

    public virtual List<CombatAction> Activate(List<CharacterCombat> targets) {
        return null;
    }

    public virtual List<CombatAction> Activate(CharacterCombat target) {
        return null;
    }

    public virtual bool GetCanBeActivated() {
        //biasanya hanya saat dalam combat, bisa diluar combat
        return Combat.CurrentAction[^1].GetType() == typeof(CombatAction.PlayerTurn) || Combat.CurrentAction[^1].GetType() == typeof(CombatAction.UsePotion);
    }

    public static Potion GetRandom() {
        return PotionsRandomizer.Instance.Get() as Potion;
    }

    public static Potion Instantiate<T>() where T : Potion {
        return Library.Instantiate<Potion>(typeof(T));
    }
}