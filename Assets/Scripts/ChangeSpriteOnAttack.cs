using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeSpriteOnAttack : MonoBehaviour
{
    private bool playerOnZone;
    public GameObject LeftDoorClosed, RightDoorClosed, LeftDoorOpened, RightDoorOpened, QuestSpotBruja, QuestSpotBrujaFinal, TeleportPoint;
    private NPCDialogue npcDialogue;
    public GameObject npcDialogueGameObject;
    SpriteRenderer LDoorClosed, RDoorClosed, LDoorOpened, RDoorOpened;
    private WeaponManager weaponManager;

  
    // Start is called before the first frame update
    void Start()
    {
        weaponManager = FindObjectOfType<WeaponManager>();
        npcDialogue = FindObjectOfType<NPCDialogue>();

        LeftDoorClosed.gameObject.SetActive(true);
        RightDoorClosed.gameObject.SetActive(true);
        LeftDoorOpened.gameObject.SetActive(false);
        RightDoorOpened.gameObject.SetActive(false);
        TeleportPoint.SetActive(false);
        QuestSpotBruja.SetActive(false);
        QuestSpotBrujaFinal.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && npcDialogue.PlayerInTheZone && QuestSpotBruja.gameObject.activeInHierarchy 
            && weaponManager.activeWeapon == 2 && !EventSystem.current.IsPointerOverGameObject())
        {
            LeftDoorClosed.gameObject.SetActive(false);
            RightDoorClosed.gameObject.SetActive(false);
            LeftDoorOpened.gameObject.SetActive(true);
            RightDoorOpened.gameObject.SetActive(true);
            Invoke("CompleteClickMission", 0.5f);
        }
    }


    void CompleteClickMission()
    {
        npcDialogueGameObject.gameObject.SetActive(false);
        TeleportPoint.SetActive(true);
        QuestSpotBrujaFinal.SetActive(true);
    }
    
}
