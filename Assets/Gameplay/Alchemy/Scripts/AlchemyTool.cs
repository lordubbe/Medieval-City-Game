using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class AlchemyTool : MonoBehaviour {
    
    public Elements elements;

    public List<AttributeType> neededattributeTypes = new List<AttributeType>();
    public List<Item> itemsInMe = new List<Item>();
    public ParticleSystem ps;
    public Color particleColor;
    public TextMeshProUGUI feedback;
    public ElementBars bars;
    
	public float affectRate = 0.1f;
    public float affectAmount = 0.05f;

    Inventory inventory;
    public Image canvasPanel;
    
    [SerializeField]
    GameObject particles;
    ParticleSystem.MinMaxGradient startgradient;
    ParticleSystem.MinMaxGradient endgradient;
    ParticleSystem.MinMaxGradient psColor;

    bool on = false;

    private AlchemyTool tool;
    public Canvas toolCanvas;
    public Transform pentaSpot;

    // Use this for initialization
    void Start()
    {
        psColor = ps.main.startColor;
        startgradient.color = particleColor;
        endgradient.color = Color.black;
        tool = GetComponent<AlchemyTool>();
        feedback.text = string.Empty;
        InteractionManager.OnMouseDown += OnClick;
        if (inventory == null)
        {
            inventory = GetComponentInChildren<Inventory>();
        }
        inventory.itemAdded += AddItem;
        inventory.itemRemoved += RemoveItem;
        if (bars != null)
        {
            ClearBars();
        }
        bars = Alchemy.Instance.DrawElementArrows(tool.elements, pentaSpot);
    }


    public void AddItem(Item i)
    {
        itemsInMe.Add(i);
        if (bars != null)
        {
            ClearBars();
        }
        bars = Alchemy.Instance.DrawElementBarsWithArrows(i.GetElements(), tool.elements, pentaSpot);
    }

    public void RemoveItem(Item i)
    {
        itemsInMe.Remove(i);
        if (bars != null)
        {
            ClearBars();
        }
        bars = Alchemy.Instance.DrawElementArrows(tool.elements, pentaSpot);
    }

    //public void SetItemsInMe()
    //{
    //    if(inventory == null)
    //    {
    //        inventory = GetComponentInChildren<Inventory>();
    //    }
        
    //    itemsInMe.AddRange(inventory.items);
    //}

    //public void RemoveItemsInMe()
    //{
    //    itemsInMe.Clear();
    //}


    public void TurnOn()
    {
        //RemoveItemsInMe();
        //SetItemsInMe();
        string testString = TestAllItems();
        if (testString == "can go")
        {
            canvasPanel.color = Color.red.WithAlpha(0.5f);
            on = true;
            ps.Play();
            StartCoroutine("AffectItems");
        }
        else
        {
            feedback.text = testString;
        }
    }

    public void TurnOff()
    {
        StopCoroutine("AffectItems");
       // RemoveItemsInMe();
        canvasPanel.color = Color.white.WithAlpha(0.5f);
        on = false;
        ps.Stop();
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
                        i.attributes[j].Trigger(affectAmount, elements);

                        //psColor = new ParticleSystem.MinMaxGradient();
                        ps.startColor = Color.Lerp(startgradient.color, endgradient.color, i.attributes[j].progress);
                        bars = Alchemy.Instance.DrawElementBarsWithArrows(i.GetElements(),tool.elements, pentaSpot);
                        if (i.attributes[j].progress >= 1)
                        {
                            print("turn off");
                            TurnOff();
                        }
                    }
                }
            }
            yield return new WaitForSeconds(affectRate);
        }
    }


    private string TestAllItems()
    {
        if(itemsInMe.Count == 0)
        {
            return "no items";
        }
        foreach (Item i in itemsInMe)
        {
            for (int k = 0; k < neededattributeTypes.Count; k++)
            {
                if (!i.attributes.Any(x=>x.type == neededattributeTypes[k]))
                {
                    return i.name + " is not " + neededattributeTypes[k].ToString();
                }
            }
            for (int j = 0; j < i.attributes.Count ; j++)
            {
                if (i.attributes[j].progress >= 1 && neededattributeTypes.Contains(i.attributes[j].type)) 
                {
                    return i.name + " is " + i.attributes[j].GetStateAsString();
                }
            }
        }
        return "can go";
    }

    private bool TestItem(ItemAttribute a)
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



    private void OnClick()
    {
        // click and open tool item window.
        toolCanvas.gameObject.SetActive(true);
    }

    public void OnMouseEnter()
    {

        //outline?
    }

    public void OnMouseExit()
    {
        //outline?
    }



    public void ClearBars()
    {
        for (int i = 0; i < bars.bars.Count; i++)
        {
            bars.bars[i].localScale = new Vector3(1, 0, 1);
        }
        for (int i = 0; i < bars.arrows.Count; i++)
        {
            for (int j = 0; j < bars.arrows[i].Count; j++)
            {
                Destroy(bars.arrows[i][j].gameObject);
            }
            bars.arrows[i].Clear();
        }
        bars.arrows.Clear();
        bars = null;
    }

}
