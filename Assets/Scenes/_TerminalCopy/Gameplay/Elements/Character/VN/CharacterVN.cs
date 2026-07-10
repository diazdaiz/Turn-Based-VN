using UnityEngine;

public class CharacterVN : MonoBehaviour {
    public string DisplayName => displayName;
    [SerializeField] string displayName = "name";
    [SerializeField] CharacterVNExpression expression;
    [SerializeField] CharacterLive2DModelManager live2DModelManager;
    public enum CharacterVNExpression { Neutral, Happy, Confuse, Angry }

    public void SetExpression(CharacterVNExpression expression, bool triggerAnimation = false, int pos = 0) {
        this.expression = expression;
        if (live2DModelManager != null) {
            live2DModelManager.ChangeExpression(expression, triggerAnimation);
        }
        Transform live2DModel = live2DModelManager.transform.GetChild(0);
        live2DModel.transform.localPosition = new Vector3(200 + 4.5f * pos, -1, 0);
    }

    public CharacterVNExpression GetExpression() {
        return expression;
    }
}
