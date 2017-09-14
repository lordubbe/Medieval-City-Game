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
        InteractionManager.OnMouseDown += OnMouseDown;
    }



    public void SetItemsInMe()
    {
        if(inventory == null)
        {
            inventory = GetComponentInChildren<Inventory>();
        }
        
        itemsInMe.AddRange(inventory.items);
    }

    public void RemoveItemsInMe()
    {
        itemsInMe.Clear();
    }


    public void TurnOn()
    {
        RemoveItemsInMe();
        SetItemsInMe();
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
                        if (i.attributes[j].progress >= 1)
                        {
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
        }
        return "can go";
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



    private void OnMouseDown()
    {
        // click and open tool item window.
        toolCanvas.gameObject.SetActive(true);
        Alchemy.Instance.DrawElementPentagon(tool.elements, pentaSpot);
        //find correct position and move ??

        //get and draw pentagon shape
    }

    public void OnMouseEnter()
    {
        GameObject g = Alchemy.Instance.DrawElementPentagon(tool.elements, pentaSpot);
        //outline?
    }

    public void OnMouseExit()
    {
        //outline?
    }


}
