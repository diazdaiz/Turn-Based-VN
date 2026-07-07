using TMPro;
using UnityEngine;

public partial class StatusHUD : MonoBehaviour {
    public SpriteRenderer StatusSprite => statusSprite;
    public TextMeshProUGUI StackOrTurnLabel => stackOrTurnLabel;

    [SerializeField] SpriteRenderer statusSprite;
    [SerializeField] TextMeshProUGUI stackOrTurnLabel;
}
