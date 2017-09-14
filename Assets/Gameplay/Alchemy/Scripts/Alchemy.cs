using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Alchemy : Singleton<Alchemy> {
    protected Alchemy() { }

    public PentagonObject pentaObj;
    public Material topPentaMat;
    public Material basePentaMat;

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
