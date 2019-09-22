using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    public List<Item> Items { get; set; }

    private Transform _bag;
    private void Awake()
    {
        _bag = transform.Find("BagUI").transform.Find("Bag");
        Items = new List<Item>();
    }

    public void OnEnterBag(Item item)
    {
        Items.Add(item);
        _bag.GetChild(Items.Count - 1).GetComponent<Image>().sprite = item.InBagSprite;
    }
}
