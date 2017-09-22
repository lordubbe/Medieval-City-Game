﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Alchemy : Singleton<Alchemy> {
    protected Alchemy() { }

    public PentagonObject pentaObj;
    public Material topPentaMat;
    public Material basePentaMat;


    public Color sinColor;
    public Color changeColor;
    public Color forceColor;
    public Color secretsColor;
    public Color beautyColor;
    public List<Color> elementColors = new List<Color>();


    public ElementBars DrawElementBars(Elements el, Transform r)
    {
        GameObject gbase;
        ElementBars eb = r.GetComponentInChildren<ElementBars>();
        if (eb == null)
        {
            gbase = Instantiate(Resources.Load("Prefabs/ElementBars")) as GameObject;
            gbase.name = "Element Bars";
            gbase.transform.SetParent(r, false);
            gbase.transform.localPosition = Vector3.zero;
            eb = gbase.GetComponent<ElementBars>();
        }
        else
        {
            gbase = eb.gameObject;
        }

        float[] els = el.ToArray();

        for (int i = 0; i < 5; i++)
        {
            eb.bars[i].localScale = new Vector3(1, Util.Map(els[i],-100f,100f,-1f,1f), 1);
            eb.bars[i].GetComponent<Image>().color = elementColors[i];
        }

        return eb;

    }


    //Draw element arrows. draw raw arrows instead. amount dependent on elements, in some sort of mapping (prolly 5 = 100, so 1 = 20)
    public ElementBars DrawElementArrows(Elements arrowEls, Transform r)
    {
        GameObject gbase;
        ElementBars eb = r.GetComponentInChildren<ElementBars>();
        if (eb == null)
        {
            gbase = Instantiate(Resources.Load("Prefabs/ElementBars")) as GameObject;
            gbase.name = "Element Bars";
            gbase.transform.SetParent(r, false);
            gbase.transform.localPosition = Vector3.zero;
            eb = gbase.GetComponent<ElementBars>();
        }
        else
        {
            gbase = eb.gameObject;
        }
        
        float[] ael = arrowEls.ToArray();
        if (eb.arrows.Count == 0)
        {
            eb.arrows.Clear();
            for (int i = 0; i < 5; i++)
            {
                eb.arrows.Add(new List<RectTransform>());
            }
        }
        List<List<RectTransform>> arrows = eb.arrows; //oh, has to be a list of lists, for the multiples upwards to work. So, there needs to be a double for later for adding +20's depending how high ael[i] is.
        for (int i = 0; i < 5; i++)
        {

            if (ael[i] != 0)
            {
                for (int j = 0; j < Mathf.Abs(ael[i]); j += 20)
                {
                    RectTransform arrow;
                    if (arrows[i].Count > (j / 20))
                    {
                        arrow = arrows[i][(j / 20)];
                    }
                    else
                    {
                        GameObject t = Instantiate(Resources.Load("Prefabs/ElementArrow")) as GameObject;
                        arrow = t.GetComponent<RectTransform>();
                        arrow.transform.SetParent(r, false);
                        arrow.transform.localPosition = Vector3.zero;
                        eb.arrows[i].Add(arrow);
                    }

                    if (ael[i] < 0)
                    {
                        float ypos = (35f * -((j / 20) + 1));// + (eb.bars[i].localScale.y < 0 ? eb.bars[i].localScale.y * 250 : 0));   //y (if it is in the same direction) otherwise, start at 0
                        arrow.anchoredPosition = new Vector2(Util.Map(i, 0, 4, -200, 200), ypos);
                        arrow.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                    }
                    else
                    {
                        float ypos = (35f * ((j / 20) + 1));// + (eb.bars[i].localScale.y > 0 ? eb.bars[i].localScale.y * 250 : 0));
                        arrow.anchoredPosition = new Vector2(Util.Map(i, 0, 4, -200, 200), ypos);
                    }
                    arrow.GetComponent<Image>().color = elementColors[i];
                }
            }
        }
        return eb;

    }



    //draw element with arrows. Requires raw item elements AND changing elements (from tool). 

    public ElementBars DrawElementBarsWithArrows(Elements el, Elements arrowEls, Transform r)
    {
        GameObject gbase;
        ElementBars eb = r.GetComponentInChildren<ElementBars>();
        if (eb == null)
        {
            gbase = Instantiate(Resources.Load("Prefabs/ElementBars")) as GameObject;
            gbase.name = "Element Bars";
            gbase.transform.SetParent(r, false);
            gbase.transform.localPosition = Vector3.zero;
            eb = gbase.GetComponent<ElementBars>();
        }
        else
        {
            gbase = eb.gameObject;
        }

        float[] els = el.ToArray();

        for (int i = 0; i < 5; i++)
        {
            eb.bars[i].localScale = new Vector3(1, Util.Map(els[i], -100f, 100f, -1f, 1f), 1);
        }

        eb.bars[0].GetComponent<Image>().color = sinColor;
        eb.bars[1].GetComponent<Image>().color = changeColor;
        eb.bars[2].GetComponent<Image>().color = forceColor;
        eb.bars[3].GetComponent<Image>().color = secretsColor;
        eb.bars[4].GetComponent<Image>().color = beautyColor;

        float[] ael = arrowEls.ToArray();
        if(eb.arrows.Count == 0)
        {
            eb.arrows.Clear();
            for (int i = 0; i < 5; i++)
            {
                eb.arrows.Add(new List<RectTransform>());
            }
        } 
        List<List<RectTransform>> arrows = eb.arrows; //oh, has to be a list of lists, for the multiples upwards to work. So, there needs to be a double for later for adding +20's depending how high ael[i] is.
        for (int i = 0; i < 5; i++)
        {
            
            if(ael[i] != 0)
            {

                for (int j = 0; j < Mathf.Abs(ael[i]); j+=20)
                {
                    RectTransform arrow;
                    if (arrows[i].Count > (j/20))
                    {
                        arrow = arrows[i][(j/20)];
                    }
                    else
                    {
                        GameObject t = Instantiate(Resources.Load("Prefabs/ElementArrow")) as GameObject;
                        arrow = t.GetComponent<RectTransform>();
                        arrow.transform.SetParent(r, false);
                        arrow.transform.localPosition = Vector3.zero;
                        eb.arrows[i].Add(arrow);
                    }
                    
                    if (ael[i] < 0)
                    {
                        float ypos = (35f * -((j / 20) + 1) + (eb.bars[i].localScale.y < 0 ? eb.bars[i].localScale.y*250 : 0));   //y (if it is in the same direction) otherwise, start at 0
                        arrow.anchoredPosition = new Vector2(Util.Map(i, 0, 4, -200, 200), ypos);
                        arrow.transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
                    }
                    else
                    {
                        float ypos = (35f * ((j / 20) + 1) + (eb.bars[i].localScale.y > 0 ? eb.bars[i].localScale.y * 250 : 0));
                        arrow.anchoredPosition = new Vector2(Util.Map(i, 0, 4, -200, 200), ypos);
                    }
                }
            }
        }

        return eb;

    }






    /// <summary>
    /// Creates a Gameobject with the pentagon shape as a Child of r. Returns the GameObject
    /// </summary>
    /// <param name="el"></param>
    /// <param name="r"></param>
    public GameObject DrawElementPentagon(Elements el, Transform r)
    {
        GameObject gbase;
        PentagonObject p = r.GetComponentInChildren<PentagonObject>();
        if (p == null)
        {
            gbase = Instantiate(Resources.Load("Prefabs/PentagonObject")) as GameObject;
            gbase.name = "Pentagon Object";
            gbase.transform.SetParent(r, false);
            p = gbase.GetComponent<PentagonObject>();
        }
        else
        {
            gbase = p.gameObject;
        }

        Elements e = ((el + 100f) / 2f);
        AlchemyUtil.sizes = e.ToArray();
        Mesh m = AlchemyUtil.CreateMesh(Vector2.zero);
        // renderShape.SetMesh();
        CanvasRenderer rr = p.topPenta;
        rr.Clear();
        rr.SetMaterial(topPentaMat, null);
        rr.SetMesh(m);

        AlchemyUtil.sizes = Elements.full.ToArray();
        m = AlchemyUtil.CreateMesh(Vector2.zero);
        rr = p.basePenta;
        rr.Clear();
        rr.SetMaterial(basePentaMat, null);
        rr.SetMesh(m);

        return gbase;
    }


}
