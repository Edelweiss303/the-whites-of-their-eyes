using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeRollStateBehaviour : StateMachineBehaviour
{
    [Range(0.0f, 1.0f)]
    public float colliderSizeMultiplier;

    [Range(0.0f, 1.0f)]
    public float colliderYOffsetMultiplier;

    private CapsuleCollider _capsuleCollider;
    private float _capsuleColliderHeight;
    private Vector3 _capsuleColliderCenter;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _capsuleCollider = animator.GetComponent<CapsuleCollider>();
        if(_capsuleCollider == null)
        {
            Debug.LogError("Capsule Collider not found");
        }
        _capsuleColliderHeight = _capsuleCollider.height;
        _capsuleColliderCenter = _capsuleCollider.center;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _capsuleCollider.height = _capsuleColliderHeight + (animator.GetFloat("colliderCurve") * colliderSizeMultiplier);
        Vector3 newCenter = _capsuleColliderCenter;
        newCenter.y += (animator.GetFloat("colliderCurve") * colliderYOffsetMultiplier);
        _capsuleCollider.center = newCenter;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _capsuleCollider.center = new Vector3(0, 0.875f, 0);
        _capsuleCollider.height = 1.75f;
        animator.ResetTrigger("dodgeRoll");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
