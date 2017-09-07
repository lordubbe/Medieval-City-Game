using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlchemyTool : MonoBehaviour {
    
    public Elements elements;

    public List<AttributeType> neededattributeTypes = new List<AttributeType>();
    public List<Item> itemsInMe = new List<Item>();
    
	public float affectRate = 0.1f;

    Inventory inventory;
    public Image canvasPanel;
    
    [SerializeField]
    GameObject particles;

    bool on = false;

    // Use this for initialization
    void Start () {
    }
    


    public void SetItemsInMe()
    {
        if(inventory == null)
        {
            inventory = GetComponentInChildren<Inventory>();
        }
        
        itemsInMe.AddRange(inventory.items);
        print(itemsInMe.Count);
    }

    public void RemoveItemsInMe()
    {
        itemsInMe.Clear();
    }


    public void TurnOn()
    {
        print("turning on");
        SetItemsInMe();
        canvasPanel.color = Color.red;
        on = true;
        StartCoroutine("AffectItems");
    }

    public void TurnOff()
    {
        RemoveItemsInMe();
        canvasPanel.color = Color.white;
        on = false;
        StopCoroutine("AffectItems");
    }


    private IEnumerator AffectItems()
    {
        while (true)
        {
            //print("affecting");
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
