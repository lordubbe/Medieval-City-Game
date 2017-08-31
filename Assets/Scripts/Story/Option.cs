using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option {

    public string text;
    public List<FlagCondition> conditions = new List<FlagCondition>();
    public Node linkToNextNode;
}


public class FlagCondition
{
    public flag f;
    public bool b;

    public FlagCondition(flag ff, bool bb)
    {
        f = ff;
        b = bb;
    }
}