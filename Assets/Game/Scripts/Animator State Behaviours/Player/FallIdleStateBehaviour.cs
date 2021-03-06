using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallIdleStateBehaviour : StateMachineBehaviour
{
    public float groundCheckDistance;
    public Vector3 sphereCastStartOffset;

    public float softLandingTime;
    public float mediumLandingTime;

    [Range(0.0f, 1.0f)]
    public float colliderSizeMultiplier;
    [Range(0.0f, 1.0f)]
    public float colliderYOffsetMultiplier;

    private float _fallTime;
    private bool _canRoll;
    private float _capsuleColliderHeight;
    private Vector3 _capsuleColliderCenter;
    private CapsuleCollider _capsuleCollider;
    private LayerMask _layerMask = ~(1 << 8);

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _capsuleCollider = animator.GetComponent<CapsuleCollider>();

        _fallTime = 0.0f;
        _canRoll = animator.GetBool("canRoll");

        _capsuleColliderHeight = _capsuleCollider.height; 
        _capsuleColliderCenter = _capsuleCollider.center;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _fallTime += Time.deltaTime;
        animator.SetFloat("fallTime", _fallTime);

        // Change collider size and center based on animation curve
        _capsuleCollider.height = _capsuleColliderHeight + (animator.GetFloat("colliderCurve") * colliderSizeMultiplier);
        Vector3 newCenter = _capsuleColliderCenter;
        newCenter.y += (animator.GetFloat("colliderCurve") * colliderYOffsetMultiplier);
        _capsuleCollider.center = newCenter;

        if (Physics.SphereCast(animator.transform.position + sphereCastStartOffset, 0.4f, Vector3.down, out _, groundCheckDistance, _layerMask))
        {
            if (_fallTime < softLandingTime)
            {
                animator.SetFloat("fallSpeed", 0.0f);
            }
            else if(_fallTime < mediumLandingTime && _canRoll)
            {
                animator.SetFloat("fallSpeed", 0.5f);
            }
            else
            {
                animator.SetFloat("fallSpeed", 1.0f);
            }

            animator.SetTrigger("fallLand");
        }
    }


}
