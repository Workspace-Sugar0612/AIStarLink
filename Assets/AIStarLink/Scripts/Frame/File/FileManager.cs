using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 通过UnityWebRequest获取本地StreamingAssets文件夹中的Json文件
    /// </summary>
    /// <param name="fileName">文件名称</param>
    /// <returns></returns>
    public IEnumerator UnityWebRequestJsonString(string filePath, Action<string> callback)
    {
        // 创建UnityWebRequest对象
        UnityWebRequest request = UnityWebRequest.Get(filePath);

        // 发送请求并等待完成
        yield return request.SendWebRequest();

        // 检查是否有错误
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // 获取文件内容
            string fileContent = request.downloadHandler.text;
            callback(fileContent);
            // Debug.Log("File Content: " + fileContent);
        }
    }
}