using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaShop : Interactable
{
    [SerializeField] Image Menu;
    public float growSpeed;
    private bool grow;
    void Start()
    {
        Menu.gameObject.SetActive(false);
    }
    private void Update()
    {
        if(grow)
        {
            if (Menu.rectTransform.localScale.x < 1f)
            {
                Menu.rectTransform.localScale += new Vector3(1, 1, 1) * Time.deltaTime * growSpeed;
            }
            else
            {
                grow = false;
            }
        }
    }

    // Update is called once per frame
    public override void InteractedWith()
    {
        Debug.Log("Opened");
        OpenShop();
    }

    public void OpenShop()
    {
        Menu.gameObject.SetActive(true);
        grow = true;
    }
}
