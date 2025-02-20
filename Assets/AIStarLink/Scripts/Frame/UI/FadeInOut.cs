using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    private float fadeSpeed = 1.5f;
    private bool sceneStarting = true;
    private RawImage backImage;

    void Start()
    {
        backImage = this.GetComponent<RawImage>();
        backImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);

        StartScene();
    }

    void Update()
    {

    }

    // ����
    private void FadeToClear()
    {
        backImage.color = Color.Lerp(backImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    // ����
    private void FadeToBlack()
    {
        backImage.color = Color.Lerp(backImage.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    // ��ʼ��ʱ����
    private void StartScene()
    {
        backImage.enabled = true;
        FadeToClear();
        backImage.DOColor(new Color(0.0f, 0.0f, 0.0f, 0.0f), 3.0f).SetEase(Ease.Linear).OnComplete(() => 
        {
            backImage.color = Color.clear;
            backImage.enabled = false;
        });
    }

    // ����ʱ����
    public void EndScene()
    {
        backImage.enabled = true;
        FadeToBlack();
        if (backImage.color.a >= 0.95f)
        {
            SceneManager.LoadScene("��һ������");
        }
    }
}