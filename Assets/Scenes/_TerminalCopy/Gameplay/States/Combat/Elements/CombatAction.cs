using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatAction : Task {
    protected static CombatManager combat => CombatManager.Instance;
    public static Dictionary<Type, Action<CombatAction>> BeforeTriggers {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return beforeTriggers;
        }
    }
    public static Dictionary<Type, Action<CombatAction>> OnTriggersFirst {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return onTriggersFirst;
        }
    }
    public static Dictionary<Type, Action<CombatAction>> OnTriggersLast {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return onTriggersLast;
        }
    }
    public static Dictionary<Type, Action<CombatAction>> AfterTriggers {
        get {
            if (!triggersSet) {
                ResetTiggers();
            }
            return afterTriggers;
        }
    }
    public override bool IsFinished {
        get {
            return isFinished && TaskTimer >= Duration;
        }
        set {
            isFinished = value;
        }
    }
    public override bool IsRunning {
        get {
            return IsStarted && !IsFinished;
        }
        set {
            isRunning = value;
        }
    }
    public float Duration { get; set; } = 0.12f;
    static Dictionary<Type, Action<CombatAction>> beforeTriggers;
    static Dictionary<Type, Action<CombatAction>> onTriggersFirst;
    static Dictionary<Type, Action<CombatAction>> onTriggersLast;
    static Dictionary<Type, Action<CombatAction>> afterTriggers;
    public float TaskTimer { get; set; } = 0;
    bool isFinished = false;
    bool isRunning = false;
    static bool triggersSet = false;
    bool beforeTriggersInvoked = false;
    bool onTriggersFirstInvoked = false;
    bool onTriggersLastInvoked = false;

    public static void ResetTiggers() {
        Debug.Log("initialize trigger");
        List<Type> derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(CombatAction)) && !type.IsAbstract)
            .ToList();
        beforeTriggers = new();
        onTriggersFirst = new();
        onTriggersLast = new();
        afterTriggers = new();

        for (int i = 0; i < derivedTypes.Count; i++) {
            beforeTriggers.Add(derivedTypes[i], null);
            onTriggersFirst.Add(derivedTypes[i], null);
            onTriggersLast.Add(derivedTypes[i], null);
            afterTriggers.Add(derivedTypes[i], null);
        }
        triggersSet = true;
    }

    /// <summary>
    /// Note: taro subscribe sebelum base start(?)
    /// </summary>
    public override void Start() {
        if (!beforeTriggersInvoked) {
            BeforeTriggers[GetType()]?.Invoke(this);
            beforeTriggersInvoked = true;
            // naro action lain yang ke trigger dari action ini biar ke start duluan
            // jadi nunda start dia, kyk ngetrigger duluan combat action lainnya
        }
        else if (!onTriggersFirstInvoked) {
            OnTriggersFirst[GetType()]?.Invoke(this);
            onTriggersFirstInvoked = true;
        }
        else {
            base.Start();
            Debug.Log(GetType());
        }
    }

    public override void Finish() {
        if (!onTriggersLastInvoked) {
            onTriggersLastInvoked = true;
            if (OnTriggersLast[GetType()] == null) {
                base.Finish();
                AfterTriggers[GetType()]?.Invoke(this);
            }
            else {
                OnTriggersLast[GetType()]?.Invoke(this);
                if (tasksSequence != null && tasksSequence.Count > 0 && !tasksSequence[0].IsFinished && !tasksSequence[0].IsCancelled) {
                    return;
                }
                else {
                    base.Finish();
                    AfterTriggers[GetType()]?.Invoke(this);
                }
            }
        }
        else {
            base.Finish();
            AfterTriggers[GetType()]?.Invoke(this);
        }
    }

    public override void FixedUpdate(float dt) {
        base.FixedUpdate(dt);
        if (TaskTimer <= Duration) {
            TaskTimer += dt;
        }
    }

    public class SnippetTemp : CombatAction {
        public SnippetTemp() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            Finish();
        }
    }

    public class Delay : CombatAction {
        float delay;
        float timer;

        public Delay(float second) {
            this.delay = second;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            timer = 0;
        }

        public override void Update(float dt) {
            base.Update(dt);
            timer += dt;
            if (timer > delay) {
                Finish();
            }
        }
    }

    #region Actions
    public class StartCombat : CombatAction {
        //misal kyk efek nambah orb untuk player, atau efek nambah draw player, dll
        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            CharacterTurn characterTurn = combat.CharacterTurnsInOrder[0];
            combat.DoAfter(characterTurn, this);
            combat.CurrentCharacterTurn = characterTurn;
            //combat.DoOnFirst(new CreateSkill(typeof(Sharingan), combat.PlayerTeam[0].Skills), typeof(PlayerTurn));

            Finish();
        }
    }

    public class CharacterTurn : CombatAction {
        public CharacterCombat Character;

        public CharacterTurn(CharacterCombat character) {
            Character = character;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            FinishWhenLastTaskInTaskSequenceCompleted = false;
            CancelWhenTaskInTaskSequenceCanceled = false;
        }

        public override void Finish() {
            combat.CharacterTurnsInOrder.RemoveAt(0);
            combat.AddCharacterTurn(Character);

            combat.DoAfter(combat.CharacterTurnsInOrder[0], this);
            combat.CurrentCharacterTurn = combat.CharacterTurnsInOrder[0];

            base.Finish();
        }
    }

    public class FinishTurn : CombatAction {
        CharacterTurn characterTurn;

        public FinishTurn(CharacterTurn characterTurn) {
            this.characterTurn = characterTurn;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }



            Finish();
        }
    }

    public class PlaySkill : CombatAction {
        public static Skill Skill => combat.SkillsActivationOrder[0];
        Skill skill;
        public CharacterCombat Caster { get; private set; }
        public CharacterCombat Target { get; private set; }
        public List<CharacterCombat> Targets { get; private set; }

        public PlaySkill(CharacterCombat caster, CharacterCombat target, Skill skill) {
            this.skill = skill;
            this.Caster = caster;
            this.Target = target;
        }

        public PlaySkill(CharacterCombat caster, List<CharacterCombat> targets, Skill skill) {
            this.skill = skill;
            this.Caster = caster;
            this.Targets = targets;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            combat.SkillPoint -= skill.SkillPointForActivation;
            if (Targets != null && Targets.Count > 0) {
                BreakTask(new(skill.Activate(Caster, Targets)));
            }
            else if (Target != null) {
                BreakTask(new(skill.Activate(Caster, Target)));
            }
        }
    }

    public class Attack : CombatAction {
        public CharacterCombat Attacker { get; private set; }
        public List<CharacterCombat> Receivers { get; private set; }
        public int DamagePercentage { get; private set; }

        public Attack(CharacterCombat attacker, CharacterCombat receiver, int damagePercentage) {
            Attacker = attacker;
            Receivers = new() { receiver };
            DamagePercentage = damagePercentage;
        }

        public Attack(CharacterCombat attacker, List<CharacterCombat> receivers, int damagePercentage) {
            Attacker = attacker;
            Receivers = receivers;
            DamagePercentage = damagePercentage;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            List<Task> tasksList = new();
            for (int i = Receivers.Count - 1; i >= 0; i--) {
                int damage = combat.CalculateAttack(Attacker, Receivers[i], DamagePercentage);
                tasksList.Add(new Damage(Receivers[i], new DamageSources.BasicAttack(Attacker, Receivers[i], damage)));
            }
            BreakTask(tasksList);
        }
    }

    public class Damage : CombatAction {
        public CharacterCombat Receiver { get; private set; }
        public DamageSources.DamageSource Source { get; private set; }

        public Damage(CharacterCombat receiver, DamageSources.DamageSource source) {
            this.Receiver = receiver;
            this.Source = source;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (Source.AffectedByBlock) {
                int damage = Source.Damage;
                Receiver.HP -= damage;
                //if (damage > Receiver.block) {
                //    damage -= Receiver.block;
                //    Receiver.block = 0;
                //    Receiver.HP -= damage;
                //}
                //else {
                //    Receiver.block -= damage;
                //}
            }
            else {
                Receiver.HP -= Source.Damage;
            }

            Finish();
        }
    }

    public class ApplyStatus : CombatAction {
        public CharacterCombat giver;
        public CharacterCombat receiver;
        public Status status;

        public ApplyStatus(CharacterCombat receiver, Status status, CharacterCombat giver = null) {
            this.receiver = receiver;
            this.status = status;
            this.giver = giver;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            Type statusType = status.GetType();
            if (!receiver.Statuses.ContainsKey(statusType)) {
                receiver.Statuses[statusType] = status;
                status.OnApply();
            }
            else {
                if (status.IsStackable) {
                    receiver.Statuses[statusType].Stack += status.Stack;
                    if (receiver.Statuses[statusType].Stack == 0) {
                        combat.DoAfter(new CombatAction.RemoveStatus(receiver, statusType), this);
                    }
                }
            }

            Finish();
        }
    }

    public class RemoveStatus : CombatAction {
        public CharacterCombat character;
        public Type statusType;

        public RemoveStatus(CharacterCombat character, Type statusType) {
            this.character = character;
            this.statusType = statusType;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (character.Statuses.ContainsKey(statusType)) {
                character.Statuses[statusType].OnRemoved();
                character.Statuses.Remove(statusType);
            }

            Finish();
        }
    }

    public class ReduceStatus : CombatAction {
        CharacterCombat character;
        Type statusType;
        int stack;

        public ReduceStatus(CharacterCombat character, Type statusType, int stack = 1) {
            this.character = character;
            this.statusType = statusType;
            this.stack = stack;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            character.Statuses[statusType].Stack -= stack;
            if (character.Statuses[statusType].Stack <= 0) {
                combat.DoAfter(new RemoveStatus(character, statusType), this);
            }
            Finish();
        }
    }

    public class CreateSkill : CombatAction {
        public Skill InstantiatedSkill;
        Type skillType;
        List<Skill> to;

        public CreateSkill(Skill skill, List<Skill> to) {
            this.InstantiatedSkill = skill;
            this.to = to;
        }

        public CreateSkill(Type skillType, List<Skill> to) {
            this.skillType = skillType;
            this.to = to;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (InstantiatedSkill == null) {
                InstantiatedSkill = Skill.Instantiate(skillType);
            }
            InstantiatedSkill.transform.parent = combat.transform;
            Finish();
        }
    }

    public class CreateSkills : CombatAction {
        Type skillType;
        int amount;
        List<Skill> to;

        public CreateSkills(Skill skill, int amount, List<Skill> to) {
            skillType = skill.GetType();
            this.amount = amount;
            this.to = to;
        }

        public CreateSkills(Type skillType, int amount, List<Skill> to) {
            this.skillType = skillType;
            this.amount = amount;
            this.to = to;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            List<Task> createSkills = new();
            for (int i = 0; i < amount; i++) {
                createSkills.Add(new CreateSkill(skillType, to));
            }
            BreakTask(createSkills);
            Finish();
        }
    }

    public class KillCharacter : CombatAction {
        public CharacterCombat character;

        public KillCharacter(CharacterCombat character) {
            this.character = character;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.PlayerTeam.Remove(character);
            combat.EnemyTeam.Remove(character);

            int indexToRemove = -1;
            for (int i = 0; i < combat.CharacterTurnsInOrder.Count; i++) {
                if (combat.CharacterTurnsInOrder[i].Character == character) {
                    indexToRemove = i;
                    break;
                }
            }
            combat.CharacterTurnsInOrder.RemoveAt(indexToRemove);

            Finish();
        }

        public override void Finish() {
            combat.DoAfter(new CheckEndCombat(), this);
            base.Finish();
        }
    }

    public class GainSkillPoint : CombatAction {
        int amount;

        public GainSkillPoint(int amount) {
            this.amount = amount;
        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.SkillPoint += amount;
            Finish();
        }
    }

    public class GenerateEnemiesIntention : CombatAction {
        public GenerateEnemiesIntention() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }

            Finish();
        }
    }

    public class CheckEndCombat : CombatAction {
        public CheckEndCombat() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            if (combat.IsCombating) {
                if (combat.PlayerTeam.Count <= 0 || combat.EnemyTeam.Count <= 0) {
                    if (combat.CurrentActionAllDepths.Count == 0) {
                        combat.Do(new CombatAction.EndCombat());
                    }
                    else {
                        combat.DoBefore(new CombatAction.EndCombat(), combat.CurrentActionAllDepths[0] as CombatAction);
                    }
                }
            }
            Finish();
        }
    }

    public class EndCombat : CombatAction {
        //kalau ada logic yang perlu dibuat sebelum ending, misal kalkulasi reward, dll
        public EndCombat() {

        }

        public override void Start() {
            base.Start();
            if (!isRunning) {
                return;
            }
            combat.IsCombating = false;
            combat.ResetCombat();

            Debug.Log("Ending Combat!");
            Finish();
        }

        public override void Finish() {
            base.Finish();
        }
    }
    #endregion
}