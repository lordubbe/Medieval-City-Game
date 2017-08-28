using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RenderMeshOnCanvas))]
public class PentagonShape : MonoBehaviour {

    private RenderMeshOnCanvas renderShape;
    public CanvasRenderer canvasRenderer;
    public Material topPentaMat;
    public Material basePentaMat;

    public GameObject pentagonObj;

    void Awake()
    {
        renderShape = GetComponent<RenderMeshOnCanvas>();
    }

    void Start()
    {
        //DrawElementPentagon(Elements.full, canvasRenderer);
    }

    void Update()
    {
        //DrawElementPentagon(Elements.full, transform);
    }


    //public void DrawElementPentagon(Elements el, Vector2 p, CanvasRenderer r) 
    //{
        
    //    renderShape.sizes = el.ToArray();
    //    Mesh m = renderShape.CreateMesh(p);
        
    //   // renderShape.SetMesh();

    //    r.Clear();
    //    r.SetMaterial(topPentaMat,null);
    //    r.SetMesh(m);
    //}

    /// <summary>
    /// called without position, means it'll get rendered on transforms's 0,0 position. Use this, generally, unless you want to offset it for some reason.
    /// </summary>
    /// <param name="el"></param>
    /// <param name="r"></param>
    public void DrawElementPentagon(Elements el, Transform r) //should have position/recttransform in parameter
    {
        PentagonObject p = r.GetComponentInChildren<PentagonObject>();
        if (p == null)
        {
            //DON'T SPAWN SHIT
            GameObject gbase = Instantiate(Resources.Load("Prefabs/PentagonObject")) as GameObject;
            gbase.name = "Pentagon Object";
            gbase.transform.SetParent(r, false);
            p = gbase.GetComponent<PentagonObject>();
        }

        renderShape.sizes = el.ToArray();
        Mesh m = renderShape.CreateMesh(Vector2.zero);
        // renderShape.SetMesh();
        CanvasRenderer rr = p.topPenta;
        rr.Clear(); 
        rr.SetMaterial(topPentaMat, null);
        rr.SetMesh(m);

        renderShape.sizes = Elements.full.ToArray();
        m = renderShape.CreateMesh(Vector2.zero);
        rr = p.basePenta;
        rr.Clear();
        rr.SetMaterial(basePentaMat, null);
        rr.SetMesh(m);
    }

    public void DrawElementPentagonWithMat(Elements el, Transform r, Material matt) //should have position/recttransform in parameter
    {
        PentagonObject p = r.GetComponentInChildren<PentagonObject>();
        if (p == null)
        {
            //DON'T SPAWN SHIT
            GameObject gbase = Instantiate(Resources.Load("Prefabs/PentagonObject")) as GameObject;
            gbase.name = "Pentagon Object";
            gbase.transform.SetParent(r, false);
            p = gbase.GetComponent<PentagonObject>();
        }

        renderShape.sizes = el.ToArray();
        Mesh m = renderShape.CreateMesh(Vector2.zero);

        // renderShape.SetMesh();
        CanvasRenderer rr = p.customPenta;
        rr.Clear();
        rr.SetMaterial(matt, null);
        rr.SetMesh(m);
    }

    public void RemovePentagon(PentagonObject t)
    {
        PentagonObject p = t.GetComponentInChildren<PentagonObject>();
        p.basePenta.Clear();
        p.topPenta.Clear();
        p.customPenta.Clear();
        Destroy(p.gameObject);
    }


}
