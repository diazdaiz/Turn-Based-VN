using System.Collections.Generic;
using UnityEngine;

public class Intention {
    protected CombatManager combat => CombatManager.Instance;

    public virtual List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return null;
    }

    public class Attack : Intention {
        public int damage;

        public Attack(int damage) {
            this.damage = damage;
        }

        public Attack(int minDamage, int maxDamage) {
            damage = Dz.Random.Randomizer.Range(minDamage, maxDamage);
        }

        public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
            return new() { new CombatAction.Attack(caster, target, damage) };
        }
    }

    public class Block : Intention {
        public Status status;
        public int block;

        public Block(Status status, int minBlock, int maxBlock) {
            this.status = status;
            block = Dz.Random.Randomizer.Range(minBlock, maxBlock);
        }

        public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
            return new() {
                new CombatAction.ApplyStatus(target, status),
                new CombatAction.ApplyStatus(target, new Status.Block(target, block))
            };
        }
    }

    public class Buff : Intention {
        public Status status;
        public CharacterCombat target;

        public Buff(Status status, CharacterCombat target = null) {
            this.status = status;
            this.target = target;
        }

        public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
            return new() {
                new CombatAction.ApplyStatus(target, status)
            };
        }
    }

    public class Debuff : Intention {
        public Status status;

        public Debuff(Status status) {
            this.status = status;
        }

        public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
            return new() {
                new CombatAction.ApplyStatus(target, status)
            };
        }
    }

    public class AttackBlock : Intention {
        public int block;
        public int damage;

        public AttackBlock(int minBlock, int maxBlock, int minDamage, int maxDamage) {
            block = Dz.Random.Randomizer.Range(minBlock, maxBlock);
            damage = Dz.Random.Randomizer.Range(minDamage, maxDamage);
        }

        public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
            return new() {
                new CombatAction.Attack(caster, target, damage),
                new CombatAction.ApplyStatus(caster, new Status.Block(caster, block))
            };
        }
    }

    //TODO - Others Intention
    public class AttackBuff : Intention {
        public int damage;
    }

    public class AttackDebuff : Intention {
        public int damage;
    }

    public class BlockBuff : Intention {

    }

    public class BlockDebuff : Intention {

    }

    public class Summon : Intention {
        List<Enemy> enemies;
        List<Vector2> positions;

        public Summon(List<Enemy> enemies, List<Vector2> positions) {
            this.enemies = enemies;
            this.positions = positions;
        }

        public override List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
            List<CombatAction> combatActions = new List<CombatAction>();
            return new() { new CombatAction.Summon(enemies, positions) };
        }
    }
}

