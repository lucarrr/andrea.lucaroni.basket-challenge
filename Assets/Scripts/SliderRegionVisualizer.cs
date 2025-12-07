using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderRegionVisualizer : MonoBehaviour
{
    public RectTransform regionContainer;
    public RectTransform sliderFillArea;

    public GameObject regionPrefab;

    public Color directShotColor, backboardShotColor; 

    private GameObject perfectShotZone, backboardShotZone;
    
    private void SetSliderRegion(float startPercent, float endPercent, Color color, GameObject zone)
    {

        float sliderHeight = sliderFillArea.rect.height;

        //GameObject zone = Instantiate(regionPrefab, regionContainer);
        RectTransform rt = zone.GetComponent<RectTransform>(); 

        float regionStartY = startPercent * sliderHeight;
        float regionEndY = endPercent * sliderHeight;
        float regionHeight = regionEndY - regionStartY;
        float regionCenterY = regionStartY + regionHeight / 2f;

        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(1, 0);
        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = new Vector2(0, regionCenterY);
        rt.sizeDelta = new Vector2(0f, regionHeight);

        zone.GetComponent<Image>().color = color;

    }

    //public void NewShotRegions(float deltaHeight)
    //{
    //    float startDirect = Random.Range(0.4f, 1 - 2 * deltaHeight);
    //    float startBackboard = Random.Range(startDirect + deltaHeight, 1 - deltaHeight);
    //    SetSliderRegion(startDirect, startDirect + deltaHeight, directShotColor);
    //    SetSliderRegion(startBackboard, startBackboard + deltaHeight, backboardShotColor);
    //}

    public void NewShotRegions(float startPerfect,float startBackboard,float epsilon)
    {
        if (!perfectShotZone || !backboardShotZone)
        {
            perfectShotZone = Instantiate(regionPrefab, regionContainer);
            backboardShotZone = Instantiate(regionPrefab, regionContainer);
        }

        SetSliderRegion(startPerfect -epsilon, startPerfect + epsilon, directShotColor, perfectShotZone);
        SetSliderRegion(startBackboard -epsilon, startBackboard + epsilon, backboardShotColor, backboardShotZone);
    }

    
}
