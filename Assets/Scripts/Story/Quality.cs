using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quality : MonoBehaviour {

    public string id;
    public string description;
    [SerializeField] float value;

    public Quality() { }
    public Quality(string idd, string desc, float val) { id = idd; description = desc; value = val; }

    public bool GetValueAsBool()
    {
        if(value != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual float GetValue()
    {
        return value;
    }

    public virtual void SetValue(float newVal)
    {
        value = newVal;
    }
}

