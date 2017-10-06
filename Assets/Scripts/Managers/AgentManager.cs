using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class AgentManager : MonoBehaviour {

    List<AIBehaviour> agents = new List<AIBehaviour>();
	public List<Transform> positions = new List<Transform>();
	public Transform positionParent;

	// Use this for initialization
	void Start () {

		agents = GetComponentsInChildren<AIBehaviour> ().ToList();

		foreach (Transform t in positionParent) {
			positions.Add (t);
		}

		foreach(AIBehaviour a in agents)
        {
            a.Init(this);
        }
	}


	public Transform GetPosition(){
	
		return positions [Random.Range (0, positions.Count)];
		
	}

}
