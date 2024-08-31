using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DictionarySample : MonoBehaviour
{
    private Dictionary<string, Item> inventoryMap; 
    List<Item> inventory;
    void Start()
    {
        
        Item potion = new("potion", "add health", 1);
        Item manaPotion = new("mana potion", "add mana", 1);
        Item sword = new("sword", "a basic weapon", 1);
        // Add to list
        inventory = new();
        inventory.Add(potion);
        inventory.Add(manaPotion);
        inventory.Add(sword);
        
        // Add to dictionary, you need to follow key (string) value (Item)
        inventoryMap = new();
        inventoryMap.Add("potion", potion);
        inventoryMap.Add("manaPotion", manaPotion);
        inventoryMap.Add("sword", sword);

        // You can directly access the value of the dictionary by providing the key
        Item potionItem = inventoryMap["potion"];
        // For lists, you access elements through an index
        Item potionItemInList = inventory[0];
    }

    private Item FindItem(string id){
        foreach(var item in inventory){
            if(item.itemId == id){
                return item;
            }
        }
        return null;
    }

    // This is better especially if you have more data
    private Item FindItemInDictionary(string id){
        return inventoryMap[id];
    }
}

public class Item{
    public string itemId;
    public string definition;
    public int quantity;

    public Item(string id, string definition, int quantity){
        itemId = id;
        this.definition = definition;
        this.quantity = quantity;
    }
}
