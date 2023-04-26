using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    [SerializeField] bool started;
   // [HideInInspector]
    public bool Finish = false;
    void Start()
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(false);
        }
    }

    public void Update()
    {
        if(!Finish && started)
        {
            foreach (GameObject enemy in enemies)
            {
                if(enemy == null)
                {
                    enemies.Remove(enemy);
                }
            }
            if(enemies.Count <= 0)
              {
                  Finish = true;
              }
        }
        
    }
    public void SpawnEnemiesFunc()
    {
        started = true;
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
}
