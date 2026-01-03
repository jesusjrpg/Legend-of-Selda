using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CircleCollider2D))]
public class NPCDialogue : MonoBehaviour
{
    public string npcName;
    public string[] npcDialogueLines;
    public Sprite npcSprite;
    private GameObject image;
    private GameObject canvas;    
    public GameObject QuestTrigger;
    public GameObject TeleportTrigger;
    public List<GameObject> ActivateGameobject;
    public List<GameObject> DeactivateGameobject;
    public bool activateAfterTalk;
    public bool deactivateAfterTalk;
    public bool teleportAfterTalk;
    public bool questTalk; 
    private QuestTrigger questTriggerScript;
    private DialogueManager dialogueManager;  
    private bool playerInTheZone;
    
    public bool PlayerInTheZone
    {
        get
        {
            return playerInTheZone;
        }
        set
        {
            playerInTheZone = value;
        }
    }
   


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        canvas = GameObject.Find("DialogImageCanvas");
        image = canvas.transform.Find("Puntos").gameObject;
        image.SetActive(false);
        questTriggerScript = FindObjectOfType<QuestTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerInTheZone = true;
            image.SetActive(true);            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerInTheZone = false;
            image.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if(playerInTheZone && Input.GetMouseButtonDown(1) && dialogueManager.dialogueActive == false)
        {
            
            string[] finalDialogue = new string[npcDialogueLines.Length];

            int i = 0;
            foreach(string line in npcDialogueLines)
            {
                finalDialogue[i++] = (npcName != null ? npcName + "\n" : "") + line;                
            }
     
            if (npcSprite != null)
            {
                dialogueManager.ShowDialogue(finalDialogue, npcSprite);
            }
            else
            {
                dialogueManager.ShowDialogue(finalDialogue);
            }
            if (gameObject.GetComponentInParent<NPCMovement>() != null)
            {
                gameObject.GetComponentInParent<NPCMovement>().isTalking = true;
            }
        }
        //Esto no funcionaba porque no ponia el playerInTheZone asi que dadas esas dos condiciones en todo el mapa, se activava la quest hablases
        //con quien hablases
        if (playerInTheZone && questTalk && dialogueManager.JustEndedDialogue == true)
        {
            Invoke("ActivateQuest", 0.5f);           
        }
        if(playerInTheZone && teleportAfterTalk && dialogueManager.JustEndedDialogue == true)
        {
            ActivateTeleport();
        }
        if(playerInTheZone && activateAfterTalk && dialogueManager.JustEndedDialogue == true)
        {
            ActivateGameObject();
        }
        if(playerInTheZone && deactivateAfterTalk && dialogueManager.JustEndedDialogue == true)
        {
            DeactivateGameObject();
        }
    }

    public void ActivateQuest()
    {        
        if (QuestTrigger != null)
        {
            QuestTrigger.SetActive(true);
            //Se hace esto porque si se activaba una quest automaticamente, al no dar click, tenias que dar dos veces click
            //porque el primer click hacia el if del firstdialogue del DialogueManager
            dialogueManager.firstDialogue = false;           
        }
    }

    public void ActivateTeleport()
    {
        if(TeleportTrigger != null)
        {
            TeleportTrigger.SetActive(true);
            Invoke("TempDeactivatedTeleport", 0.1f);
        }
    }

    public void TempDeactivatedTeleport()
    {
        TeleportTrigger.SetActive(false);
    }

    public void ActivateGameObject()
    {
        if(ActivateGameobject != null)
        {
            foreach (GameObject gobject in ActivateGameobject)
            {                
                if(gobject != null)
                {
                    gobject.SetActive(true);
                }
            }
        }
        
        
    }

    public void DeactivateGameObject()
    {
        if(DeactivateGameobject != null)
        {
            foreach (GameObject gobject in DeactivateGameobject)
            {
                if(gobject != null)
                {
                    gobject.SetActive(false);
                }
            }
        }
    }
}
