using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class NodeCreator : MonoBehaviour {

    public EntryPoint entry = new EntryPoint();
    public List<ExitPoint> exits;
    public List<Node> nodes = new List<Node>();
    public string id;

    public delegate void MyDelegate();      // I DUNNO HOW IT'S SUPPOSED TO DEAL WITH DELEGATES????? make no sense. and unityevents have problems, unless I make a parser. BUT STRINGS.
    public System.Action myAction = new System.Action(YoStart);


    void Start()
    {
        myAction = YoStart;
    }



    static void YoStart()
    {
        print("hey");
    }
    
}
