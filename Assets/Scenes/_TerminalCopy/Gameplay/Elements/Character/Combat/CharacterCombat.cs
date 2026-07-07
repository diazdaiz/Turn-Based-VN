using System;
using UnityEngine;

public class CharacterCombat : MonoBehaviour {
    public int MaxHealth { get; private set; }
    public int HP {
        get {
            return stats.HP;
        }
        set {
            stats.HP = value;
            if (stats.HP > MaxHealth) {
                stats.HP = MaxHealth;
            }
            if (stats.HP < 0) {
                stats.HP = 0;
            }
        }
    }
    public int block {
        get {
            return Statuses.ContainsKey(typeof(Status.Block)) ? Statuses[typeof(Status.Block)].Stack : 0;
        }
        set {
            if (Statuses.ContainsKey(typeof(Status.Block))) {
                Statuses[typeof(Status.Block)].Stack = value;
            }
            else {
                combat.DoAfter(new CombatAction.ApplyStatus(this, new Status.Block(this, value)), combat.CurrentAction[^1]);
            }
        }
    }
    public bool isAlive => HP > 0;

    [SerializeField] CharacterStats stats;

    [System.Serializable]
    public class CharacterStats {
        public int HP = 100;
        public int ATK = 50;
        public int DEF = 10;
        public int SPD = 100;
    }

    public System.Collections.Generic.Dictionary<Type, Status> Statuses {
        get {
            if (statuses == null) {
                statuses = new();
            }
            return statuses;
        }
        set {
            statuses = value;
        }
    }
    System.Collections.Generic.Dictionary<Type, Status> statuses;
    public System.Collections.Generic.Dictionary<Type, Status> initialStatuses;
    protected CombatManager combat => CombatManager.Instance;

    public virtual void Start() {
        // untuk sementara, MaxHealth di set dari HP awal dari stats
        MaxHealth = stats.HP;
        if (initialStatuses == null) {
            initialStatuses = new();
        }
        foreach (Type type in initialStatuses.Keys) {
            combat.Do(new CombatAction.ApplyStatus(this, initialStatuses[type]));
        }
    }

    public void SubscribeDamage() {
        CombatAction.AfterTriggers[typeof(CombatAction.Damage)] += OnDamaged;
    }

    public virtual void OnDamaged(CombatAction combatAction) {
        if (combatAction is CombatAction.Damage damage && damage.Receiver == this) {
            if (HP <= 0) {
                combat.DoAfter(new CombatAction.KillCharacter(this), combatAction);
                CombatAction.AfterTriggers[typeof(CombatAction.Damage)] -= OnDamaged;
            }
        }
    }

    public virtual void OnDestroy() {

    }
}
