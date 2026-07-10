using Live2D.Cubism.Framework.Expression;
using UnityEngine;

public class TestPoseAndExpression : MonoBehaviour {
    [SerializeField] CubismExpressionController expressionController;
    [SerializeField] Animator animator;

    void Start() {

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            expressionController.CurrentExpressionIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            expressionController.CurrentExpressionIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            expressionController.CurrentExpressionIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            expressionController.CurrentExpressionIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            expressionController.CurrentExpressionIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            expressionController.CurrentExpressionIndex = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            expressionController.CurrentExpressionIndex = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            expressionController.CurrentExpressionIndex = 7;
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            animator.Play("mtn_00");
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            animator.Play("mtn_01");
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            animator.Play("mtn_02");
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            animator.Play("mtn_03");
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            animator.Play("mtn_04");
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            animator.Play("mtn_05");
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            animator.Play("mtn_06");
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            animator.Play("mtn_07");
        }
    }
}
