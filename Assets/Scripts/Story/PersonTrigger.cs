using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonTrigger : MonoBehaviour {

    [SerializeField] Person p;


    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            p.InConvoRange();
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            p.OutsideConvoRange();
        }
    }


}
