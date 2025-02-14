
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour
{
    public GameObject spherePrefab;
    public Transform parentTrans;
    public int sphereCount = 30;
    public float orbitRadius = 20f;
    [HideInInspector] public float limitDistance = 20f;

    private SphereController controllerSphere;

    private List<GameObject> spheres = new List<GameObject>();
    private float x, y;
    private bool isCursorControl = false;

    private void Awake()
    {
    }

    void Start()
    {
        GenerateSpheres();
        Camera.main.transform.position = new Vector3(0, 15, -15);
        Camera.main.transform.LookAt(Vector3.zero);
    }

    void GenerateSpheres()
    {
        // 生成球体并随机分布[[7]]
        for (int i = 0; i < sphereCount; i++)
        {
            Vector3 insideSphere = Random.insideUnitSphere;
            Vector3 oval = new Vector3(insideSphere.x, insideSphere.y * 0.6f, insideSphere.z);
            Vector3 pos = oval * orbitRadius;
            GameObject sphere = Instantiate(spherePrefab, pos, Quaternion.identity, parentTrans);
            sphere.name = $"{spherePrefab.name}_{spheres.Count}";
            spheres.Add(sphere);
        }
    }

    void Update()
    {
        HandleRotation();
        UpdateSizes();
        UpdateRootRotation();
        OnClickedSphere();
    }

    void HandleRotation()
    {
        if (Input.GetMouseButton(0))
        {
            isCursorControl = true;
            x -= Input.GetAxis("Mouse X") * 200.0f * Time.deltaTime;
            y += Input.GetAxis("Mouse Y") * 200.0f * Time.deltaTime;

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
            parentTrans.rotation = Quaternion.Euler(parentTrans.localEulerAngles.x, x, 0.0f);
        }
    }

    public void OnClickedSphere()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Star")
                { 
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
}
