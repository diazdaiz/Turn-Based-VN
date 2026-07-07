using System;
using System.Collections.Generic;
using UnityEngine;

//[Tool]
public partial class Card : MonoBehaviour {
    public CombatManager Combat => CombatManager.Instance;
    public CardIdentity Identity => identity;
    public CardRarity Rarity => rarity;
    public CardType Type => type;
    public CardActivation Activation => activation;
    public CardTarget Target => target;
    public bool CanBePlayedFromHand {
        get {
            if (Combat.Hero.Statuses.ContainsKey(typeof(Status.NoAttack)) && Type == CardType.Attack) {
                return false;
            }
            return GetCanBePlayedFromHand();
        }
    }
    public bool XEnergyForActivation => xEnergyForActivation;
    public int InitialEnergyForActivation => initialEnergyForActivation;
    public int EnergyForActivation {
        get {
            if (Setup.Cards.Contains(this)) {
                return 0;
            }
            if (Distraction.Cards.Contains(this)) {
                return 0;
            }
            if (Combat != null && Combat.Hero != null && Combat.Hero.Statuses.ContainsKey(typeof(Status.BulletTime))) {
                return 0;
            }
            return GetEnergyForActivation();
        }
        set {
            setEnergyForActivation = true;
            energyForActivation = value;
        }
    }
    public bool IsInnate => GetInnate();
    public bool IsUpgraded {
        get {
            return isUpgraded;
        }
        set {
            isUpgraded = value;
        }
    }
    public bool IsExhaust => GetExhaust();
    public bool IsEthereal => GetEtheral();

    [SerializeField] CardIdentity identity;
    [SerializeField] CardRarity rarity;
    [SerializeField] CardType type;
    [SerializeField] CardActivation activation;
    [SerializeField] CardTarget target;
    [SerializeField] bool xEnergyForActivation;
    [SerializeField] int initialEnergyForActivation;
    [SerializeField] bool isUpgraded;
    [SerializeField] bool innate;
    [SerializeField] bool exhaust;
    [SerializeField] bool ethereal;

    public enum CardIdentity { Colorless, Special, Silent }
    public enum CardRarity { Common, Uncommon, Rare }
    public enum CardType { Attack, Skill, Power }
    public enum CardActivation { PlayedByHand, OnDiscarded }
    public enum CardTarget { SingleEnemy, AllEnemies, Player }

    bool setEnergyForActivation = false;
    int energyForActivation;

    public virtual List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return null;
    }

    public virtual List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
        return null;
    }

    public virtual string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return "Description";
    }

    protected virtual bool GetCanBePlayedFromHand() {
        if (Activation != CardActivation.PlayedByHand) {
            return false;
        }
        return Combat.Energy >= EnergyForActivation && Activation == CardActivation.PlayedByHand;
    }

    protected virtual int GetEnergyForActivation() {
        // rada brute force dulu aja, kalau card ada di setup card atau player punya status bullet time, cost == 0
        //^^^^^^^ BAHKAN NARONYA DI GET AJA, biar kalau ke set manual juga msh 0 (sry capslock diaz di masa depan, biar ga lupa) ^^^^^^^^^^^^
        if (setEnergyForActivation) {
            return ManualSetEnergyForActivation();
        }
        return initialEnergyForActivation;
    }

    int ManualSetEnergyForActivation() {
        return energyForActivation;
    }

    protected virtual bool GetInnate() {
        return innate;
    }

    protected virtual bool GetExhaust() {
        return exhaust;
    }

    protected virtual bool GetEtheral() {
        return ethereal;
    }

    public static Card Instantiate<T>() where T : Card {
        return Library.Instantiate<Card>(typeof(T));
    }

    public static Card GetRandom(Card.CardIdentity identity, bool attacks = true, bool skills = true, bool powers = true, bool refreshPool = true, List<float> rarityChances = null, float upgradeChance = 0) {
        return CardsRandomizer.Instance.Get(identity, attacks, skills, powers, refreshPool, rarityChances, upgradeChance);
    }

    public static Card Instantiate(Type type) {
        return Library.Instantiate(type) as Card;
    }
}