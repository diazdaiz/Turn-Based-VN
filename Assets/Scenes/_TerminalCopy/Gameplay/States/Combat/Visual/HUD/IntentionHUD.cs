using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class IntentionHUD : MonoBehaviour {
    //public TextMeshProUGUI intentionTMP;
    Enemy enemy;
    CombatManager combat => CombatManager.Instance;
    [SerializeField] SpriteRenderer intentionSprite;
    [SerializeField] TextMeshProUGUI attackLabel;
    [SerializeField] Sprite attackTexture;
    [SerializeField] Sprite blockTexture;
    [SerializeField] Sprite buffTexture;
    [SerializeField] Sprite debuffTexture;
    [SerializeField] Sprite attackBlockTexture;
    [SerializeField] Sprite attackBuffTexture;
    [SerializeField] Sprite attackDebuffTexture;
    [SerializeField] Sprite blockBuffTexture;
    [SerializeField] Sprite blockDebuffTexture;
    Dictionary<Type, Sprite> IntentionsTexture;

    private void Awake() {
        enemy = transform.GetComponentInParent<Enemy>();
    }

    public void Start() {
        IntentionsTexture = new() {
            {typeof(Intention.Attack), attackTexture},
            {typeof(Intention.Block), blockTexture},
            {typeof(Intention.Buff), buffTexture},
            {typeof(Intention.Debuff), debuffTexture},
            {typeof(Intention.AttackBlock), attackBlockTexture},
            {typeof(Intention.AttackBuff), attackBuffTexture},
            {typeof(Intention.AttackDebuff), attackDebuffTexture},
            {typeof(Intention.BlockBuff), blockBuffTexture},
            {typeof(Intention.BlockDebuff), blockDebuffTexture}
        };
    }

    public void Update() {
        Intention intention = enemy.intention;
        if (intention == null) {
            return;
        }

        //Intention texture
        intentionSprite.sprite = IntentionsTexture[intention.GetType()];

        //Intention attack label
        if (intention is Intention.Attack || intention is Intention.AttackBlock || intention is Intention.AttackBuff || intention is Intention.AttackDebuff) {
            int damage = 0;
            if (intention is Intention.Attack attack) {
                damage = attack.damage;
            }
            if (intention is Intention.AttackBlock attackBlock) {
                damage = attackBlock.damage;
            }
            if (intention is Intention.AttackBuff attackBuff) {
                damage = attackBuff.damage;
            }
            if (intention is Intention.AttackDebuff attackDebuff) {
                damage = attackDebuff.damage;
            }
            attackLabel.text = combat.CalculateAttack(enemy, combat.Hero, damage).ToString();
        }
        else {
            attackLabel.text = "";
        }
    }
}
