using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationState : MonoBehaviour
{
    [Header("Components")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    public string currentState;

    public float currentFrame;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        // HACK: 'Game object with animator is inactive' after dying
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        SetCurrentClipCompletionProgress();
    }

    public void SetAnimationState(string animationState)
    {
        // ?
        if (currentState == animationState) return;

        currentState = animationState;
        // play animation
        animator.Play(animationState);
    }

    public float GetClipLength(string clipName)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        if (ac != null)
        {
            //Get Animator controller
            for (int i = 0; i < ac.animationClips.Length; i++)
            {
                if (ac.animationClips[i].name == clipName)
                {
                    return ac.animationClips[i].length;
                }

            }

            return 0f;
        }
        else
        {
            // FIXME: should be cannot access animator exception;
            return 0f;
        }

    }

    private void SetCurrentClipCompletionProgress()
    {

        // 1 being completed
        currentFrame = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1) * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;

        //return animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}