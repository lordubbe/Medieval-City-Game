using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class OptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

    public TextMeshProUGUI text;
    public Color defColor;
    public Color highLightColor;
    public Color clickColor;


    
   
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highLightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        text.color = clickColor;
    }
}
