using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QualityAlchemy : Quality
{
	[SerializeField] Elements elements;

    public Elements GetElements()
    {
        return elements;
    }

    public void SetElements(Elements e)
    {
        elements = e;
    }
}