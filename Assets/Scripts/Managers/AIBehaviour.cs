using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour {

    public NavMeshAgent navmesh = new NavMeshAgent();
	[HideInInspector] public AgentManager agentMan;
	public Transform dest;

	public Quaternion facingUp = new Quaternion (90, 0, 0,0);


    public void Init(AgentManager am)
    {
        agentMan = am;
        navmesh = GetComponent<NavMeshAgent>();
		GetNewDestination ();
		navmesh.updateRotation = false;

	}

	
	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (transform.position, navmesh.destination) < 3) 
		{
			GetNewDestination ();
		} 

		//transform.rotation = Quaternion.LookRotation (Vector3.forward);

		Vector3 lookPos = navmesh.steeringTarget - transform.position;
		if (lookPos != Vector3.zero && navmesh.desiredVelocity != Vector3.zero) {
			Quaternion newRotation = Quaternion.LookRotation(lookPos.WithY(0));
			newRotation *= Quaternion.Euler (-90, 0, 0);
			transform.rotation = newRotation;
		}		
	//	transform.rotation = newRotation;
		//transform.Rotate (-90, 0, 0);// = Quaternion.AngleAxis (-90, Vector3.right);

	}


	public void GetNewDestination(){
		Transform t = agentMan.GetPosition ();
		navmesh.destination = t.position;	
		dest = t;
	}

}
