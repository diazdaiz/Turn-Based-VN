using System;
using System.Collections.Generic;
using UnityEngine;

public partial class CombatManager : MonoBehaviour {
    public static CombatManager Instance { get; private set; }
    public List<CombatAction> CurrentActionAllDepths => combatActionManager.CurrentTaskWithDepth?.ConvertAll(x => (CombatAction)x);

    //Character
    public List<CharacterCombat> PlayerTeam { get; private set; }
    public List<CharacterCombat> EnemyTeam { get; private set; }

    //Skill
    public CharacterCombat PotentialTarget { get; set; }
    public List<Skill> SkillsActivationOrder { get; set; } //0 <- n, 0 yang sedang aktif

    //Turn Order
    public CombatAction.CharacterTurn CurrentCharacterTurn { get; set; }
    public List<CombatAction.CharacterTurn> CharacterTurnsInOrder { get; set; } //diambil dari awal
    public int SmallestActionValue { get; set; }

    //Other Properties
    GameplayManager gameplay => GameplayManager.Instance;
    public Action EndingCombat { get; set; }
    public int MaxSkillPoint {
        get {
            return maxSkillPoint;
        }
        set {
            maxSkillPoint = value;
        }
    }
    public int SkillPoint { get; set; } = 0;
    public bool IsCombating { get; set; } // combating combating combating combating combating combating
    public GameObject Canvas => canvas;

    [SerializeField] int maxSkillPoint = 5;
    [SerializeField] int startingSkillPoint = 3;
    [SerializeField] GameObject canvas;
    //[SerializeField] PlayerCombatController skillSelection;

    CombatActionManager combatActionManager;

    public void SetCombatInitialProperties(List<CharacterCombat> playerTeam, List<CharacterCombat> enemyTeam, bool Ambushed) {
        ResetCombat();
        PlayerTeam = playerTeam;
        for (int i = 0; i < playerTeam.Count; i++) {
            PlayerTeam[i].InitiateSkills();
            PlayerTeam[i].SubscribeCombatEvents();
            AddCharacterTurn(playerTeam[i], Ambushed);
        }
        EnemyTeam = enemyTeam;
        for (int i = 0; i < EnemyTeam.Count; i++) {
            EnemyTeam[i].InitiateSkills();
            EnemyTeam[i].SubscribeCombatEvents();
            //EnemyTeam[i].ActionValue = (int)(10000f / EnemyTeam[i].Stats.SPD);
            AddCharacterTurn(enemyTeam[i]);
        }
        SkillPoint = startingSkillPoint;
    }

    #region Combat
    public void StartCombat() {
        canvas.SetActive(true);
        Do(new CombatAction.StartCombat());
        IsCombating = true;
    }

    public void AddCharacterTurn(CharacterCombat character, bool Ambushed = false) {
        character.ActionValue += (int)(10000f / character.Stats.SPD * (Ambushed ? 1.2f : 1f));
        for (int i = 0; i < CharacterTurnsInOrder.Count; i++) {
            if (CharacterTurnsInOrder[i].Character.ActionValue < character.ActionValue) {
                continue;
            }
            if (PlayerTeam.Contains(character) || EnemyTeam.Contains(character)) {
                CharacterTurnsInOrder.Insert(i, new CombatAction.CharacterTurn(character));
                return;
            }
            else {
                Debug.LogError("Character engga di player team & enemy team");
                return;
            }
        }
        CharacterTurnsInOrder.Add(new CombatAction.CharacterTurn(character));
    }

    public int CalculateAttack(CharacterCombat attacker, CharacterCombat receiver, int damagePercentage) {
        //relic, status
        int strength = attacker.Statuses.ContainsKey(typeof(Status.Strength)) ? attacker.Statuses[typeof(Status.Strength)].Stack : 0;
        bool weak = attacker.Statuses.ContainsKey(typeof(Status.Weak));

        bool vulnerable = false;
        if (receiver != null) {
            vulnerable = receiver.Statuses.ContainsKey(typeof(Status.Vulnerable));
        }
        int damage = Mathf.FloorToInt((damagePercentage / 100f * attacker.Stats.ATK + strength) * (weak ? 0.75f : 1f) * (vulnerable ? 1.5f : 1f) * 250f / receiver.Stats.DEF);
        Debug.Log(damage);
        // ga bisa kurang dari 0
        return Mathf.Max(0, damage);
    }

    public int CalculateBlockGain(CharacterCombat character, int baseBlockGain) {
        bool frail = character.Statuses.ContainsKey(typeof(Status.Frail));
        int block = Mathf.RoundToInt(baseBlockGain * (frail ? 0.75f : 1f));
        return Mathf.Max(0, block);
    }

    public void Do(CombatAction combatAction, CombatAction before = null, CombatAction after = null) {
        if (before != null) {
            DoBefore(combatAction, before);
        }
        else if (after != null) {
            DoAfter(combatAction, after);
        }
        else {
            combatActionManager.AddTask(combatAction);
        }
    }

    public void DoBefore(CombatAction Do, CombatAction before) {
        combatActionManager.AddTaskBefore(Do, before);
    }

    public void DoBefore(CombatAction Do, Type before) {
        combatActionManager.AddTaskBefore(Do, before);
    }

    public void DoAfter(CombatAction Do, CombatAction after) {
        combatActionManager.AddTaskAfter(Do, after);
    }

    public void DoAfter(CombatAction Do, Type after) {
        combatActionManager.AddTaskAfter(Do, after);
    }

    public void DoOnFirst(CombatAction Do, Type On) {
        combatActionManager.AddTaskOnFirst(Do, On);
    }

    public void DoOnFirst(CombatAction Do, CombatAction On) {
        combatActionManager.AddTaskOnFirst(Do, On);
    }

    public void DoOnLast(CombatAction Do, Type On) {
        combatActionManager.AddTaskOnLast(Do, On);
    }

    public void DoOnLast(CombatAction Do, CombatAction On) {
        combatActionManager.AddTaskOnLast(Do, On);
    }

    public Skill CreateSkill(Type skillType, List<Skill> on) {
        Skill skill = Skill.Instantiate(skillType);
        skill.transform.parent = transform;
        on.Add(skill);
        return skill;
    }

    public Skill CopySkill(Skill skill, List<Skill> on) {
        Skill duplicatedSkill = Instantiate(skill, transform);
        on.Add(duplicatedSkill);
        return duplicatedSkill;
    }

    public void Shuffle<T>(IList<T> list) {
        System.Random rng = new System.Random();
        int n = list.Count;

        while (n > 1) {
            n--;
            int k = rng.Next(n + 1); // 0 <= k <= n
            (list[k], list[n]) = (list[n], list[k]); // swap
        }
    }

    public void ResetCombat() {
        SkillPoint = 0;

        if (PlayerTeam != null) {
            for (int i = 0; i < PlayerTeam.Count; i++) {
                for (int j = PlayerTeam[i].Skills.Count - 1; j >= 0; j--) {
                    Destroy(PlayerTeam[i].Skills[j]);
                }
                PlayerTeam[i].Skills = new();
                PlayerTeam[i].Statuses = new();
                PlayerTeam[i].ActionValue = 0;
            }
        }
        if (EnemyTeam != null) {
            for (int i = 0; i < EnemyTeam.Count; i++) {
                for (int j = EnemyTeam[i].Skills.Count - 1; j >= 0; j--) {
                    Destroy(EnemyTeam[i].Skills[j]);
                }
                EnemyTeam[i].Skills = new();
                EnemyTeam[i].Statuses = new();
                EnemyTeam[i].ActionValue = 0;
            }
        }
        CombatAction.ResetTiggers();
        //// CombatAction.
        PlayerTeam = new();
        EnemyTeam = new();
        CharacterTurnsInOrder = new();

        combatActionManager = new();
        combatActionManager.Start();
    }
    #endregion

    public void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }

    public void Update() {
        if (combatActionManager != null) {
            combatActionManager.Update(Time.deltaTime);
        }
    }

    public void FixedUpdate() {
        if (combatActionManager != null) {
            combatActionManager.FixedUpdate(Time.fixedDeltaTime);
        }
    }
}