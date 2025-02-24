using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class DeepSeekInterface : MonoBehaviour
{
    public string apiKey; // = "sk-0a6b3ddfa5294f98a4efb06aa35ee5ab";
    public string apiUrl; // = "https://api.deepseek.com/chat/completions";
    public string modelDeepseek;
    // 用于存储对话历史
    private List<Dictionary<string, string>> messages = new List<Dictionary<string, string>>();

    void Start()
    {
        Debug.Log($"apikey: {apiKey}.\n apiurl: {apiUrl}.\n modelDeepseek: {modelDeepseek}.\n");
        // 初始化系统消息
        messages.Add(new Dictionary<string, string> { { "role", "system" }, { "content", "You are a helpful assistant." } });
    }

    public IEnumerator SendMessage(string mess, Action<string> callback, bool isStream = false)
    {
        string userMessage = mess;//userInputField.text;
        if (string.IsNullOrEmpty(userMessage)) yield return null;

        // 添加用户消息到对话历史
        messages.Add(new Dictionary<string, string> { { "role", "user" }, { "content", userMessage } });

        // 调用 DeepSeek API
        StartCoroutine(CallDeepSeekAPI(callback, isStream));
    }

    private IEnumerator CallDeepSeekAPI(Action<string> callback, bool isStream = false)
    {
        // 创建请求数据
        var requestData = new
        {
            model = modelDeepseek,
            messages = messages,
            stream = isStream
        };

        string jsonData = JsonMapper.ToJson(requestData);
        //string jsonData = JsonConvert.SerializeObject(requestData);
        Debug.Log("jsondata: " + jsonData);
        // 创建 UnityWebRequest
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        // 发送请求
        yield return request.SendWebRequest();
        //if (request.result != UnityWebRequest.Result.Success)
        //{
        //    Debug.LogError(request.error);
        //}

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"request.downloadHandler.text: {request.downloadHandler.text}, request.responseCode: {request.responseCode}");
            if (request.downloadHandler.text.Count() <= 0)
            {
                ServerTimeOut();
                yield break;
            }
            // 解析响应
            var response = JsonMapper.ToObject<DeepSeekResponse>(request.downloadHandler.text);
            string botMessage = response.choices[0].message.content;
            //string reasoning_content = response.choices[0].message.reasoning_content;
            
            // 添加 AI 消息到对话历史
            messages.Add(new Dictionary<string, string> { { "role", "assistant" }, { "content", botMessage } });

            // callback("<think/>\n" + reasoning_content + "\n</endthink>\n" + botMessage);
            callback(botMessage);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private UIConsole _uiConsole;
    private UISearchAnimController _uiSearchAnimController;
    public void ServerTimeOut()
    {
        if (!_uiConsole) _uiConsole = (UIConsole) FindObjectOfType(typeof(UIConsole));
        if (!_uiSearchAnimController) _uiSearchAnimController = (UISearchAnimController)FindObjectOfType(typeof(UISearchAnimController));

        _uiConsole.ShowHintTip($"服务器繁忙，请稍后再试！", 3.5f);
        _uiSearchAnimController.SetAnimState(UISearchAnimController.AnimState.ChangedBack);
        Staticvariables.isSearch = false;
    }

    public class StreamingDownloadHandler : DownloadHandlerScript
    {
        private StringBuilder receivedData = new StringBuilder();

        protected override bool ReceiveData(byte[] data, int dataLength)
        {
            if (data == null || dataLength == 0) return false;
            string chunk = Encoding.UTF8.GetString(data, 0, dataLength);
            receivedData.Append(chunk);

            // 解析分块数据（示例按行分割）
            string[] lines = receivedData.ToString().Split('\n');
            foreach (var line in lines)
            {
                if (line.StartsWith("data: "))
                {
                    string json = line.Substring(6).Trim();
                    if (json == "[DONE]") break;
                    var response = JsonUtility.FromJson<DeepSeekResponse>(json);
                    Debug.Log($"Received: {response.choices[0].message.content}");
                }
            }
            return true;
        }
    }

    [System.Serializable]
    public class DeepSeekResponse // 定义响应数据结构
    {
        public Choice[] choices;
        public Usage usage;

        [System.Serializable]
        public class Choice
        {
            public Message message;
        }

        [System.Serializable]
        public class Message
        {
            public string role;
            public string content;
        }

        [System.Serializable]
        public class Usage
        {
            public int total_tokens;
        }
    }
}
