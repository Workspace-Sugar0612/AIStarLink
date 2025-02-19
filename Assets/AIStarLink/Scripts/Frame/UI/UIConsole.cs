using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIConsole : MonoBehaviour
{
    private static Dictionary<string, BasePanel> m_PanelList = new Dictionary<string, BasePanel>();

    public void Awake()
    {
        m_PanelList = new Dictionary<string, BasePanel>();
    }

    public T FindPanel<T>() where T : BasePanel
    {
        foreach (var panel in m_PanelList.Values)
        {
            if (panel is T)
            {
                return panel as T;
            }
        }
        return null;
    }

    public void AddList(string name, BasePanel panel)
    {
        m_PanelList.Add(name, panel);
    }
}