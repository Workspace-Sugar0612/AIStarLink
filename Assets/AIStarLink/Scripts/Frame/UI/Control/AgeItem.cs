using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class AgeItem : MonoBehaviour
{
    public Transform rootTransform;
    public Image imgIcon;
    public TMP_Text title;

    private LoopListViewItem2 _item;

    public void Start()
    {
        _item = GetComponent<LoopListViewItem2>();
    }

    public void Update()
    {
  
    }

    public void SetText(string text)
    {
        title.text = text;
    }

    public void SetScale(float scale)
    {
        rootTransform.GetComponent<CanvasGroup>().alpha = scale;
        rootTransform.transform.localScale = new Vector3(1.0f, scale, 1.0f);
    }
}
