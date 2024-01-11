using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class GrayEffect : MonoBehaviour
{
    private Volume volume;
    private ColorAdjustments colorAdjustments;

    private bool isChange;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }

    private void Start()
    {
        isChange = false;
        volume.profile.TryGet(out colorAdjustments);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ChangeGray(0.2f);
        }
    }

    private void ChangeGray(float time)
    {
        if (!isChange)
        {
            print("f");
            DOTween.To(() => colorAdjustments.saturation.value,
                x => colorAdjustments.saturation.value = x, -100f, time);

            DOTween.To(() => colorAdjustments.postExposure.value,
                x => colorAdjustments.postExposure.value = x, -1, time);

            DOTween.To(() => Time.timeScale,
                x => Time.timeScale = x, 0.1f, time);

            isChange = true;
        }
        else
        {
            print("t");
            DOTween.To(() => colorAdjustments.saturation.value,
                x => colorAdjustments.saturation.value = x, 0, time);

            DOTween.To(() => colorAdjustments.postExposure.value,
                x => colorAdjustments.postExposure.value = x, 0, time);

            DOTween.To(() => Time.timeScale,
                x => Time.timeScale = x, 1f, time);

            isChange = false;
        }
    }
}
