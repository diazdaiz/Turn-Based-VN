using TMPro;
using UnityEngine;

//[Tool]
public partial class CardVisual : MonoBehaviour {
    public static string redColorCode => "c2252f";
    public static string normalColorCode => "ffffff";
    public static string greenColorCode => "24b62e";
    public static string brownColorCode => "ba7e26";

    [SerializeField] TextMeshProUGUI energyRequiredLabel;
    [SerializeField] TextMeshProUGUI nameLabel;
    [SerializeField] TextMeshProUGUI typeLabel;
    [SerializeField] TextMeshProUGUI descriptionLabel;
    [SerializeField] SpriteRenderer imageFrame;
    [SerializeField] Sprite commonFrame;
    [SerializeField] Sprite uncommonFrame;
    [SerializeField] Sprite rareFrame;

    Card card;
    #region (temporary) untuk icon status
    public Sprite CardSprite => cardSprite.sprite;
    [SerializeField] SpriteRenderer cardSprite;
    #endregion

    /// <summary>
    /// kalau number dibawah initial jadi merah, kalau diatas hijau
    /// </summary>
    /// <param name="normalValue"></param>
    /// <param name="currentValue"></param>
    /// <param name="flip">kalau di flip, kalau number dibawah initial jadi hijau, kalau diatas merah</param>
    /// <returns></returns>
    public static string NumberText(int normalValue, int currentValue, bool flip = false) {
        string colorCode = currentValue < normalValue ? redColorCode : currentValue == normalValue ? normalColorCode : greenColorCode;
        if (flip) {
            colorCode = currentValue < normalValue ? greenColorCode : currentValue == normalValue ? normalColorCode : redColorCode;
        }
        return $"[color=#{colorCode}]{currentValue}[/color]";
    }

    public static string BrownText(string text) {
        return $"[color=#{brownColorCode}]{text}[/color]";
    }

    /// <summary>
    /// Huruf depannya engga, contoh "AbcDefGh" jadi "Abc Def Gh"
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    string SeparateStringByCapital(string text) {
        for (int i = text.Length - 1; i > 0; i--) {
            if ("ABCDEFGHIJKLMNOPQRSTUVWXYZ".Contains(text[i])) {
                text = text.Insert(i, " ");
            }
        }
        return text;
    }

    public void Update() {
        if (card == null) {
            card = transform.parent.GetComponent<Card>();
            if (card == null) {
                return;
            }
        }
        if (imageFrame == null || nameLabel == null || descriptionLabel == null || typeLabel == null || card == null || energyRequiredLabel == null) {
            return;
        }

        //Energy
        energyRequiredLabel.gameObject.SetActive(card.Activation == Card.CardActivation.OnDiscarded);

        if (card.XEnergyForActivation) {
            energyRequiredLabel.text = "[outline_size=8][outline_color=black][font_size=36]x";
        }
        else {
            energyRequiredLabel.text = $"[outline_size=8][outline_color=black][font_size=36]{NumberText(card.InitialEnergyForActivation, card.EnergyForActivation, true)}";
        }

        //Name
        if (card.IsUpgraded) {
            Color color;
            ColorUtility.TryParseHtmlString($"#{greenColorCode}", out color);
            nameLabel.color = color;
            nameLabel.text = SeparateStringByCapital(card.GetType().Name) + "+";
        }
        else {
            Color color;
            ColorUtility.TryParseHtmlString($"#ffffff", out color);
            nameLabel.color = color;
            nameLabel.text = SeparateStringByCapital(card.GetType().Name);
        }

        //Image Frame
        if (card.Rarity == Card.CardRarity.Common) {
            imageFrame.sprite = commonFrame;
        }
        else if (card.Rarity == Card.CardRarity.Uncommon) {
            imageFrame.sprite = uncommonFrame;
        }
        else if (card.Rarity == Card.CardRarity.Rare) {
            imageFrame.sprite = rareFrame;
        }

        //Type
        typeLabel.text = card.Type.ToString();

        CombatManager combat = CombatManager.Instance;
        if (combat != null) {
            descriptionLabel.text = $"[outline_size=6][outline_color=black][font_size=16]{card.GetDescription(combat.Hero, combat.PotentialTarget)}";
        }
        else {
            descriptionLabel.text = $"[outline_size=6][outline_color=black][font_size=16]{card.GetDescription(null, null)}";
        }
    }
}
