using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode, RequireComponent(typeof(CanvasRenderer))]
public class RenderMeshOnCanvas : MonoBehaviour
{
    public Mesh mesh;
    public Material material;
    public float scale = 1f;

    float innerangle = 72f;
    float outerangle = 108f;

    public float[] sizes = new float[5];

    private CanvasRenderer canvasRenderer;

    public bool rendMesh = false;

    public Vector2 startPos;

#if UNITY_EDITOR // only compile in editor
    private Mesh currentMesh;
    private Material currentMaterial;
    private float currentScale;
#endif

    public void Awake()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }

    //public void OnEnable()
    //{
    //   // CreateMesh();
    //    SetMesh();
    //}

    public void OnDisable()
    {
        canvasRenderer.Clear();
    }

#if UNITY_EDITOR // only compile in editor
    public void Update()
    {
        if (mesh != currentMesh || material != currentMaterial || !Mathf.Approximately(scale, currentScale))
        {
        //    SetMesh();
        }

        if (rendMesh)
        {
            CreateMesh(startPos);
            SetMesh();
            rendMesh = false;
        }

    }
#endif

    public void SetMesh()
    {
        // clear the canvas renderer every time
        canvasRenderer.Clear();

#if UNITY_EDITOR // only compile in editor
        currentMesh = mesh;
        currentMaterial = material;
        currentScale = scale;
#endif

        if (mesh == null)
        {
            Debug.LogWarning("Mesh is null.");
            return;
        }
        else if (material == null)
        {
            Debug.LogWarning("Material is null.");
            return;
        }

      //  List<UIVertex> list = ConvertMesh();

        canvasRenderer.SetMaterial(material, null);
//        canvasRenderer.SetVertices(list);
        canvasRenderer.SetMesh(mesh);
    }




    public Mesh CreateMesh(Vector2 position)
    {
        List<Vector3> points = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        points.Add(position);
        uvs.Add(Vector2.zero);

        float startingAngle = 90f;
        float angle = startingAngle; //starting angle
        int j = 0;
        for (float i = startingAngle; i < startingAngle + 360.0; i += innerangle) //go in a full circle
        {
            Vector2 s = DegreesToXY(angle, sizes[j], position);
            Vector3 ss = new Vector3(s.x, s.y, 0);
            points.Add(ss); //code snippet from above
            uvs.Add(Vector2.one);
            angle += innerangle;
            j++;
        }

        Mesh m = new Mesh();
        m.vertices = points.ToArray();
        m.uv = uvs.ToArray();

        int[] tris = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 1 };

        m.triangles = tris;

        if (mesh != null)
        {
            mesh.Clear();
        }
        mesh = m;
        return m;

    }



    /// <summary>
    /// Calculates a point that is at an angle from the origin (0 is to the right)
    /// </summary>
    private Vector2 DegreesToXY(float degrees, float radius, Vector2 origin)
    {
        Vector2 xy = new Vector2();
        float radians = degrees * Mathf.PI / 180.0f;

        xy.x = (float)Mathf.Cos(radians) * radius + origin.x;
        xy.y = (float)Mathf.Sin(-radians) * radius + origin.y;

        return xy;
    }

    /// <summary>
    /// Calculates the angle a point is to the origin (0 is to the right)
    /// </summary>
    private float XYToDegrees(Vector2 xy, Vector2 origin)
    {
        float deltaX = origin.x - xy.x;
        float deltaY = origin.y - xy.y;

        double radAngle = Mathf.Atan2(deltaY, deltaX);
        double degreeAngle = radAngle * 180.0f / Mathf.PI;

        return (float)(180.0 - degreeAngle);
    }





    public List<UIVertex> ConvertMesh()
    {

        List<Vector3> points = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        points.Add(Vector2.zero);
        uvs.Add(Vector2.zero);

        float startingAngle = 90f;
        float angle = startingAngle; //starting angle
        int it = 0;
        for (float i = startingAngle; i < startingAngle + 360.0; i += innerangle) //go in a full circle
        {
            Vector2 s = DegreesToXY(angle, sizes[it], Vector2.zero);
            Vector3 ss = new Vector3(s.x, s.y, 0);
            points.Add(ss); //code snippet from above
            uvs.Add(Vector2.one);
            angle += innerangle;
            it++;
        }

        int[] tris = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 1 };


        List<UIVertex> vertexList = new List<UIVertex>(tris.Length);
        vertexList.Clear();

        UIVertex vertex;
        for (int i = 0; i < tris.Length; i++)
        {
            vertex = new UIVertex();
            int triangle = tris[i];

            vertex.position = ((points[triangle] - Vector3.zero) * scale);
            vertex.uv0 = uvs[triangle];

            vertexList.Add(vertex);

            if (i % 3 == 0)
                vertexList.Add(vertex);
        }

        return vertexList;
    }


}



//Vector3[] vertices = mesh.vertices;
//int[] triangles = mesh.triangles;
//Vector3[] normals = mesh.normals;
//Vector2[] uv = mesh.uv;

//List<UIVertex> vertexList = new List<UIVertex>(triangles.Length);
//vertexList.Clear();

//        UIVertex vertex;
//        for (int i = 0; i<triangles.Length; i++)
//        {
//            vertex = new UIVertex();
//int triangle = triangles[i];

//vertex.position = ((vertices[triangle] - mesh.bounds.center) * scale);
//            vertex.uv0 = uv[triangle];

//            vertexList.Add(vertex);

//            if (i % 3 == 0)
//                vertexList.Add(vertex);
//        }
