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

    private CanvasRenderer canvasRenderer;

#if UNITY_EDITOR // only compile in editor
    private Mesh currentMesh;
    private Material currentMaterial;
    private float currentScale;
#endif

    public void Awake()
    {
        canvasRenderer = GetComponent<CanvasRenderer>();
    }

    public void OnEnable()
    {
        SetMesh();
    }

    public void OnDisable()
    {
        canvasRenderer.Clear();
    }

#if UNITY_EDITOR // only compile in editor
    public void Update()
    {
        if (mesh != currentMesh || material != currentMaterial || !Mathf.Approximately(scale, currentScale))
        {
            SetMesh();
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

        List<UIVertex> list = ConvertMesh();

        canvasRenderer.SetMaterial(material, null);
        canvasRenderer.SetVertices(list);
    }

    public List<UIVertex> ConvertMesh()
    {
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;
        Vector2[] uv = mesh.uv;

        List<UIVertex> vertexList = new List<UIVertex>(triangles.Length);

        UIVertex vertex;
        for (int i = 0; i < triangles.Length; i++)
        {
            vertex = new UIVertex();
            int triangle = triangles[i];

            vertex.position = ((vertices[triangle] - mesh.bounds.center) * scale);
            vertex.uv0 = uv[triangle];
            vertex.normal = normals[triangle];

            vertexList.Add(vertex);

            if (i % 3 == 0)
                vertexList.Add(vertex);
        }

        return vertexList;
    }
}