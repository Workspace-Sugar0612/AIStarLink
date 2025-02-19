using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISearchAnimController : MonoBehaviour
{
   
    public Animator animator;

    public void Search()
    {
        animator.SetBool("Changed", false);
        animator.SetBool("Search", true);
    }
    
    public void Changed()
    {
        animator.SetBool("isNormal", false);
        animator.SetBool("Changed", true);
    }

    public void Normal()
    {
        animator.SetBool("Search", false);
        animator.SetBool("isNormal", true);
    }
}
