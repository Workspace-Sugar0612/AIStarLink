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

    private Dictionary<string, string> m_AgeCn2En = new Dictionary<string, string>() {{"入门级", "Entry level" }, {"进阶级", "Advanced level" }, {"专业级", "Professional level" }, {"应用级", "Application level" }, {"前沿级", "Frontier level" } }; // 询问等级中转英
    private string _keyword = string.Empty;
    private string _level = string.Empty;
    private AIChatAPI _aiChatAPI;

    private string _usrMessage = string.Empty; //用户想传递给aichat的内容
    public override void Awake()
    {
        base.Awake();
        ageItems = new List<AgeItem>();

        if (!_uiSearchAnimController) _uiSearchAnimController = (UISearchAnimController)FindObjectOfType(typeof(UISearchAnimController));
        if (!_aiChatAPI) _aiChatAPI = (AIChatAPI)FindObjectOfType(typeof(AIChatAPI));
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
        for (int i = 0; i <　itemNameList.Count; ++i)
        {
            GameObject go = Instantiate(ageItem, ageItemParent);
            AgeItem item = go.GetComponent<AgeItem>();
            item.Init(itemNameList[i], $"Toggle_{i}");
            ageItems.Add(item);
        }
        if (ageItems.Count > 0) ageItems[0].toggle.isOn = true;
    }

    /// <summary>
    /// 激活窗口，传递内容
    /// </summary>
    /// <param name="active"></param>
    /// <param name="content"></param>
    public override void Active(bool active)
    {
        if (active) { SetShow(ShowMethod.Progressive, new object[] { canvasGroup, 1.0f, 0.6f }); }
        else { SetShow(ShowMethod.Progressive, new object[] { canvasGroup, 0.0f, 0.6f }); }
    }

    /// <summary>
    /// 打开窗口，并且把需要对话的内容传递给AICHAT
    /// </summary>
    /// <param name="active"></param>
    /// <param name="mess">要传递给aichat的内容</param>
    public void Open(string mess)
    {
        Active(true);
        _keyword = mess;
        _usrMessage = $"Please tell me some knowledge about {mess} in nuclear aspect.";
    }

    public void Hidden()
    {
        Active(false);
        _usrMessage = string.Empty;
    }

    private void onClickedNormalBtn()
    {
        _usrMessage += $"Applicable to public, please return to Chinese, thank you.";
        _aiChatAPI.SendOnDeepSeek(_usrMessage, _keyword);
        Active(false);
        _uiSearchAnimController.SetAnimState(UISearchAnimController.AnimState.Changed);
    }

    private void onClickedSubmitBtn()
    {
        _usrMessage += $"Applicable to {m_AgeCn2En[Staticvariables.level]}, please return to Chinese, thank you.";
        _aiChatAPI.SendOnDeepSeek(_usrMessage, _keyword);
        Active(false);
        _uiSearchAnimController.SetAnimState(UISearchAnimController.AnimState.Changed);
    }
}
