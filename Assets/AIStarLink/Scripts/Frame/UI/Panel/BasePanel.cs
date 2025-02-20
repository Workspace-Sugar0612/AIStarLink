using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public enum ShowMethod
    {
        Neno,
        Progressive,
        Traverse
    }

    protected UIConsole _uiConsole;
    public string panelName;

    public virtual void Awake()
    {
        _uiConsole = (UIConsole)FindObjectOfType(typeof(UIConsole));
    }

    public virtual void Start()
    {
        _uiConsole.AddList(panelName, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetShow(ShowMethod method, object[] param)
    {
        if (method == ShowMethod.Progressive) { Progressive(param); }
        else if (method == ShowMethod.Traverse) { TraverseShow(param); }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    private void Progressive(object[] param)
    {
        CanvasGroup canvasGroup = param[0] as CanvasGroup;
        float alpha = (float)param[1];
        float duration = (float)param[2];
        canvasGroup.DOFade(alpha, duration);
    }

    private void TraverseShow(object[] param)
    {
        Transform trans = param[0] as Transform;
        Vector3 pos = (Vector3)param[1];
        float duration = (float)param[2];
        trans.DOMove(pos, duration);
    }

    public virtual void Active(bool active) 
    {
    
    }
}
