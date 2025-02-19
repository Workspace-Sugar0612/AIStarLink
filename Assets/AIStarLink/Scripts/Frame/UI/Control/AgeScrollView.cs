using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Progress;
using static UnityEngine.Rendering.DebugUI;

public class AgeScrollView : MonoBehaviour
{
    public Camera camera;
    public LoopListView2 loopListView;

    public float minScale = 0.5f;
    public float maxScale = 1f;
    public AnimationCurve scaleCurve; // 在Inspector中设置曲线：0时1，1时0
    private ScrollRect scrollRect;

    private void Awake()
    {
        loopListView.InitListView(100, RefreshListInfo);

        scrollRect = GetComponent<ScrollRect>();
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    public void Start()
    {
        UpdateButtonScales(); // 初始更新
    }

    private void OnScroll(Vector2 _)
    {
        UpdateButtonScales();
    }

    private void LateUpdate()
    {
        //loopListView.UpdateAllShownItemSnapData();

        //int count = loopListView.ShownItemCount;
        //for (int i = 0; i < count; ++i)
        //{
        //    var itemObj = loopListView.GetShownItemByIndex(i);
        //    var itemUI = itemObj.GetComponent<AgeItem>();

        //    Vector3 buttonWorldPosition = itemUI.transform.position;

        //    // 将世界坐标转换为视口坐标
        //    Vector3 buttonViewportPosition = camera.WorldToViewportPoint(buttonWorldPosition);

        //    // 计算按钮到视口中心的距离
        //    Vector2 viewportCenter = new Vector2(0.5f, 0.5f);
        //    float distance = Vector2.Distance(new Vector2(buttonViewportPosition.x, buttonViewportPosition.y), viewportCenter);

        //    var amount = 1 - Mathf.Abs(distance) / 235.0f;
        //    Debug.Log(distance + " || " + amount);
        //    // Debug.Log($"{itemUI.title.text}: {itemUI.IsSceneBound()}");
        //    var scale = Mathf.Clamp(amount, 0.4f, 1);
        //    itemUI.SetScale(scale);
        //}
    }
    
    private LoopListViewItem2 RefreshListInfo(LoopListView2 loopListView, int index)
    {
        if (index < 0 || index> 100) return null;
        LoopListViewItem2 item = loopListView.NewListViewItem("item");
        AgeItem ageItem = item.GetComponent<AgeItem>();
        ageItem.SetText(index.ToString());
        return item;
    }

    private void UpdateButtonScales()
    {
        RectTransform viewport = scrollRect.viewport;
        RectTransform content = scrollRect.content;

        Vector3[] viewportCorners = new Vector3[4];
        viewport.GetWorldCorners(viewportCorners);
        float viewportCenterY = (viewportCorners[0].y + viewportCorners[1].y) / 2f;
        float viewportHeight = viewportCorners[1].y - viewportCorners[0].y;

        foreach (RectTransform child in content)
        {
            Vector3[] childCorners = new Vector3[4];
            child.GetWorldCorners(childCorners);
            float childCenterY = (childCorners[0].y + childCorners[1].y) / 2f;

            float distance = Mathf.Abs(childCenterY - viewportCenterY);
            float normalizedDistance = Mathf.Clamp01(distance / (viewportHeight / 2f));
            float scale = Mathf.Lerp(maxScale, minScale, scaleCurve.Evaluate(normalizedDistance));

            child.localScale = new Vector3(1f, scale, 1f);
        }
    }
}
