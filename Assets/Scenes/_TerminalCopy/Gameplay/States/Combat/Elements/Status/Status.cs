using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Status {
    public CombatManager Combat => CombatManager.Instance;
    public CharacterCombat Character { get; set; }
    public bool IsStackable { get; set; }
    public string Description => GetDescription();
    public int Stack { get; set; }
    public enum StatusType { Buff, Debuff }
    public StatusType Type { get; set; }

    public Status(CharacterCombat character, StatusType type, bool isStackable, int stack = 0) {
        Character = character;
        Type = type;
        IsStackable = isStackable;
        Stack = stack;
    }

    protected virtual string GetDescription() {
        return "description";
    }

    public virtual void OnApply() {
        List<Type> derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(CombatAction)) && !type.IsAbstract)
            .ToList();

        for (int i = 0; i < derivedTypes.Count; i++) {
            CombatAction.BeforeTriggers[derivedTypes[i]] += BeforeTrigger;
            CombatAction.OnTriggersFirst[derivedTypes[i]] += OnTriggerFirst;
            CombatAction.OnTriggersLast[derivedTypes[i]] += OnTriggerLast;
            CombatAction.AfterTriggers[derivedTypes[i]] += AfterTrigger;
        }
    }

    public virtual void BeforeTrigger(CombatAction combatAction) {

    }

    public virtual void OnTriggerFirst(CombatAction combatAction) {

    }

    public virtual void OnTriggerLast(CombatAction combatAction) {

    }

    public virtual void AfterTrigger(CombatAction combatAction) {

    }

    public virtual void OnRemoved() {
        List<Type> derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(CombatAction)) && !type.IsAbstract)
            .ToList();

        for (int i = 0; i < derivedTypes.Count; i++) {
            CombatAction.BeforeTriggers[derivedTypes[i]] -= BeforeTrigger;
            CombatAction.OnTriggersFirst[derivedTypes[i]] -= OnTriggerFirst;
            CombatAction.OnTriggersLast[derivedTypes[i]] -= OnTriggerLast;
            CombatAction.AfterTriggers[derivedTypes[i]] -= AfterTrigger;
        }
    }

    public void ReduceStatusBeforeCharacterTurn(CombatAction combatAction) {
        if (CombatActionsIsCharacterTurn(combatAction, Character)) {
            Combat.DoBefore(new CombatAction.ReduceStatus(Character, GetType()), combatAction);
        }
    }

    public void ReduceStatusAfterCharacterTurn(CombatAction combatAction) {
        if (CombatActionsIsCharacterTurn(combatAction, Character)) {
            Combat.DoAfter(new CombatAction.ReduceStatus(Character, GetType()), combatAction);
        }
    }

    public void RemoveStatusBeforeCharacterTurn(CombatAction combatAction) {
        if (CombatActionsIsCharacterTurn(combatAction, Character)) {
            Combat.DoBefore(new CombatAction.RemoveStatus(Character, GetType()), combatAction);
        }
    }

    public void RemoveStatusAfterCharacterTurn(CombatAction combatAction) {
        if (CombatActionsIsCharacterTurn(combatAction, Character)) {
            Combat.DoAfter(new CombatAction.RemoveStatus(Character, GetType()), combatAction);
        }
    }

    bool CombatActionsIsCharacterTurn(CombatAction combatAction, CharacterCombat character) {
        return combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == character;
    }

    #region General
    public class Block : Status {
        public Block(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        protected override string GetDescription() {
            return $"Block for {Stack} attack damage.";
        }

    }

    public class Weak : Status {
        public Weak(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            ReduceStatusAfterCharacterTurn(combatAction);
        }

        protected override string GetDescription() {
            return $"Target deals 25% less attack damage.";
        }
    }

    public class Strength : Status {
        public Strength(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) {
            if (stack >= 0) {
                if (stack == 0) {
                    Debug.LogWarning("nambahin 0 strength");
                }
                Type = StatusType.Buff;
            }
            else {
                Type = StatusType.Debuff;
            }
        }

        protected override string GetDescription() {
            return $"Increases attack damage by {Stack}.";
        }
    }

    public class Artist : Status {
        public Artist(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }
    }

    public class Focused : Status {
        public Focused(CharacterCombat character, int stack) : base(character, StatusType.Debuff, false, stack) { }
    }

    public class Frail : Status {
        public Frail(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            ReduceStatusAfterCharacterTurn(combatAction);
        }
    }

    public class Vulnerable : Status {
        public Vulnerable(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            ReduceStatusAfterCharacterTurn(combatAction);
        }

        protected override string GetDescription() {
            return $"Target takes 50% more damage from attacks.";
        }
    }

    public class Burn : Status {
        public Burn(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (CombatActionsIsCharacterTurn(combatAction, Character)) {
                Combat.DoBefore(new CombatAction.ReduceStatus(Character, typeof(Burn), 10), combatAction);
                Combat.DoBefore(new CombatAction.Damage(Character, new DamageSources.Burn(Character, Stack)), combatAction);
            }
        }
    }

    #endregion

    #region By Player


    public class NoAttack : Status {
        public NoAttack(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            ReduceStatusAfterCharacterTurn(combatAction);
        }

        protected override string GetDescription() {
            return $"You can't attack this turn.";
        }
    }
    #endregion
}