using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneFade : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [Range(0f,2.5f)]
    public float fadeInDurtion;
    private void Awake()
    {
        canvasGroup =GetComponentInChildren <CanvasGroup>();
        DontDestroyOnLoad(this);
    }

    /// <summary>
    /// 没有调用。示例协程套携程
    /// </summary>
    /// <returns></returns>
    public IEnumerator FadeOutIn() {

        yield return FadeOut(fadeInDurtion);
        yield return FadeIn(fadeInDurtion);
    }



    public IEnumerator FadeOut(float time)
    {

        while (canvasGroup.alpha<1)
        {
            canvasGroup.alpha += Time.deltaTime / time;
            yield return null;
        }


    }

    public IEnumerator FadeIn(float time)
    {

        while (canvasGroup.alpha != 0)
        {
            canvasGroup.alpha -= Time.deltaTime / time;
            yield return null;
        }

        Destroy(gameObject);

    }



}
