using System;
using System.Collections;
using System.Drawing;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

public class SphereController : MonoBehaviour
{
    private Transform _parentTrans;
    private TMP_Text _label;
    private SphereManager _sphereManager;

    private Color _normalColor = new Color(44.0f / 255.0f, 79.0f / 255.0f, 255.0f / 255.0f, 85.0f / 255.0f);
    private Color _clickedColor = new Color(255.0f / 255.0f, 177.0f / 255.0f, 67.0f / 255.0f, 85.0f / 255.0f);
    private Color _farColor = new Color(5.0f / 255.0f, 11.0f / 255.0f, 41.0f / 255.0f, 85.0f / 255.0f);

    private Material outerRingMat = null;
    private Material innerRingMat = null;
    private bool isSelect = false;
    public bool isGetKeyword = false;
    public float dist = 0.0f;

    public void Awake()
    {
        _label = GetComponentInChildren<TMP_Text>();
        _sphereManager = (SphereManager)FindObjectOfType(typeof(SphereManager));
    }

    public void Start()
    {

    }

    public void Update()
    {
        dist = Vector3.Distance(Camera.main.transform.position, this.transform.position);
        UpdateTextControl();
        UpdateMatColor();
    }

    private void UpdateTextControl()
    {
        if (dist <= _sphereManager.limitDistance)
        {
            if (!isGetKeyword)
            {
                StartCoroutine(_sphereManager.AssignKeyword(SetLableText));
                _label.gameObject.SetActive(true);
            }
        }
        else
        {
            if (isGetKeyword)
            {
                isGetKeyword = false;
                _label.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateMatColor()
    {
        if (dist <= _sphereManager.limitDistance)
            SetMatColor(_normalColor, 3.0f);
        else
            SetMatColor(_farColor, 3.0f);
    }

    public void OnClickedSphere()
    {
        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
        if (!isSelect && dist <= _sphereManager.limitDistance)
        {
            Debug.Log("Select Sphere!");
            transform.SetParent(null);
            SetMatColor(_clickedColor);
            isSelect = true;
        }
        else if (isSelect && dist <= _sphereManager.limitDistance)
        {
            Debug.Log("UnSelect Sphere!");
            transform.SetParent(_sphereManager.parentTrans);
            SetMatColor(_normalColor);
            isSelect = false;
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

        StartCoroutine(SetMatColorGradient(outerRingMat, color, sec));
        StartCoroutine(SetMatColorGradient(innerRingMat, color, sec));
        // SetMatColor(outerRingMat, color);
        // SetMatColor(innerRingMat, color);
        // outerRingMat.color = color;
        // innerRingMat.color = color;
    }

    public IEnumerator SetMatColorGradient(Material mat, Color newColor, float sec = 0.5f)
    {
        if (mat == null || mat.color == newColor) yield break;
        Color oldColor = mat.color;
        float rDiff = 0.0f, gDiff = 0.0f, bDiff = 0.0f, aDiff = 0.0f;
        rDiff = newColor.r - oldColor.r;
        gDiff = newColor.g - oldColor.g;
        bDiff = newColor.b - oldColor.b;
        aDiff = newColor.a - oldColor.a;

        float gradSec = sec / Mathf.Max(Mathf.Abs(rDiff), Mathf.Abs(gDiff), Mathf.Abs(bDiff), Mathf.Abs(aDiff));
        while (oldColor != newColor)
        {
            if (oldColor.r != newColor.r) oldColor.r += rDiff / (sec * 100.0f);
            if (oldColor.g != newColor.g) oldColor.g += gDiff / (sec * 100.0f);
            if (oldColor.b != newColor.b) oldColor.b += bDiff / (sec * 100.0f);
            if (oldColor.a != newColor.a) oldColor.a += aDiff / (sec * 100.0f);
            SetMatColor(mat, oldColor);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SetMatColor(Material mat, UnityEngine.Color color)
    {
        mat.color = color;
    }

    private void SetLableText(string text)
    {
        isGetKeyword = true;
        _label.text = text;
    }
}
