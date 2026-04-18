using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBarSpawn : MonoBehaviour
{
    public Slider bossHpSlider;
    public float animationDuration = 1.5f;

    void Start()
    {
        StartCoroutine(FillSliderOverTime());
    }

    IEnumerator FillSliderOverTime()
    {
        if (bossHpSlider != null)
        {
            bossHpSlider.value = 0f;

            float elapsedTime = 0f;
            while (elapsedTime < animationDuration)
            {
                elapsedTime += Time.deltaTime;
                float normalizedTime = elapsedTime / animationDuration;
                bossHpSlider.value = Mathf.Lerp(0f, 1f, normalizedTime);
                yield return null;
            }

            bossHpSlider.value = 1f;
        }
    }
}
