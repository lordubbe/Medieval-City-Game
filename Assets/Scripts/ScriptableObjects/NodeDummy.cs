using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Node", menuName = "Story/Node", order = 1)]
public class NodeDummy : ScriptableObject {

    public string id;
    public string text;
    public List<string> dictest = new List<string>();

    public UnityEvent ev;
    public GameObject g;

}
