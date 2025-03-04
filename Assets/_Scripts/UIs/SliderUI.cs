using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class SliderUI : MonoBehaviour
{
    [Serializable]
    public struct Icons
    {
        public Image image;
        [Range(0, 1)] public float value;

        public Icons(Image i, float v)
        {
            image = i;
            value = Mathf.Clamp(v, 0f, 1f);
        }
    }

    [SerializeField] Slider slider;
    [SerializeField] Image handleIcon;

    [SerializeField] List<Image> imagepools = new List<Image>();
    [SerializeField] List<Icons> icons = new List<Icons>();

    RectTransform sliderRect;

    void Start()
    {
        sliderRect = slider.GetComponent<RectTransform>();
    }
    float elapsed = 0f;
    void Update()
    {
        if (GameManager.IsPlaying == false || GameManager.IsGameOver == true || GameManager.IsUIOpen==true)
        {
            return;
        }
        SetPosition(handleIcon, slider.normalizedValue);
        elapsed += Time.deltaTime;
        if (elapsed > 0.1f)
        {
            SetAllPosition();
            elapsed = 0f;
        }
    }
    int _imagenum = 0;
    public void AddIcon(Sprite sprite, float value)
    {
        if (sprite == null)
        {
            return;
        }
        Image image = imagepools[_imagenum++ % imagepools.Count];

        if (image == null)
        {
            Debug.LogWarning("사용할 수 있는 Image pool 없음");
            return;
        }
        image.gameObject.SetActive(true);
        image.sprite = sprite;

        icons.Add(new Icons(image, value));

        SetPosition(image, value);
    }

    void SetPosition(Image imagee, float percent)
    {
        float width = sliderRect.rect.width;
        float xpos = sliderRect.rect.xMin + percent * width;
        imagee.rectTransform.localPosition = new Vector3(xpos, imagee.rectTransform.localPosition.y, imagee.rectTransform.localPosition.z);
    }

    void SetAllPosition()
    {
        icons.ForEach(v =>
        {
            SetPosition(v.image, v.value);
        });
    }
}
