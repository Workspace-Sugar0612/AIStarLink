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
    public string UnityWebRequestJsonString(string fileName)
    {
        string url;
        url = Application.dataPath + "/StreamingAssets/" + fileName;
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();//读取数据
        while (true)
        {
            if (request.downloadHandler.isDone)//是否读取完数据
            {
                return request.downloadHandler.text;
            }
        }
    }
}
