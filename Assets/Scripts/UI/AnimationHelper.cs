using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class AnimationHelper : MonoBehaviour
{
    private readonly static int[] animations =
    {
        Animator.StringToHash("Idle 1"),
        Animator.StringToHash("Idle 2"),
        Animator.StringToHash("Idle 3"),
        Animator.StringToHash("Run Forward"),
        Animator.StringToHash("Run Backward"),
        Animator.StringToHash("Run Left"),
        Animator.StringToHash("Run Right"),
        Animator.StringToHash("Shoot"),
        Animator.StringToHash("Reload"),
        Animator.StringToHash("Jump Start"),
        Animator.StringToHash("Jump In Air"),
        Animator.StringToHash("Jump Land"),
        Animator.StringToHash("Death"),
        Animator.StringToHash("None"),
        
        //Animator.StringToHash(),
    };

    private Animator mAnimator;
    private Animations[] currentAnimation;
    private bool[] layerLocked;
    private Action<int> DefaultAnimation;

    private void Initialize(int layers, Animations startingAnimation, Animator animator, Action<int> DefaultAnimation)
    {
        layerLocked = new bool[layers];
        currentAnimation = new Animations[layers];
        this.mAnimator = animator;
        this.DefaultAnimation = DefaultAnimation;

        for (int i = 0; i < layers; i++)
        {
            layerLocked[i] = false;
            currentAnimation[i] = startingAnimation;
        }

    }


}

public enum Animations
{
    IDLE_1,
    IDLE_2, 
    IDLE_3,
    RUN_FORWARD,
    RUN_BACKWARD,
    RUN_LEFT,
    RUN_RIGHT,
    SHOOT,
    RELOAD,
    JUMP_START,
    JUMP_IN_AIR,
    JUMP_LAND,
    DEATH,
    NONE,

    FADE_IN, 
    FADE_OUT,
    WIPE_LEFT,
    WIPE_RIGHT,
    WIPE_UP,
    WIPE_DOWN,



}