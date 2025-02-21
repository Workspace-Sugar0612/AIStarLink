using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectAgePanel : BasePanel
{
    public Button submitButton;
    public Button normalButton;
    public CanvasGroup canvasGroup;

    public List<string> itemNameList;
    public GameObject ageItem;
    public Transform ageItemParent;

    private List<AgeItem> ageItems;
    private UISearchAnimController _uiSearchAnimController;

    public override void Awake()
    {
        base.Awake();
        ageItems = new List<AgeItem>();

        if (!_uiSearchAnimController) _uiSearchAnimController = (UISearchAnimController)FindObjectOfType(typeof(UISearchAnimController));
    }

    public override void Start()
    {
        base.Start();
        canvasGroup.alpha = 0.0f;

        normalButton.onClick.AddListener(onClickedNormalBtn);
        submitButton.onClick.AddListener(onClickedSubmitBtn);

        InitItem();
    }

    private void InitItem()
    {
        ageItems.Clear();
        for (int i = 0; i <¡¡itemNameList.Count; ++i)
        {
            GameObject go = Instantiate(ageItem, ageItemParent);
            AgeItem item = go.GetComponent<AgeItem>();
            item.Init(itemNameList[i], $"Toggle_{i}");
            ageItems.Add(item);
        }
        if (ageItems.Count > 0) ageItems[0].toggle.isOn = true;
    }

    public override void Active(bool active)
    {
        if (active) { SetShow(ShowMethod.Progressive, new object[] { canvasGroup, 1.0f, 0.6f }); }
        else { SetShow(ShowMethod.Progressive, new object[] { canvasGroup, 0.0f, 0.6f }); }
    }

    private void onClickedNormalBtn()
    {
        Staticvariables.isSearch = true;
        Active(false);
        _uiSearchAnimController.SetAnimState(UISearchAnimController.AnimState.Normal);
    }

    private void onClickedSubmitBtn()
    {
        Staticvariables.isSearch = true;
        Active(false);
        _uiSearchAnimController.SetAnimState(UISearchAnimController.AnimState.Changed);
    }
}
