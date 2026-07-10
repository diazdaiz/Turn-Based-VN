using Live2D.Cubism.Core;
using Live2D.Cubism.Framework.Expression;
using System.Collections.Generic;
using UnityEngine;
using static CharacterVN;

[ExecuteAlways]
public class CharacterLive2DModelManager : MonoBehaviour {
    [SerializeField] CubismModel model;
    [SerializeField] List<Live2DExpressionKey> live2DExpressionKeys;
    [System.Serializable]
    class Live2DExpressionKey {
        public CharacterVNExpression expression;
        public int expressionId;
        public string animationClipName;
    }
    CubismExpressionController expressionController;
    Animator animator;
    Dictionary<CharacterVNExpression, int> expressionsId;
    Dictionary<CharacterVNExpression, string> expressionsAnimationClipName;

    CharacterVNExpression expression;

    private void Awake() {
        expressionController = model.transform.GetComponent<CubismExpressionController>();
        animator = model.transform.GetComponent<Animator>();
    }

    void Start() {
        expressionsId = new();
        expressionsAnimationClipName = new();
        for (int i = 0; i < live2DExpressionKeys.Count; i++) {
            expressionsId.Add(live2DExpressionKeys[i].expression, live2DExpressionKeys[i].expressionId);
            expressionsAnimationClipName.Add(live2DExpressionKeys[i].expression, live2DExpressionKeys[i].animationClipName);
        }
        ChangeExpression(CharacterVNExpression.Neutral);
    }

    public void ChangeExpression(CharacterVNExpression expression, bool triggerAnimation = false) {
        expressionController.CurrentExpressionIndex = expressionsId[expression];
        if (triggerAnimation) {
            animator.Play(expressionsAnimationClipName[expression]);
        }
    }

    //look left, middle, right



    void Update() {
        transform.rotation = Quaternion.identity;
    }
}
