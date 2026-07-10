using TMPro;
using UnityEngine;

public class SkillPointUI : MonoBehaviour {
    [SerializeField] TextMeshProUGUI spTMP;

    private void Update() {
        spTMP.text = "Skill Point: " + CombatManager.Instance.SkillPoint.ToString();
    }
}
