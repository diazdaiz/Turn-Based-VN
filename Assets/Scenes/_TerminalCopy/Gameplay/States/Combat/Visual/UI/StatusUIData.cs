
using UnityEngine;

public partial class StatusUIData : MonoBehaviour {
    public Sprite StatusSprite => useCardImageSprite ? CardSpriteVisual.CardSprite : statusSprite;
    public Sprite StatusDescription => useCardImageSprite ? CardSpriteVisual.CardSprite : statusSprite;

    CardVisual CardSpriteVisual;
    //CardVisual cardSpriteVisual {
    //    get {
    //        if()
    //    }
    //}

    CardVisual CardDescriptionVisual;
    CardVisual cardDescriptionVisual;

    // [EnableIf(nameof(notUseCardImageSprite))]
    [SerializeField] Sprite statusSprite;
    [SerializeField] bool useCardImageSprite;
    bool notUseCardImageSprite => !useCardImageSprite;
    // [ShowIf(nameof(useCardImageSprite))]
    [SerializeField] Card cardForImageSprite;

    // [EnableIf(nameof(notUseCardDescription))]
    [SerializeField] string description;
    [SerializeField] bool useCardDescription;
    bool notUseCardDescription => !useCardDescription;
    // [ShowIf(nameof(useCardDescription))]
    [SerializeField] Card cardForDescription;


    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
