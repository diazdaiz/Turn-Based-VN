using System;
using UnityEngine;

//[GlobalClass]
public partial class Relic : MonoBehaviour {
    public CombatManager Combat => CombatManager.Instance;
    public RelicRarity Rarity => rarity;

    [SerializeField] RelicRarity rarity;

    public enum RelicRarity { Common, Uncommon, Rare, Shop, Boss, Event }

    public virtual void Subscribe() {

    }

    public virtual void Activate(CombatAction combatAction) {

    }

    public static Relic GetRandomFromPool() {
        return RelicsRandomizer.Instance.Get() as Relic;
    }

    public static Relic Instantiate<T>() where T : Relic {
        return Library.Instantiate<Relic>(typeof(T));
    }

    public static Relic Instantiate(Type type) {
        return Library.Instantiate(type) as Relic;
    }
}