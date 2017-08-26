using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlchemyTool : MonoBehaviour {
    
    public Elements elements;

    public List<AttributeType> neededattributeTypes = new List<AttributeType>();
    List<Item> itemsInMe = new List<Item>();
    
	public float affectRate = 0.1f;
    
    [SerializeField]
    GameObject particles;

    bool on = false;

    // Use this for initialization
    void Start () {
    }
    
    public void PlaceItem(Item a)
    {
        a.gameObject.transform.position = transform.position;
        itemsInMe.Add(a);
    }

    public void TakeItem(Item a, Vector3 pos)
    {
        a.gameObject.transform.position = pos;
        if (itemsInMe.Contains(a))
        {
            itemsInMe.Remove(a);
        }
    }


    public void TurnOn()
    {
        on = true;
        StartCoroutine("AffectItems");
    }

    public void TurnOff()
    {
        on = false;
        StopCoroutine("AffectItems");
    }


    private IEnumerator AffectItems()
    {
        while (true)
        {
            foreach (Item i in itemsInMe)
            {
                
                for (int j = 0; j < i.attributes.Count; j++)
                {
                    if (TestItem(i.attributes[j]))
                    {
                        i.attributes[j].Trigger(0.05f, elements);
                    }
                }
            }
            yield return new WaitForSeconds(affectRate);
        }
    }



    private bool TestItem(Attribute a)
    {
        if(neededattributeTypes.Count == 0)
        {
            return true;
        }

        if (neededattributeTypes.Exists(x => x == a.type))
        {
            return true;
        }
        return false;
    }

    
}
