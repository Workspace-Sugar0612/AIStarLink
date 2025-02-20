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

    // 渐现
    private void FadeToClear()
    {
        backImage.color = Color.Lerp(backImage.color, Color.clear, fadeSpeed * Time.deltaTime);
    }

    // 渐隐
    private void FadeToBlack()
    {
        backImage.color = Color.Lerp(backImage.color, Color.black, fadeSpeed * Time.deltaTime);
    }

    // 初始化时调用
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

    // 结束时调用
    public void EndScene()
    {
        backImage.enabled = true;
        FadeToBlack();
        if (backImage.color.a >= 0.95f)
        {
            SceneManager.LoadScene("另一个场景");
        }
    }
}