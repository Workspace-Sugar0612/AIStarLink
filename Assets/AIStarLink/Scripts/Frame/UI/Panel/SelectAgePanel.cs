using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectAgePanel : BasePanel
{
    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }
}
