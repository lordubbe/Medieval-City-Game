using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Item {

	public List<Item> containedItems = new List<Item>();
	public int availableSpace;
    public List<ItemTag> tagRequirements = new List<ItemTag>();


    public void AddItem(Item i)
    {
        if (availableSpace <= 0)
        {
            // no more space!
            print("no more space in container!");
            return;
        }
        if (!TestItem(i))
        {
            print("test failed");
            //item doesn't fulfill requirements!
            return;
        }
        containedItems.Add(i);
        availableSpace--;
    }

    public bool TestItem(Item i)
    {
        for (int j = 0; j < tagRequirements.Count; j++)
        {
            if (!i.tags.Contains(tagRequirements[j]))
            {
                return false;
            }
        }
        return true;
    }

    public List<Item> EmptyContainer()
    {
        List<Item> newList = containedItems;
        availableSpace += containedItems.Count;
        containedItems.Clear();
        return newList;
    }

    public override Elements GetElements()
    {
        Elements combinedElements = new Elements();
        foreach (Item i in containedItems)
        {
            combinedElements += i.GetElements();
        }

        return combinedElements;
    }

}