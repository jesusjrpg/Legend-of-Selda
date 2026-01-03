using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    private QuestManager questManager;
    public int questID;
    public bool startPoint, endPoint;
    private bool playerInZone;
    public bool PlayerInZone
    {
        get { return playerInZone; }
    }
    public bool automaticCatch;
    private DialogueManager dialogueManager;
    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            playerInZone = false;
        }
    }

    private void Update()
    {
        TriggersQuest();
    }

    public void TriggersQuest()
    {
        if (playerInZone)
        {
            if (automaticCatch || (!automaticCatch && Input.GetMouseButtonDown(1)))
            {
                Quest q = questManager.QuestWithID(questID);
                if (q == null)
                {
                    Debug.LogErrorFormat("La mision con ID {0} no existe", questID);
                    return;
                }
                //Si llego aqui la mision existe
                if (!q.questCompleted)//Si quitamos esta linea la mision es REPETIBLE
                {
                    //No he completado la mision actual
                    if (startPoint)
                    {
                        //Estoy en la zona que empieza la mision
                        if (!q.gameObject.activeInHierarchy)
                        {
                            q.gameObject.SetActive(true);
                            q.StartQuest();
                        }
                    }
                    if (endPoint)
                    {
                        //Estoy en la zona que acaba la mision
                        if (q.gameObject.activeInHierarchy)
                        {
                            q.CompleteQuest();
                            dialogueManager = FindObjectOfType<DialogueManager>();
                            dialogueManager.firstDialogue = false;
                            
                        }
                    }
                }
            }
        }
    }
}
