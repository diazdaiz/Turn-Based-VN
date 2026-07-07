using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public partial class HealthHUD : MonoBehaviour {
    CharacterCombat character;
    [SerializeField] SpriteRenderer hpBar;
    [SerializeField] SpriteRenderer blockIcon;
    [SerializeField] SpriteRenderer blockOutline;
    [SerializeField] TextMeshProUGUI hpLabel;
    [SerializeField] TextMeshProUGUI blockLabel;
    [SerializeField] GameObject statusHUDPrefab;
    [SerializeField] GameObject statusesHolder;

    [SerializeField] Sprite naNStatusSprite;
    [SerializeField] Sprite weakStatusSprite;
    [SerializeField] Sprite strengthStatusSprite;
    [SerializeField] Sprite shackledStatusSprite;
    [SerializeField] Sprite dexterityStatusSprite;
    [SerializeField] Sprite frailStatusSprite;
    [SerializeField] Sprite vulnerableStatusSprite;
    [SerializeField] Sprite ritualStatusSprite;
    [SerializeField] Sprite platingStatusSprite;
    [SerializeField] Sprite poisonStatusSprite;
    [SerializeField] Sprite regenerateStatusSprite;
    [SerializeField] Sprite intangibleStatusSprite;
    [SerializeField] Sprite aThousandCutsStatusSprite;
    [SerializeField] Sprite accuracyStatusSprite;
    [SerializeField] Sprite afterImageStatusSprite;
    [SerializeField] Sprite blurStatusSprite;
    [SerializeField] Sprite bulletTimeStatusSprite;
    [SerializeField] Sprite burstStatusSprite;
    [SerializeField] Sprite caltropsStatusSprite;
    [SerializeField] Sprite chokeStatusSprite;
    [SerializeField] Sprite corpseExplosionStatusSprite;
    [SerializeField] Sprite gainBlocksNextTurnSprite;
    [SerializeField] Sprite doppelgangerStatusSprite;
    [SerializeField] Sprite envenomStatusSprite;
    [SerializeField] Sprite nightmareStatusSprite;
    [SerializeField] Sprite noAttackStatusSprite;
    [SerializeField] Sprite gainEnergiesNextTurnSprite;
    [SerializeField] Sprite infiniteBladesStatusSprite;
    [SerializeField] Sprite noxiusFumesStatusSprite;
    [SerializeField] Sprite phantasmalKillerStatusSprite;
    [SerializeField] Sprite doubleAttackDamageStatusSprite;
    [SerializeField] Sprite gainDrawsCardsNextTurnSprite;
    [SerializeField] Sprite toolsOfTheTradeStatusSprite;
    [SerializeField] Sprite vigorStatusSprite;
    [SerializeField] Sprite wellLaidPlansStatusSprite;
    [SerializeField] Sprite wraithFormStatusSprite;
    [SerializeField] Sprite curlUpStatusSprite;
    [SerializeField] Sprite enrageStatusSprite;

    Dictionary<Type, Sprite> statusesSprite;
    Dictionary<Type, StatusHUD> statusesHUD;

    private void Awake() {
        character = GetComponentInParent<CharacterCombat>();
    }

    public void Start() {
        statusesHUD = new();
        for (int i = statusesHolder.transform.childCount - 1; i >= 0; i--) {
            Destroy(statusesHolder.transform.GetChild(i));
        }
        statusesSprite = new() {
            { typeof(Status.Weak), weakStatusSprite},
            { typeof(Status.Strength), strengthStatusSprite},
            { typeof(Status.Shackled), shackledStatusSprite},
            { typeof(Status.Dexterity), dexterityStatusSprite},
            { typeof(Status.Frail), frailStatusSprite},
            { typeof(Status.Vulnerable), vulnerableStatusSprite},
            { typeof(Status.Ritual), ritualStatusSprite},
            { typeof(Status.Plating), platingStatusSprite},
            { typeof(Status.Poison), poisonStatusSprite},
            { typeof(Status.Regenerate), regenerateStatusSprite},
            { typeof(Status.Intangible), intangibleStatusSprite},
            { typeof(Status.AThousandCuts), aThousandCutsStatusSprite},
            { typeof(Status.Accuracy), accuracyStatusSprite},
            { typeof(Status.AfterImage), afterImageStatusSprite},
            { typeof(Status.Blur), blurStatusSprite},
            { typeof(Status.BulletTime), bulletTimeStatusSprite},
            { typeof(Status.Burst), burstStatusSprite},
            { typeof(Status.Caltrops), caltropsStatusSprite},
            { typeof(Status.Choke), chokeStatusSprite},
            { typeof(Status.CorpseExplosion), corpseExplosionStatusSprite},
            { typeof(Status.GainBlocksNextTurn), gainBlocksNextTurnSprite},
            { typeof(Status.Envenom), envenomStatusSprite},
            { typeof(Status.Nightmare), nightmareStatusSprite},
            { typeof(Status.NoAttack), noAttackStatusSprite},
            { typeof(Status.GainEnergiesNextTurn), gainEnergiesNextTurnSprite},
            { typeof(Status.InfiniteBlades), infiniteBladesStatusSprite},
            { typeof(Status.NoxiusFumes), noxiusFumesStatusSprite},
            { typeof(Status.PhantasmalKiller), phantasmalKillerStatusSprite},
            { typeof(Status.DoubleAttackDamage), doubleAttackDamageStatusSprite},
            { typeof(Status.DrawCardsNextTurn), gainDrawsCardsNextTurnSprite},
            { typeof(Status.ToolsOfTheTrade), toolsOfTheTradeStatusSprite},
            { typeof(Status.Vigor), vigorStatusSprite},
            { typeof(Status.WellLaidPlans), wellLaidPlansStatusSprite},
            { typeof(Status.WraithForm), wraithFormStatusSprite},
            { typeof(Status.CurlUp), curlUpStatusSprite},
            { typeof(Status.Enrage), enrageStatusSprite},
        };
    }

    public void Update() {
        #region Hp & Block
        hpLabel.text = $"{character.HP}/{character.MaxHealth}";
        if (character.MaxHealth == 0) return;
        hpBar.transform.localScale = new Vector3((float)character.HP / character.MaxHealth * 0.975f, hpBar.transform.localScale.y, hpBar.transform.localScale.z);
        if (character.block > 0) {
            blockIcon.gameObject.SetActive(true);
            blockOutline.gameObject.SetActive(true);
            blockLabel.text = $"{character.block}";
        }
        else {
            blockIcon.gameObject.SetActive(false);
            blockOutline.gameObject.SetActive(false);
            blockLabel.text = "";
        }
        #endregion

        #region Add Status UI
        int n = 0;
        foreach (Type type in character.Statuses.Keys) {
            Status status = character.Statuses[type];
            //block skip, udah sepaket sama hp disebelumnya
            if (status.GetType() == typeof(Status.Block)) {
                continue;
            }

            //add kalau blom ada
            if (!statusesHUD.ContainsKey(type)) {
                StatusHUD statusHUD = Instantiate(statusHUDPrefab).GetComponent<StatusHUD>();
                statusHUD.transform.parent = statusesHolder.transform;
                statusesHUD.Add(type, statusHUD);
            }
            // StatusUI statusUI = statusesUI[type].GetComponent<StatusUI>();

            if (statusesSprite.ContainsKey(type)) {
                statusesHUD[type].StatusSprite.sprite = statusesSprite[type];
            }
            else {
                statusesHUD[type].StatusSprite.sprite = naNStatusSprite;
            }
            statusesHUD[type].transform.position = new Vector3(n * 0.32f, 0, 0f);

            string stack = "";
            if (status.IsStackable) {
                if (status.Stack < 0) {
                    stack += "-";
                }
                stack += Mathf.Abs(status.Stack).ToString();
            }

            statusesHUD[type].StackOrTurnLabel.text = status.IsStackable ? status.Stack < 0 ? "-" + status.Stack.ToString() : status.Stack.ToString() : "";
            //check if status have stack / turn, set statusesUI[type].stackOrTurnTMP
            n += 1;
        }
        #endregion

        #region Remove Status HUD
        List<Type> markToRemove = new List<Type>();
        foreach (Type type in statusesHUD.Keys) {
            if (!character.Statuses.ContainsKey(type)) {
                markToRemove.Add(type);
            }
        }
        for (int i = 0; i < markToRemove.Count; i++) {
            GameObject gameObject = statusesHUD[markToRemove[i]].gameObject;
            statusesHUD.Remove(markToRemove[i]);
            Destroy(gameObject);
        }
        #endregion
    }
}
