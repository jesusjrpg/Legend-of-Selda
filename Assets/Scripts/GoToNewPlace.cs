using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GoToNewPlace : MonoBehaviour
{
    public string newPlaceName = "New Scene Name Here!!";
    public bool needsClick = false;
    public string uuid;   
    private GameObject canvas;
    private GameObject image;
    // Start is called before the first frame update

    private void Start()
    {
        canvas = GameObject.Find("DialogImageCanvas");
        image = canvas.transform.Find("Puntos").gameObject;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {        
        Teleport(collision.gameObject.name);         
    }

    private void Teleport(string objName)
    {
        if (objName == "Player")
        {
            if (!needsClick || (needsClick && Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject()))
            {

                if (needsClick)
                {
                    SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.KNOCK);
                }
             FindObjectOfType<PlayerController>().nextUuid = uuid;
             SceneManager.LoadScene(newPlaceName);
             image.SetActive(false);
            }
        }   
    }
}
