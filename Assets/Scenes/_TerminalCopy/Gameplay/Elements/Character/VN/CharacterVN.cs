using UnityEngine;

public class CharacterVN : MonoBehaviour {
    public string DisplayName => displayName;
    //Portrait
    //Live2D
    [SerializeField] string displayName = "name";
    [SerializeField] CharacterVNExpression expression;
    public enum CharacterVNExpression { Neutral, Happy, Confuse, Angry }

    void Start() {

    }

    void Update() {

    }
}
