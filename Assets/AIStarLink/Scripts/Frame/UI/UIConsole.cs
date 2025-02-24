using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConsole : MonoBehaviour
{
    private static Dictionary<string, BasePanel> m_PanelList = new Dictionary<string, BasePanel>();

    public GameObject hintTip;
    public Transform tipParent;

    Vector3 tipStartPos = new Vector3();
    Vector3 tipEndPos = new Vector3();

    private PoolList<HintPanel> hintPool = new PoolList<HintPanel>();

    public void Awake()
    {
        m_PanelList = new Dictionary<string, BasePanel>();
        tipStartPos = new Vector3(0, 28, 0);
        tipEndPos = new Vector3(0, -90, 0);
        hintPool = new PoolList<HintPanel>();
    }

    public void Start()
    {
        hintPool.AddListener(InstanceHintTip);
    }

    public T FindPanel<T>() where T : BasePanel
    {
        foreach (var panel in m_PanelList.Values)
        {
            if (panel is T)
            {
                return panel as T;
            }
        }
        return null;
    }

    public void AddList(string name, BasePanel panel)
    {
        m_PanelList.Add(name, panel);
    }

    public HintPanel InstanceHintTip()
    {
        GameObject go = Instantiate(hintTip, tipParent);
        HintPanel hintPanel = go.GetComponent<HintPanel>();
        return hintPanel;
    }

    public void ShowHintTip(string content, float duration)
    {
        HintPanel hintPanel = hintPool.Create("HintTip");
        hintPanel.canvasGroup.DOFade(1.0f, 0f);
        hintPanel.SetContent(content);
        hintPanel.transform.DOLocalMoveY(Screen.height / 2.0f - 90.0f, 0.5f).SetEase(Ease.OutBounce).OnComplete(() => 
        {
            hintPanel.SetCanvasGroup(0.0f, 1.0f, duration, hintPool.Destroy);
        });
    }
}