using DG.Tweening;
using System;
using System.Collections;
using System.Drawing;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static SphereController;
using Color = UnityEngine.Color;

public class SphereController : MonoBehaviour
{
    // 用于在Update里面判断当前的位置状态
    // 避免在同一个范围状态中反复只能相应操作
    public enum SpherePosState
    {
        None,
        OutSide,
        InSide
    }
    private SpherePosState _spherePosState = SpherePosState.None;

    private Transform _parentTrans;
    private TMP_Text _label;
    private SphereManager _sphereManager;
    private UIConsole _uiConsole;

    private Color _normalColor = new Color(44.0f / 255.0f, 79.0f / 255.0f, 255.0f / 255.0f, 85.0f / 255.0f);
    private Color _clickedColor = new Color(255.0f / 255.0f, 177.0f / 255.0f, 67.0f / 255.0f, 85.0f / 255.0f);
    private Color _farColor = new Color(5.0f / 255.0f, 11.0f / 255.0f, 41.0f / 255.0f, 85.0f / 255.0f);

    private Material outerRingMat = null;
    private Material innerRingMat = null;
    private bool isSelected = false;
    public bool isGetKeyword = false;
    public float dist = 0.0f;

    public void Awake()
    {
        _label = GetComponentInChildren<TMP_Text>();
        _sphereManager = (SphereManager)FindObjectOfType(typeof(SphereManager));
        _uiConsole = (UIConsole)FindObjectOfType(typeof(UIConsole));
    }

    public void Start()
    {

    }

    public void Update()
    {
        dist = Vector3.Distance(Camera.main.transform.position, this.transform.position);
        UpdateTextControl();
        UpdateMatColor();
        UpdatePosState();
    }

    private void UpdateTextControl()
    {
        if (dist <= _sphereManager.limitDistance)
        {
            if (!isGetKeyword && _spherePosState != SpherePosState.InSide)
            {
                //_label.gameObject.SetActive(true);

                StartCoroutine(_sphereManager.AssignKeyword(SetLableText));
                _label.DOColor(Color.white, 3.0f);
                _label.DOFade(0.7f, 3.0f).OnComplete(() => {  });
            }
        }
        else
        {
            if (isGetKeyword && _spherePosState != SpherePosState.OutSide)
            {
                isGetKeyword = false;
                _label.DOColor(Color.white, 3.0f);
                _label.DOFade(0, 3.0f);
            }
        }
    }

    private void UpdateMatColor()
    {
        if (dist <= _sphereManager.limitDistance && !isSelected && _spherePosState != SpherePosState.InSide)
            SetMatColor(_normalColor, 3.0f);
        else if (dist > _sphereManager.limitDistance && !isSelected && _spherePosState != SpherePosState.OutSide)
            SetMatColor(_farColor, 3.0f);
    }

    // 更新PosState
    private void UpdatePosState()
    {
        if (dist <= _sphereManager.limitDistance && _spherePosState != SpherePosState.InSide)
            _spherePosState = SpherePosState.InSide;
        else if (dist > _sphereManager.limitDistance && _spherePosState != SpherePosState.OutSide)
            _spherePosState = SpherePosState.OutSide;
    }

    public void OnClickedSphere()
    {
        DOKillMat();
        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
        if (!isSelected && dist <= _sphereManager.limitDistance)
        {
            isSelected = true;
            transform.SetParent(Camera.main.transform);
            SetMatColor(_clickedColor);

            // SelectAgePanel saPanel = _uiConsole.FindPanel<SelectAgePanel>();
            // saPanel.gameObject.SetActive(true);
        }
        else if (isSelected && dist <= _sphereManager.limitDistance)
        {
            isSelected = false;
            transform.SetParent(_sphereManager.parentTrans);
            SetMatColor(_normalColor);
        }
    }

    public void SetMatColor(Color color, float sec = 0.3f)
    {
        if (outerRingMat == null && innerRingMat == null)
        {
            MeshRenderer[] renderArr = transform.gameObject.GetComponentsInChildren<MeshRenderer>();
            outerRingMat = renderArr[0].material;
            innerRingMat = renderArr[1].material;
        }

        outerRingMat.DOColor(color, sec).SetEase(Ease.Linear);
        innerRingMat.DOColor(color, sec).SetEase(Ease.Linear);
    }

    private void SetLableText(string text)
    {
        isGetKeyword = true;
        _label.text = text;
    }

    // Kill the previous dotween task
    public void DOKillMat()
    {
        if (outerRingMat) outerRingMat.DOKill();
        if (innerRingMat) innerRingMat.DOKill();
    }    
}
