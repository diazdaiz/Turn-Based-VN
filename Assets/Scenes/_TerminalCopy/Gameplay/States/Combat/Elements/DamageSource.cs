public class DamageSources {
    public class DamageSource {
        public CharacterCombat Receiver;
        public int Damage;
        public bool AffectedByBlock;

        public DamageSource(CharacterCombat receiver, int damage, bool affectedByBlock) {
            AffectedByBlock = affectedByBlock;
            Receiver = receiver;
            Damage = damage;
        }
    }

    public class BasicAttack : DamageSource {
        public CharacterCombat Attacker { get; private set; }

        public BasicAttack(CharacterCombat attacker, CharacterCombat receiver, int damage) : base(receiver, damage, true) {
            this.Attacker = attacker;
        }
    }

    public class Poison : DamageSource {
        public Poison(CharacterCombat receiver, int damage) : base(receiver, damage, false) { }
    }

    public class AnonymousDamage : DamageSource {
        public AnonymousDamage(CharacterCombat receiver, int damage, bool affectedByBlock) : base(receiver, damage, affectedByBlock) { }
    }
}