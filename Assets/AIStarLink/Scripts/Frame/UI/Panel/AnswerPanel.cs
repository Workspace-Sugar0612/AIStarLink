using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerPanel : BasePanel
{
    public TMP_Text keyWordTx;
    public TMP_Text explainTx;
    public Button backReturn;

    public override void Awake()
    {
        base.Awake();

    }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        //Active(true);

        backReturn.onClick.AddListener(Hideen);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 激活窗口，传递内容
    /// </summary>
    /// <param name="active"></param>
    /// <param name="content"></param>
    public override void Active(bool active)
    {
        if (active) { SetShow(ShowMethod.TraverseLocal, new object[] { transform, new Vector3(0f, 0f, 0f), 0.6f }); }
        else { SetShow(ShowMethod.TraverseLocal, new object[] { transform, new Vector3(1920f, 0f, 0f), 0.6f }); }
    }

    public void Open(string content, string keyword)
    {
        explainTx.text = content;
        keyWordTx.text = keyword;
        Active(true);
    }

    public void Hideen()
    {
        Active(false);
    }
}
