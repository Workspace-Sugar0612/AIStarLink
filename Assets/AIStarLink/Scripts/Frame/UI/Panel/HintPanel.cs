using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintPanel : MonoBehaviour
{
    public TMP_Text contentTX;
    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetContent(string content)
    {
        contentTX.text = content;
    }

    public void SetCanvasGroup(float alpha, float duration, Action<HintPanel> destroy)
    {
        canvasGroup.DOFade(alpha, duration).SetDelay(1f).OnComplete(() => 
        {
            if (destroy != null)
                destroy(this); 

            transform.DOLocalMoveY(Screen.height / 2.0f + 50f, 0.5f);
        });
        //Destroy(this);
    }
}
