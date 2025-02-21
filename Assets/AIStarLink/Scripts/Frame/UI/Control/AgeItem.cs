using UnityEngine;
using UnityEngine.UI;

public class AgeItem : MonoBehaviour
{
    public Text content;
    public Toggle toggle;
    public Image img;

    public Sprite normal;
    public Sprite press;

    private void Awake()
    {

    }

    void Start()
    {
        toggle.onValueChanged.AddListener(onValChanged);

        if (toggle.isOn) { onValChanged(true); }
        else { onValChanged(false); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(string content, string name)
    {
        SetContent(content);
        gameObject.name = name;
        gameObject.SetActive(true);
    }

    public void SetContent(string text)
    {
        content.text = text;
    }

    public void onValChanged(bool b)
    {
        if (b)
        {
            SetImage(press);
        }
        else
        {
            SetImage(normal);
        }
    }

    private void SetImage(Sprite sprite)
    {
        img.sprite = sprite;
    }
}
