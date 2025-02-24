using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChatAPI : MonoBehaviour
{
    private DeepSeekInterface _deepseekInterface;
    public void Awake()
    {
        if (!_deepseekInterface)
            _deepseekInterface = (DeepSeekInterface)FindObjectOfType(typeof(DeepSeekInterface));
    }

    private UISearchAnimController _uiSearchAnimController;
    private UIConsole _uiConsole;
    public void SendOnDeepSeek(string mess, string keyword)
    {
        Debug.Log($"Send Message: {mess}.");
        Staticvariables.isSearch = true;
        StartCoroutine(_deepseekInterface.SendMessage(mess, (s) => 
        {
            Debug.Log($"SendOnDeepSeek: {s}");
            if (!_uiSearchAnimController) _uiSearchAnimController = (UISearchAnimController)FindObjectOfType(typeof(UISearchAnimController));
            if (!_uiConsole) _uiConsole = (UIConsole)FindObjectOfType(typeof(UIConsole));

            AnswerPanel answerPanel = _uiConsole.FindPanel<AnswerPanel>();
            answerPanel.Open(s, keyword);
            _uiSearchAnimController.SetAnimState(UISearchAnimController.AnimState.ChangedBack);

            Staticvariables.isSearch = false;
        }, false));
    }
}
