using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected UIConsole _uiConsole;
    public string panelName;

    public virtual void Awake()
    {
        _uiConsole = (UIConsole)FindObjectOfType(typeof(UIConsole));
    }

    public virtual void Start()
    {
        _uiConsole.AddList(panelName, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
