using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterDontDestroyOnLoad : MonoBehaviour
{
    public List<GameObject> gameobjectToDontDestroy;
    public bool createdEverything;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        foreach(GameObject gobject in gameobjectToDontDestroy)
        {
            if (!createdEverything)
            {
                DontDestroyOnLoad(gobject.transform.gameObject);
            }
        }
        createdEverything = true;
        Debug.Log("La variable created es " + createdEverything);
    }   

}
