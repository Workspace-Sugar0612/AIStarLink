
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    public GameObject spherePrefab;
    public Transform parentTrans;
    public int sphereCount = 30;
    public float orbitRadius = 25.0f;
    [HideInInspector] public float limitDistance = 20.0f;

    private SphereController controllerSphere;
    private float x = 0.0f, y = 0.0f;
    private List<GameObject> spheres = new List<GameObject>();
    private bool isCursorControl = false;
    private UIToolkit _uiToolkit; // 用来检查鼠标点击的是UI层还是3D场景层

    public bool isReadContent = false;
    private void Awake()
    {
        orbitRadius = 30.0f;
        limitDistance = 35.0f;
        x = 0.0f;
        y = 0.0f;

        if (!_asMath) _asMath = (ASMath)FindObjectOfType(typeof(ASMath));
        if (!_fileManager) _fileManager = (FileManager)FindObjectOfType(typeof(FileManager));
        if (!_uiToolkit) _uiToolkit = (UIToolkit)FindObjectOfType(typeof(UIToolkit));
    }

    void Start()
    {
        GenerateSpheres();
        StartCoroutine(_fileManager.UnityWebRequestJsonString(Application.streamingAssetsPath + "/keyword.txt", GetSearchTerm));
    }

    void GenerateSpheres()
    {
        for (int i = 0; i < sphereCount; i++)
        {
            Vector3 insideSphere = UnityEngine.Random.insideUnitSphere;
            Vector3 oval = new Vector3(insideSphere.x, insideSphere.y, insideSphere.z);
            Vector3 pos = oval * orbitRadius;
            GameObject sphere = Instantiate(spherePrefab, pos, Quaternion.identity, parentTrans);
            sphere.name = $"{spherePrefab.name}_{spheres.Count}";
            spheres.Add(sphere);
        }
    }

    void Update()
    {
        //HandleRotation();
        UpdateSizes();
        //UpdateRootRotation();
        OnClickedSphere();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            
            isCursorControl = true;
            x -= Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
            y += Input.GetAxis("Mouse Y") * 100.0f * Time.deltaTime;

            // 应用旋转到父物体（空物体）
            parentTrans.rotation = Quaternion.Euler(y, x, 0);
        }
        else
            isCursorControl = false;
    }

    void UpdateSizes()
    {
        foreach (var sphere in spheres)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(sphere.transform.position);
            float distance = Vector2.Distance(viewPos, Vector2.one * 0.5f);
            Vector3 scale = Vector3.one * (1.2f * (1 - distance));
            sphere.transform.localScale = scale;
        }
    }

    public void UpdateRootRotation()
    {
        if (!isCursorControl)
        {
            x -= 3.0f * Time.deltaTime;
            parentTrans.rotation = Quaternion.Euler(y, x, 0.0f);
        }
    }

    private UIConsole _uiConsole;
    public void OnClickedSphere()
    {
        if (Input.GetMouseButtonDown(0) && !_uiToolkit.CheckGuiRaycastObjects())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Star")
                {
                    if (Staticvariables.isSearch)
                    {
                        if (!_uiConsole) _uiConsole = (UIConsole)FindObjectOfType(typeof(UIConsole));
                        _uiConsole.ShowHintTip($"正在搜索中，请勿切换内容", 1.5f);
                        return;
                    }

                    SphereController controller = hit.transform.GetComponent<SphereController>();
                    if (controllerSphere != null)
                    {
                        if (controllerSphere.name == controller.name)
                        {
                            controller.OnClickedSphere();
                            controllerSphere = null;
                        }
                        else
                        {
                            controllerSphere.OnClickedSphere();
                            controller.OnClickedSphere();
                            controllerSphere = controller;
                        }
                    }
                    else
                    {
                        controller.OnClickedSphere();
                        controllerSphere = controller;
                    }
                }
            }
        }
    }

    private List<string> m_SearchTermList = new List<string>();
    private FileManager _fileManager;
    private ASMath _asMath;

    // Get keyword into streamingAssets/keyword.txt
    private void GetSearchTerm(string resultStr)
    {
        string[] keywordGroup = resultStr.Split('#');
        m_SearchTermList = keywordGroup.ToList();
        isReadContent = true;
    }

    // Assign Keyword to controller
    public IEnumerator AssignKeyword(Action<string> callback)
    {
        yield return new WaitUntil(() => { return isReadContent == true; });
        string keyword = string.Empty;
        int randIdx = _asMath.RandomInt(0, m_SearchTermList.Count - 1);
        keyword = (randIdx >= 0 && randIdx < m_SearchTermList.Count) ? m_SearchTermList[randIdx] : "NULL";
        callback(keyword);
    }
}