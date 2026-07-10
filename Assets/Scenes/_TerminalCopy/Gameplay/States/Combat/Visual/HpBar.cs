using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HpBar : MonoBehaviour {
    CombatManager Combat => CombatManager.Instance;
    [SerializeField] RectTransform playerTeam1Rect;
    [SerializeField] TextMeshProUGUI playerTeam1Text;
    [SerializeField] Image playerTeam1Image;

    [SerializeField] RectTransform playerTeam2Rect;
    [SerializeField] TextMeshProUGUI playerTeam2Text;
    [SerializeField] Image playerTeam2Image;

    [SerializeField] RectTransform enemyTeam1Rect;
    [SerializeField] TextMeshProUGUI enemyTeam1Text;
    [SerializeField] Image enemyTeam1Image;

    [SerializeField] RectTransform enemyTeam2Rect;
    [SerializeField] TextMeshProUGUI enemyTeam2Text;
    [SerializeField] Image enemyTeam2Image;


    // Update is called once per frame
    void Update() {
        //ini udah yang penting ada, sangat engga rapih (deket deadline)
        if (Combat.PlayerTeam != null && Combat.PlayerTeam.Count > 0) {
            CharacterCombat character = Combat.PlayerTeam[0];
            playerTeam1Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)character.HP / (float)character.MaxHealth * 286.9f);
            playerTeam1Text.text = character.HP.ToString();
            playerTeam1Image.sprite = character.Portrait;

        }
        if (Combat.PlayerTeam != null && Combat.PlayerTeam.Count > 1) {
            CharacterCombat character = Combat.PlayerTeam[1];
            playerTeam2Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)character.HP / (float)character.MaxHealth * 286.9f);
            playerTeam2Text.text = character.HP.ToString();
            playerTeam2Image.sprite = character.Portrait;
        }
        if (Combat.EnemyTeam != null && Combat.EnemyTeam.Count > 0) {
            CharacterCombat character = Combat.EnemyTeam[0];
            enemyTeam1Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)character.HP / (float)character.MaxHealth * 286.9f);
            enemyTeam1Text.text = character.HP.ToString();
            enemyTeam1Image.sprite = character.Portrait;
        }
        if (Combat.EnemyTeam != null && Combat.EnemyTeam.Count > 1) {
            CharacterCombat character = Combat.EnemyTeam[1];
            enemyTeam2Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (float)character.HP / (float)character.MaxHealth * 286.9f);
            enemyTeam2Text.text = character.HP.ToString();
            enemyTeam2Image.sprite = character.Portrait;
        }
        if (Combat.EnemyTeam != null && Combat.EnemyTeam.Count == 1) {
            enemyTeam2Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
            enemyTeam2Text.text = 0.ToString();
            enemyTeam2Image.sprite = null;
        }
        if (Combat.EnemyTeam != null && Combat.PlayerTeam.Count == 1) {
            playerTeam2Rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
            playerTeam2Text.text = 0.ToString();
            playerTeam2Image.sprite = null;
        }
    }
}
