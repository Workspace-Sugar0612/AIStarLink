using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public enum LookAtMethod
    {
        Rotation,
        Forward,
        LookAt,
        None
    }

    private Transform _mainCameraTrans;
    public LookAtMethod lookAtMethod = LookAtMethod.None;

    // Start is called before the first frame update
    void Start()
    {

    }

    public Transform parent; // B物体
    public float heightOffset = 1.0f; // A物体在B物体上方的高度偏移

    private void Update()
    {
        if (parent != null)
        {
            // 获取B物体的世界坐标
            Vector3 parentPosition = parent.position;

            // 设置A物体的位置为B物体的正上方
            transform.position = parentPosition + Vector3.up * heightOffset;

            // 保持A物体的旋转与B物体一致
            transform.rotation = parent.rotation;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_mainCameraTrans != null)
        {
            if (lookAtMethod == LookAtMethod.Rotation) { this.transform.rotation = Quaternion.LookRotation(this.transform.position - _mainCameraTrans.position); }
            else if (lookAtMethod == LookAtMethod.Forward) { this.transform.forward = _mainCameraTrans.forward; }
            else if (lookAtMethod == LookAtMethod.LookAt) { this.transform.LookAt(_mainCameraTrans); }
            else { }
        }
        else
            _mainCameraTrans = Camera.main.transform;
    }
}
