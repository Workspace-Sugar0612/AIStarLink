using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SphereController : MonoBehaviour
{
    private Transform _parentTrans;
    private TMP_Text _label;
    private SphereManager _sphereManager;

    private Color _normalColor = new Color(1.0f, 0.7f, 0.26f, 0.3f);
    private Color _clickedColor = new Color(0.32f, 0.6f, 0.8f, 0.3f);
    private Material outerRingMat = null;
    private Material innerRingMat = null;
    private bool isSelect = false;

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
        SetParamBaseOnDistance();
    }

    public void SetParamBaseOnDistance()
    {
        float dist = Vector3.Distance(Camera.main.transform.position, this.transform.position);
        if (dist <= _sphereManager.limitDistance) { _label.gameObject.SetActive(true); }
        else { _label.gameObject.SetActive(false); }
    }

    public void OnClickedSphere()
    {
        float dist = Vector3.Distance(Camera.main.transform.position, transform.position);
        if (!isSelect && dist <= _sphereManager.limitDistance)
        {
            transform.SetParent(null);
            SetMatColor(_clickedColor);
            isSelect = true;
        }
        else if (isSelect && dist <= _sphereManager.limitDistance)
        {
            transform.SetParent(_sphereManager.parentTrans);
            SetMatColor(_normalColor);
            isSelect = false;
        }
    }

    public void SetMatColor(Color color)
    {
        if (outerRingMat == null && innerRingMat == null)
        {
            MeshRenderer[] renderArr = transform.gameObject.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer meshRender in renderArr)
            {
                meshRender.material.color = color;
            }
            outerRingMat = renderArr[0].material;
            innerRingMat = renderArr[1].material;
        }
        else
        {
            outerRingMat.color = color;
            innerRingMat.color = color;
        }
    }
}
