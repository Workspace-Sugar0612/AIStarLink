using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISearchAnimController : MonoBehaviour
{
    public enum AnimState
    {
        None,
        Search,
        Changed,
        Normal,
        ChangedBack
    }

    public Animator animator;

    public void SetAnimState(AnimState state)
    {
        switch (state)
        {
            case AnimState.Search:
                Search();
                break;
            case AnimState.Changed:
                Changed();
                break;
            case AnimState.Normal:
                Normal();
                break;
            case AnimState.ChangedBack:
                ChangedBack();
                break;
            default:
                break;
        }
    }

    private void Search()
    {
        animator.SetBool("Changed", false);
        animator.SetBool("Search", true);
    }

    private void Changed()
    {
        animator.SetBool("isNormal", false);
        animator.SetBool("Changed", true);
    }

    private void Normal()
    {
        animator.SetBool("Changed", false);
        animator.SetBool("Search", false);
        animator.SetBool("isNormal", true);
    }

    private void ChangedBack()
    {
        // animator.speed = -1.0f;
        animator.SetBool("isNormal", false);
        animator.SetBool("Search", false);
        animator.SetBool("Changed", true);
    }
}
