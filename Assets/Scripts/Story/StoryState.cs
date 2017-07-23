using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryState : MonoBehaviour {

	public string statename;



	public virtual void OnCall(){

	}

	public virtual void OnExit(){

	}


}


/// DUNNNNO how to make these editable for me for individual states and stories. think I need some editor thing. but maybe I can just do it in script.
/// like, i need delegates? or something? can't really do delegates in editor?
/// 