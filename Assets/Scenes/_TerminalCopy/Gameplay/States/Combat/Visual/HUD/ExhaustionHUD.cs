using TMPro;
using UnityEngine;

public partial class ExhaustionHUD : MonoBehaviour {
    public CombatManager combat => CombatManager.Instance;
    TextMeshProUGUI tmpUGUI;
    SpriteRenderer spriteRenderer;

    private void Awake() {
        tmpUGUI = GetComponent<TextMeshProUGUI>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        if (combat.ExhaustedPile.Count > 0) {
            spriteRenderer.enabled = true;
        }
        else {
            spriteRenderer.enabled = false;
        }
        tmpUGUI.text = combat.ExhaustedPile.Count.ToString();
    }
}
