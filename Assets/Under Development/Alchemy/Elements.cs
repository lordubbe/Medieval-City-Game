using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Elements {

    public float sin;
    public float change;
    public float force;
    public float secrets;
    public float beauty;

    public static Elements zero = new Elements(0, 0, 0, 0, 0);


    public Elements(float s, float c, float f, float se, float b)
    {
        sin = s;
        force = f;
        change = c;
        secrets = se;
        beauty = b;
    }

    public Elements()
    {
    }


    public override string ToString()
    {
        return "("+sin+", "+change+", "+force+", "+secrets+", "+beauty+")";
    }

}
