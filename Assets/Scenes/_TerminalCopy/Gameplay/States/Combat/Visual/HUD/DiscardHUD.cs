
using TMPro;
using UnityEngine;

public partial class DiscardHUD : MonoBehaviour {
    public CombatManager combat => CombatManager.Instance;

    TextMeshProUGUI tmpUGUI;

    private void Awake() {
        tmpUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Update() {
        tmpUGUI.text = combat.DiscardPile.Count.ToString();
    }
}
