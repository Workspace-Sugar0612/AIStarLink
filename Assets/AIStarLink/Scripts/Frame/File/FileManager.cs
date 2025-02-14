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
    public string UnityWebRequestJsonString(string fileName)
    {
        string url;
        url = Application.dataPath + "/StreamingAssets/" + fileName;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();//��ȡ����
        while (true)
        {
            if (request.downloadHandler.isDone)//�Ƿ��ȡ������
            {
                return request.downloadHandler.text;
            }
        }
    }
}
