

using TMPro;
using UnityEngine;

public partial class DrawHUD : MonoBehaviour {
    public CombatManager combat => CombatManager.Instance;
    TextMeshProUGUI tmpUGUI;

    private void Awake() {
        tmpUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Update() {
        tmpUGUI.text = combat.DrawPile.Count.ToString();
    }
}
