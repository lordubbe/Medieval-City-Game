using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderMeshTest : MonoBehaviour {

    float innerangle = 72f;
    float outerangle = 108f;

    public MeshFilter mf;
    public Mesh mesh;
    public Material mat;


    // Use this for initialization
    void Start () {
        CreateMesh();
	}
	
	// Update is called once per frame
	void Update () {
        Graphics.DrawMesh(mesh, Vector3.zero, Quaternion.identity, mat,0);
	}






    public void CreateMesh()
    {

        List<Vector3> points = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        points.Add(Vector2.zero);
        uvs.Add(Vector2.zero);

        float startingAngle = 0f;
        float angle = startingAngle; //starting angle
        for (float i = startingAngle; i < startingAngle + 360.0; i += innerangle) //go in a full circle
        {
            Vector2 s = DegreesToXY(angle, 100f, Vector2.zero);
            Vector3 ss = new Vector3(s.x, s.y, 0);
            points.Add(ss); //code snippet from above
            uvs.Add(Vector2.one);
            angle += innerangle;
        }
        
        Mesh m = new Mesh();
        m.vertices = points.ToArray();
        m.uv = uvs.ToArray();
        
        int[] tris = new int[] { 0, 1, 2, 0, 2, 3, 0, 3, 4, 0, 4, 5, 0, 5, 1 };

        m.triangles = tris;
        mesh = m;
        mf.mesh = mesh;
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



}
