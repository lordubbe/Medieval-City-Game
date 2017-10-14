using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

[Serializable]
public class Node {

    public string id;
    public string text;
    public List<Option> options = new List<Option>();

	[SerializeField]
	public UnityEvent OnEnter = new UnityEvent(); 

    public Person characterSpeaking;

}

