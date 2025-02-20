using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectAgePanel : BasePanel
{
    public Button submitButton;
    public Button normalButton;
    public CanvasGroup canvasGroup;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Start()
    {
        base.Start();
        //gameObject.SetActive(false);
        canvasGroup.alpha = 0.0f;
    }

    public override void Active(bool active)
    {
        if (active) { SetShow(ShowMethod.Progressive, new object[] { canvasGroup, 1.0f, 0.6f }); }
        else { SetShow(ShowMethod.Progressive, new object[] { canvasGroup, 0.0f, 0.6f }); }
    }
}
