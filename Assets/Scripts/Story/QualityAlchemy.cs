using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityAlchemy : Quality
{
    Elements elements;

    public Elements GetElements()
    {
        return elements;
    }

    public void SetElements(Elements e)
    {
        elements = e;
    }
}