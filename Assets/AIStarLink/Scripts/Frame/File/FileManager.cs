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
    /// ͨ��UnityWebRequest��ȡ����StreamingAssets�ļ����е�Json�ļ�
    /// </summary>
    /// <param name="fileName">�ļ�����</param>
    /// <returns></returns>
    public IEnumerator UnityWebRequestJsonString(string filePath, Action<string> callback)
    {
        // ����UnityWebRequest����
        UnityWebRequest request = UnityWebRequest.Get(filePath);

        // �������󲢵ȴ����
        yield return request.SendWebRequest();

        // ����Ƿ��д���
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error: " + request.error);
        }
        else
        {
            // ��ȡ�ļ�����
            string fileContent = request.downloadHandler.text;
            callback(fileContent);
            // Debug.Log("File Content: " + fileContent);
        }
    }
}