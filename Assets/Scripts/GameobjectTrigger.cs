using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameobjectTrigger : MonoBehaviour
{

    public bool activateGameobject;
    public List<GameObject> gameobjectToActivate;

    public bool deactivateGameobject;
    public List<GameObject> gameobjectToDeactivate;

    public bool createdEverything;   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Equals("Player"))
        {
            if (activateGameobject && gameobjectToActivate != null)
            {
                foreach (GameObject gobject in gameobjectToActivate)
                {
                    gobject.SetActive(true);   
                }
                
            }

            if(deactivateGameobject && gameobjectToDeactivate != null)
            {
                foreach (GameObject gobject in gameobjectToDeactivate)
                {
                    gobject.SetActive(false);

                }
            }                 
        }

    }
}
