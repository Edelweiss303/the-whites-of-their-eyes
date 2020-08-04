﻿///-------------------------------------------------------------------------------------------------
// file: BossRepairTreeStateBehavior.cs
//
// author: Jesse Berube
// date: 2020-07-13
//
// summary: The repair tree state behavior. Once the boss is facing a fallen tree, they will repair them one at a time.
///-------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRepairTreeStateBehavior : StateMachineBehaviour
{
    private Animator _animator;
    private BossController _bossController;
    private bool _firstAnimationStarted;
    private bool _secondAnimationStarted;

    private float currentTime = 0.0f;

    private RopeAnchorPoint _tree;
    private PlayerGrapplingHook _hook;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_animator == null)
        {
            _animator = fsm.transform.parent.GetComponent<Animator>();
        }

        if (_bossController == null)
        {
            _bossController = fsm.GetComponentInParent<BossController>();
        }

        _tree = _bossController.fallenTreeList[0];
        _animator.SetTrigger("Heal Start");
        _hook = _bossController.player.hook;

    }


    public override void OnStateUpdate(Animator fsm, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_bossController.flinch)
        {
            fsm.SetTrigger("Flinch");
            _tree.PauseRotation();
            return;
        }

        // Check to see if point to repait animation is playing. When the animation is finished, change states.
        //if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Point To Repair"))
        //{
        //    _animationStarted = true;
        //}
        //else if (_animationStarted == true)
        //{
        //    _animationStarted = false;
        //    fsm.SetTrigger("Idle");
        //}

        //if (_firstAnimationStarted)
        //{
        //    currentTime += Time.deltaTime;

        //    if(currentTime >= repairTime)
        //    {
        //        _animator.SetTrigger("Heal End");
        //        currentTime = 0.0f;
        //    }
        //}

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Tree Heal Start"))
        {
            if (_firstAnimationStarted == false)
            {
                if (_hook.targetAnchor == null || _hook.targetAnchor.transform.root != _bossController.fallenTreeList[0].transform.root)
                {
                    _tree.ResetPull(_tree.pullTime);
                }

                _firstAnimationStarted = true;
            }
        }
        else if (_firstAnimationStarted == true)
        {
            _firstAnimationStarted = false;
        }

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Tree Heal End"))
        {
            if (_secondAnimationStarted == false)
            {
                _secondAnimationStarted = true;
            }
        }
        else if (_secondAnimationStarted == true)
        {
            _secondAnimationStarted = false;
            fsm.SetTrigger("Idle");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _firstAnimationStarted = false;
        _secondAnimationStarted = false;
    }

}
