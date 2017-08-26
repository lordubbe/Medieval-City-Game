using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PentagonShape : Graphic {

    float innerangle = 72f;
    float outerangle = 108f;



    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Vector2 corner1 = Vector2.zero;
        Vector2 corner2 = Vector2.zero;

        corner1.x = 0f;
        corner1.y = 0f;
        corner2.x = 1f;
        corner2.y = 1f;

        corner1.x -= rectTransform.pivot.x;
        corner1.y -= rectTransform.pivot.y;
        corner2.x -= rectTransform.pivot.x;
        corner2.y -= rectTransform.pivot.y;

        corner1.x *= 100;
        corner1.y *= 100;
        corner2.x *= 100;
        corner2.y *= 100;

        //vh.Clear();

        //UIVertex vert = UIVertex.simpleVert;

        //vert.position = new Vector2(corner1.x, corner1.y);
        //vert.color = color;
        //vert.uv0 = Vector2.zero;
        //vh.AddVert(vert);

        //vert.position = new Vector2(corner1.x, corner2.y);
        //vert.color = color;
        //vert.uv0 = Vector2.zero;
        //vh.AddVert(vert);

        //vert.position = new Vector2(corner2.x, corner2.y);
        //vert.color = color;
        //vert.uv0 = Vector2.one;
        //vh.AddVert(vert);

        //vert.position = new Vector2(corner2.x, corner1.y);
        //vert.color = color;
        //vert.uv0 = Vector2.one;
        //vh.AddVert(vert);

        //vh.AddTriangle(0, 1, 2);
        //vh.AddTriangle(2, 3, 0);

        List<Vector2> points = new List<Vector2>();

        float startingAngle = 0f;
        float angle = startingAngle; //starting angle
        for (float i = startingAngle; i < startingAngle + 360.0; i += innerangle) //go in a full circle
        {
            points.Add(DegreesToXY(angle, 100f, Vector2.zero)); //code snippet from above
            angle += innerangle;
        }
       




        vh.Clear();

        UIVertex vert = UIVertex.simpleVert;
        vert.position = Vector2.zero;
        vert.color = Color.black;
        vert.uv0 = Vector2.zero;
        vh.AddVert(vert);

        UIVertex vert2 = UIVertex.simpleVert;
        vert2.position = points[0];
        vert2.color = Color.red;
        vert2.uv0 = new Vector2(1, 1);
        vh.AddVert(vert2);

        UIVertex vert3 = UIVertex.simpleVert;
        vert3.position = points[1];
        vert3.color = Color.blue;
        vert3.uv0 = new Vector2(1, 1);
        vh.AddVert(vert3);

        UIVertex vert4 = UIVertex.simpleVert;
        vert4.position = points[2];
        vert4.color = Color.green;
        vert4.uv0 = new Vector2(1, 1);
        vh.AddVert(vert4);

        UIVertex vert5 = UIVertex.simpleVert;
        vert5.position = points[3];
        vert5.color = Color.grey;
        vert5.uv0 = new Vector2(1, 1);
        vh.AddVert(vert5);

        UIVertex vert6 = UIVertex.simpleVert;
        vert6.position = points[4];
        vert6.color = color;
        vert6.uv0 = new Vector2(1, 1);
        vh.AddVert(vert6);

        vh.AddTriangle(0, 1, 2);
        vh.AddTriangle(0, 2, 3);
        vh.AddTriangle(0, 3, 4);
        vh.AddTriangle(0, 4, 5);
        vh.AddTriangle(0, 5, 1);

        //List<UIVertex> verts = new List<UIVertex>();

        //UIVertex vertee = UIVertex.simpleVert;
        //vertee.position = Vector2.zero;
        //vertee.color = color;
        //vertee.uv0 = Vector2.zero;
        //verts.Add(vertee);

        //int j = 0;
        //foreach(Vector2 v in points)
        //{
        //    print("www" + j);
        //    vertee = UIVertex.simpleVert;
        //    vertee.position = points[j];
        //    vertee.color = color;
        //    vertee.uv0 = Vector2.one;
        //    verts.Add(vertee);
        //    j++;
        //}

        //vh.AddUIVertexQuad(verts.ToArray());

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
