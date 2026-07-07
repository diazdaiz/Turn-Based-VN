using TMPro;
using UnityEngine;

public partial class EnergyHUD : MonoBehaviour {
    public CombatManager combat => CombatManager.Instance;
    TextMeshProUGUI tmpUGUI;

    private void Awake() {
        tmpUGUI = GetComponent<TextMeshProUGUI>();
    }

    public void Update() {
        tmpUGUI.text = combat.Energy.ToString();
    }
}
