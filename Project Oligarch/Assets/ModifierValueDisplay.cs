using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModifierValueDisplay : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public ItemManager manager;
    public ItemData item;
    void Start()
    {
        _text = gameObject.GetComponent<TextMeshProUGUI>();
        manager = GameObject.FindWithTag("GameManager").GetComponent<ItemManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(item != null)
            _text.text = manager
                .PlayerInventory[item].ToString();
    }
}
