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

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (CombatActionsIsCharacterTurn(combatAction, Character)) {
                if (Character.Statuses.ContainsKey(typeof(Status.Blur))) { // || Statuses.ContainsKey(typeof(Status.block))
                    Combat.DoBefore(new CombatAction.ReduceStatus(Character, typeof(Status.Blur)), combatAction);
                }
                else {
                    Combat.DoBefore(new CombatAction.RemoveStatus(Character, typeof(Status.Block)), combatAction);
                }
            }
        }

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

    public class Shackled : Status {
        public Shackled(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) {
            if (stack >= 0) {
                Type = StatusType.Buff;
            }
            else {
                Type = StatusType.Debuff;
            }
        }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (CombatActionsIsCharacterTurn(combatAction, Character)) {
                Combat.DoAfter(new CombatAction.RemoveStatus(Character, typeof(Status.Shackled)), combatAction);
                Combat.DoAfter(new CombatAction.ApplyStatus(Character, new Status.Strength(Character, Stack)), combatAction);
            }
        }
    }

    public class Dexterity : Status {
        public Dexterity(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) {
            if (stack >= 0) {
                Type = StatusType.Buff;
            }
            else {
                Type = StatusType.Debuff;
            }
        }
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

    public class Vigor : Status {
        public Vigor(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.Attack attack && attack.Attacker == Character) {
                Combat.DoAfter(new CombatAction.RemoveStatus(Character, typeof(Vigor)), combatAction);
            }
        }
    }

    public class Ritual : Status {
        public Ritual(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (CombatActionsIsCharacterTurn(combatAction, Character)) {
                Combat.DoAfter(new CombatAction.ApplyStatus(Character, new Status.Strength(Character, Stack)), combatAction);
            }
        }
    }

    public class Plating : Status {
        int hpBefore;
        int hpAfter;
        public Plating(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void OnTriggerFirst(CombatAction combatAction) {
            base.OnTriggerFirst(combatAction);
            if (CombatActionsIsCharacterTurn(combatAction, Character)) {
                Combat.DoOnLast(new CombatAction.ApplyStatus(Character, new Status.Block(Character, Stack)), combatAction);
            }
        }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (combatAction is CombatAction.Damage damage && damage.Source is DamageSources.BasicAttack basicAttack && basicAttack.Attacker.Statuses.ContainsKey(typeof(Envenom))) {
                hpBefore = damage.Receiver.HP;
            }
        }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.Damage damage && damage.Source is DamageSources.BasicAttack basicAttack && basicAttack.Attacker.Statuses.ContainsKey(typeof(Envenom))) {
                hpAfter = damage.Receiver.HP;
                if (hpAfter < hpBefore) {
                    Combat.DoAfter(new CombatAction.ReduceStatus(damage.Receiver, typeof(Plating)), combatAction);
                }
            }
        }
    }

    public class Poison : Status {
        public Poison(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (CombatActionsIsCharacterTurn(combatAction, Character)) {
                Combat.DoBefore(new CombatAction.ReduceStatus(Character, typeof(Poison)), combatAction);
                Combat.DoBefore(new CombatAction.Damage(Character, new DamageSources.Poison(Character, Stack)), combatAction);
            }
        }
    }

    public class Regenerate : Status {
        bool isReduceByTurn;

        public Regenerate(CharacterCombat character, int stack, bool isReduceByTurn = true) : base(character, StatusType.Buff, true, stack) {
            this.isReduceByTurn = isReduceByTurn;
        }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Character.HP += Stack;
                if (isReduceByTurn) {
                    ReduceStatusAfterCharacterTurn(combatAction);
                }
            }
        }
    }

    public class Intangible : Status {
        public Intangible(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            ReduceStatusAfterCharacterTurn(combatAction);
        }
    }

    public class Artifact : Status {
        public Artifact(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (combatAction is CombatAction.ApplyStatus applyStatus && applyStatus.status.Type == StatusType.Debuff && applyStatus.receiver == Character) {
                applyStatus.Cancel();
                Combat.DoAfter(new CombatAction.ReduceStatus(Character, typeof(Artifact)), combatAction);
            }
        }
    }
    #endregion

    #region By Player
    public class AThousandCuts : Status {
        public AThousandCuts(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.PlayCard) {
                for (int i = 0; i < Combat.Enemies.Count; i++) {
                    Combat.DoAfter(new CombatAction.Damage(Combat.Enemies[i], new DamageSources.AnonymousDamage(Combat.Enemies[i], Stack, true)), combatAction);
                }
            }
        }
    }

    public class Accuracy : Status {
        public Accuracy(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }
    }

    public class AfterImage : Status {
        public AfterImage(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.PlayCard) {
                Combat.DoAfter(new CombatAction.ApplyStatus(Character, new Status.Block(Character, Stack)), combatAction);
            }
        }
    }

    public class Blur : Status {
        public Blur(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }
    }

    public class BulletTime : Status {
        public BulletTime(CharacterCombat character, List<Card> cards) : base(character, StatusType.Buff, false, 0) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            RemoveStatusAfterCharacterTurn(combatAction);
        }
    }

    public class Burst : Status {
        Card card;

        public Burst(CharacterCombat character, int stack, Card card) : base(character, StatusType.Buff, true, stack) {
            this.card = card;
        }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            RemoveStatusAfterCharacterTurn(combatAction);
            if (combatAction is CombatAction.PlayCard playCard) {
                if (CombatAction.PlayCard.Card == card) {
                    return;
                }
                if (CombatAction.PlayCard.Card.Type == Card.CardType.Skill && Character.Statuses.ContainsKey(typeof(Status.Burst))) {
                    if (playCard.Targets != null) {
                        Combat.DoAfter(new CombatAction.PlayCard(Character, playCard.Targets, CombatAction.PlayCard.Card), combatAction);
                    }
                    else {
                        Combat.DoAfter(new CombatAction.PlayCard(Character, playCard.Target, CombatAction.PlayCard.Card), combatAction);
                    }
                    Combat.DoAfter(new CombatAction.ReduceStatus(Character, typeof(Status.Burst)), combatAction);
                }
            }
        }
    }

    public class Caltrops : Status {
        public Caltrops(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.Attack attack && attack.Receivers.Contains(Character)) {
                Combat.DoAfter(new CombatAction.Damage(attack.Attacker, new DamageSources.AnonymousDamage(attack.Attacker, Stack, true)), combatAction);
            }
        }
    }

    public class Choke : Status {
        public Choke(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            RemoveStatusBeforeCharacterTurn(combatAction);
        }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.PlayCard playCard) {
                Combat.DoAfter(new CombatAction.Damage(Character, new DamageSources.AnonymousDamage(Character, Stack, true)), combatAction);
            }
        }
    }

    public class CorpseExplosion : Status {
        public CorpseExplosion(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.KillCharacter killCharacter && killCharacter.character == Character) {
                for (int i = 0; i < Combat.Enemies.Count; i++) {
                    if (Combat.Enemies[i] == Character) {
                        continue;
                    }
                    Combat.DoAfter(new CombatAction.Damage(Combat.Enemies[i], new DamageSources.AnonymousDamage(Combat.Enemies[i], Stack * Character.MaxHealth, true)), combatAction);
                }
            }
        }
    }

    public class DrawCardsNextTurn : Status {
        public DrawCardsNextTurn(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void OnTriggerFirst(CombatAction combatAction) {
            base.OnTriggerFirst(combatAction);
            if (CombatActionsIsCharacterTurn(combatAction, Character)) {
                Combat.DoOnFirst(new CombatAction.Draws(Stack), combatAction);
                Combat.DoOnFirst(new CombatAction.RemoveStatus(Character, typeof(DrawCardsNextTurn)), combatAction);
            }
        }
    }

    public class Envenom : Status {
        int hpBefore;
        int hpAfter;

        public Envenom(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (combatAction is CombatAction.Damage damage && damage.Source is DamageSources.BasicAttack basicAttack && basicAttack.Attacker.Statuses.ContainsKey(typeof(Envenom))) {
                hpBefore = damage.Receiver.HP;
            }
        }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.Damage damage && damage.Source is DamageSources.BasicAttack basicAttack && basicAttack.Attacker.Statuses.ContainsKey(typeof(Envenom))) {
                hpAfter = damage.Receiver.HP;
                if (hpAfter < hpBefore) {
                    Combat.DoAfter(new CombatAction.ApplyStatus(damage.Receiver, new Status.Poison(damage.Receiver, Stack)), combatAction);
                }
            }
        }
    }

    public class GainBlocksNextTurn : Status {
        public GainBlocksNextTurn(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void OnTriggerFirst(CombatAction combatAction) {
            base.OnTriggerFirst(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoOnLast(new CombatAction.ApplyStatus(Character, new Status.Block(Character, Stack)), typeof(CombatAction.CharacterTurn));
            }
            RemoveStatusBeforeCharacterTurn(combatAction);
        }
    }

    public class GainEnergiesNextTurn : Status {
        public GainEnergiesNextTurn(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void OnTriggerFirst(CombatAction combatAction) {
            base.OnTriggerFirst(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoOnLast(new CombatAction.AddEnergy(Stack), combatAction);
                RemoveStatusBeforeCharacterTurn(combatAction);
            }
        }
    }

    public class InfiniteBlades : Status {
        Card shivCard;

        public InfiniteBlades(CharacterCombat character, int stack, Card shivCard) : base(character, StatusType.Buff, true, stack) {
            this.shivCard = shivCard;
        }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                for (int i = 0; i < Stack; i++) {
                    Combat.DoBefore(new CombatAction.CreateCard(shivCard, Combat.HandPile), combatAction);
                }
            }
        }
    }

    public class Nightmare : Status {
        Card card;

        public Nightmare(CharacterCombat character, List<Card> cards) : base(character, StatusType.Buff, false) {
            this.card = cards[0];
        }

        public override void OnTriggerFirst(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoOnFirst(new CombatAction.CreateCards(card, 3, Combat.HandPile), combatAction);
                RemoveStatusBeforeCharacterTurn(combatAction);
            }
        }
    }

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

    public class NoxiusFumes : Status {
        public NoxiusFumes(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                for (int i = 0; i < Combat.Enemies.Count; i++) {
                    Combat.DoBefore(new CombatAction.ApplyStatus(Combat.Enemies[i], new Status.Poison(Combat.Enemies[i], Stack)), combatAction);
                }
            }
        }
    }

    public class PhantasmalKiller : Status {
        public PhantasmalKiller(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void OnTriggerFirst(CombatAction combatAction) {
            base.OnTriggerFirst(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoBefore(new CombatAction.ApplyStatus(Character, new Status.DoubleAttackDamage(Character, 1)), combatAction);
                ReduceStatusBeforeCharacterTurn(combatAction);
            }
        }
    }

    public class DoubleAttackDamage : Status {
        public DoubleAttackDamage(CharacterCombat character, int stack) : base(character, StatusType.Buff, false, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                RemoveStatusAfterCharacterTurn(combatAction);
            }
        }
    }

    public class Predator : Status {
        public Predator(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void BeforeTrigger(CombatAction combatAction) {
            base.BeforeTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoBefore(new CombatAction.Draws(Stack), combatAction);
                Combat.DoBefore(new CombatAction.RemoveStatus(Character, typeof(Predator)), combatAction);
            }
        }
    }

    public class ToolsOfTheTrade : Status {
        public ToolsOfTheTrade(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void OnTriggerLast(CombatAction combatAction) {
            base.OnTriggerLast(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoOnLast(new CombatAction.Draws(Stack), typeof(CombatAction.CharacterTurn));
                Combat.DoOnLast(new CombatAction.CardsSelection(Combat.HandPile, Stack, true, (cards) => { Combat.DoAfter(new CombatAction.Discards(cards), typeof(CombatAction.CardsSelection)); }), typeof(CombatAction.CharacterTurn));
            }
        }
    }

    public class WellLaidPlans : Status {
        static List<Card> Cards {
            get {
                if (cards == null) {
                    cards = new List<Card>();
                }
                return cards;
            }
            set {
                cards = value;
            }
        }
        static List<Card> cards;

        public WellLaidPlans(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        //ga bisa after trigger?, karena habis finish, dimasukin task lagi, jadi ada lagi?
        //TODO - Do On last
        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoOnLast(new CombatAction.CardsSelection(Combat.HandPile, Stack, false, (cards) => { Combat.MoveCards(cards, Cards); }), combatAction);
                Combat.DoAfter(new CombatAction.MoveCards(Cards, Combat.HandPile), combatAction);
            }
        }
    }

    public class WraithForm : Status {
        public WraithForm(CharacterCombat character, int stack) : base(character, StatusType.Debuff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.CharacterTurn characterTurn && characterTurn.Character == Character) {
                Combat.DoAfter(new CombatAction.ApplyStatus(Character, new Status.Dexterity(Character, -Stack)), combatAction);
            }
        }
    }
    #endregion

    #region By Enemy
    public class CurlUp : Status {
        public CurlUp(CharacterCombat character, int minBlockAmount, int maxBlockAmount) : base(character, StatusType.Buff, true, Dz.Random.Randomizer.Range(minBlockAmount, maxBlockAmount)) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.Damage damage && damage.Source is DamageSources.BasicAttack basicAttack && basicAttack.Receiver == Character) {
                Combat.DoAfter(new CombatAction.ApplyStatus(Character, new Block(Character, Combat.CalculateBlockGain(Character, (Character.Statuses[typeof(CurlUp)] as CurlUp).Stack))), combatAction);
                Combat.DoAfter(new CombatAction.RemoveStatus(Character, typeof(CurlUp)), combatAction);
            }
        }

        protected override string GetDescription() {
            return $"Gain {Stack} Blocks upon first receiving attack damage.";
        }
    }

    public class Enrage : Status {
        public Enrage(CharacterCombat character, int stack) : base(character, StatusType.Buff, true, stack) { }

        public override void AfterTrigger(CombatAction combatAction) {
            base.AfterTrigger(combatAction);
            if (combatAction is CombatAction.PlayedCard playedCard && playedCard.card.Type == Card.CardType.Skill) {
                Combat.DoAfter(new CombatAction.ApplyStatus(Character, new Status.Strength(Character, Stack)), combatAction);
            }
        }
    }
    #endregion
}