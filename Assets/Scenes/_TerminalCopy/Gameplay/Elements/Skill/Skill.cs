using System;
using System.Collections.Generic;
using UnityEngine;

//[Tool]
public partial class Skill : MonoBehaviour {
    public CombatManager Combat => CombatManager.Instance;
    public SkillType Type => type;
    public SkillTarget Target => target;
    public int SkillPointForActivation => skillPointForActivation;

    [SerializeField] SkillType type;
    [SerializeField] SkillTarget target;
    [SerializeField] int skillPointForActivation = 1;

    public enum SkillType { Attack, Passive, NonAttack }
    public enum SkillTarget { SingleEnemy, AllEnemies, Player }

    public virtual List<CombatAction> Activate(CharacterCombat caster, CharacterCombat target) {
        return null;
    }

    public virtual List<CombatAction> Activate(CharacterCombat caster, List<CharacterCombat> targets) {
        return null;
    }

    public virtual string GetDescription(CharacterCombat caster = null, CharacterCombat target = null) {
        return "Description";
    }

    public static Skill Instantiate<T>() where T : Skill {
        return Library.Instantiate<Skill>(typeof(T));
    }

    public static Skill GetRandom(bool attacks = true, bool skills = true, bool powers = true, bool refreshPool = true, List<float> rarityChances = null) {
        return SkillsRandomizer.Instance.Get(attacks, skills, powers, refreshPool, rarityChances);
    }

    public static Skill Instantiate(Type type) {
        return Library.Instantiate(type) as Skill;
    }
}