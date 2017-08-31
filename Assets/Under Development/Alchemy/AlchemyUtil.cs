using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AlchemyUtil {

    public static Mesh mesh;
    public static Material material;
    public static float scale = 1f;

    static float innerangle = 72f;

    public static float[] sizes = new float[5];


    public static void SetPentagon()
    {
      //  penta.DrawElementPentagon(e, rt.transform);
    }


    public static Mesh CreateMesh(Vector2 position)
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
    public static Vector2 DegreesToXY(float degrees, float radius, Vector2 origin)
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
    public static float XYToDegrees(Vector2 xy, Vector2 origin)
    {
        float deltaX = origin.x - xy.x;
        float deltaY = origin.y - xy.y;

        double radAngle = Mathf.Atan2(deltaY, deltaX);
        double degreeAngle = radAngle * 180.0f / Mathf.PI;

        return (float)(180.0 - degreeAngle);
    }

}
