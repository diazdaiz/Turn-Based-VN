using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombat : MonoBehaviour {
    public int MaxHealth { get; private set; }
    public CharacterStats Stats { get; set; }
    public List<Skill> Skills { get; set; }
    public int ActionValue { get; set; }
    public int HP {
        get {
            return Stats.HP;
        }
        set {
            Stats.HP = value;
            if (Stats.HP > MaxHealth) {
                Stats.HP = MaxHealth;
            }
            if (Stats.HP < 0) {
                Stats.HP = 0;
            }
        }
    }
    public bool isAlive => HP > 0;

    public Action<CombatAction.CharacterTurn> OnTurnStart;
    public Action<CombatAction.Attack> OnAttack;
    public Action<CombatAction.Damage> OnTakeDamage;
    public Action<CombatAction.KillCharacter> OnDeath;

    [SerializeField] CharacterStats stats;
    [SerializeField] List<Skill> skills = new();

    [Serializable]
    public class CharacterStats {
        public int HP = 1397;
        public int ATK = 523;
        public int DEF = 485;
        public int SPD = 101;
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
        Stats = new() {
            HP = stats.HP,
            SPD = stats.SPD,
            ATK = stats.ATK,
            DEF = stats.DEF,
        };
        if (initialStatuses == null) {
            initialStatuses = new();
        }
        foreach (Type type in initialStatuses.Keys) {
            combat.Do(new CombatAction.ApplyStatus(this, initialStatuses[type]));
        }
    }

    public void InitiateSkills() {
        Skills = new();
        for (int i = 0; i < skills.Count; i++) {
            Skills.Add(Instantiate(skills[i]));
        }
    }

    public void SubscribeCombatEvents() {
        CombatAction.OnTriggersFirst[typeof(CombatAction.CharacterTurn)] += OnBeginCharacterTurn;
        CombatAction.AfterTriggers[typeof(CombatAction.Damage)] += OnCharacterDamaged;
        CombatAction.AfterTriggers[typeof(CombatAction.Attack)] += OnCharacterAttacked;
        CombatAction.AfterTriggers[typeof(CombatAction.KillCharacter)] += OnCharacterKilled;
    }

    public virtual void OnBeginCharacterTurn(CombatAction combatAction) {
        if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == this) {
            OnTurnStart?.Invoke(characterTurn);
        }
    }

    public virtual void OnCharacterAttacked(CombatAction combatAction) {
        if (combatAction is CombatAction.Attack attack && attack.Attacker == this) {
            OnAttack?.Invoke(attack);
        }
    }

    public virtual void OnCharacterDamaged(CombatAction combatAction) {
        if (combatAction is CombatAction.Damage damage && damage.Receiver == this) {
            OnTakeDamage?.Invoke(damage);
            if (HP <= 0) {
                combat.DoAfter(new CombatAction.KillCharacter(this), combatAction);
                CombatAction.AfterTriggers[typeof(CombatAction.Damage)] -= OnCharacterDamaged;
            }
        }
    }

    public virtual void OnCharacterKilled(CombatAction combatAction) {
        if (combatAction is CombatAction.KillCharacter killCharacter && killCharacter.character == this) {
            OnDeath?.Invoke(killCharacter);
        }
    }

    public virtual void OnDestroy() {

    }
}
