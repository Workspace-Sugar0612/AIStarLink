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
        Normal
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
        animator.SetBool("Search", false);
        animator.SetBool("isNormal", true);
    }
}
