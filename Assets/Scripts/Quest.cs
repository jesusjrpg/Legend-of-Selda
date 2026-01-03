using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Quest : MonoBehaviour
{
    public int questID;
    public bool questCompleted;
    private QuestManager questManager;

    public string title;
    public string startText;
    public string completeText;

    public bool needsItem;    
    public List<QuestItem> itemNeeded;

    public bool killsEnemy;
    public List<QuestEnemy> enemies;
    public List<int> numberOfEnemies;

    public Quest nextQuest;
    private DialogueManager dialogueManager;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }   

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (needsItem)
        {
            ActivateItems();
        }
        if (killsEnemy)
        {
            ActivateEnemies();
        }       
    }

    public void StartQuest()
    {        
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.M_START);
        questManager = FindObjectOfType<QuestManager>();
        questManager.ShowQuestText(title + "\n" + startText);
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.firstDialogue = false;
        if (needsItem)
        {
            ActivateItems();
        }
        if (killsEnemy)
        {
            ActivateEnemies();
        }       
    }
    void ActivateItems()
    {
        Object[] items = Resources.FindObjectsOfTypeAll<QuestItem>();
        foreach (QuestItem item in items)
        {
            if (item.questID == questID)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    void ActivateEnemies()
    {
        Object[] qenemies = Resources.FindObjectsOfTypeAll<QuestEnemy>();
        foreach (QuestEnemy enemy in qenemies)
        {
            if (enemy.questID == questID)
            {
                enemy.gameObject.SetActive(true);
            }
        }
    }
    public void CompleteQuest()
    {
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.M_END);        
        questManager = FindObjectOfType<QuestManager>();
        questManager.ShowQuestText(title + "\n" + completeText);
        questCompleted = true;
        dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.firstDialogue = false;       
        if(nextQuest != null)
        {
            Invoke("ActivateNextQuest", 5.0f);
        }
        gameObject.SetActive(false);
    }

    void ActivateNextQuest()
    {
        nextQuest.gameObject.SetActive(true);
        nextQuest.StartQuest();
    }

    private void Update()
    {
        if(needsItem && questManager.itemCollected != null)
        {
            for(int i = 0; i<itemNeeded.Count; i++)
            {
                if(itemNeeded[i].itemName == questManager.itemCollected.itemName)
                {
                    itemNeeded.RemoveAt(i);
                    questManager.itemCollected = null;
                    break;
                }
            }
            if(itemNeeded.Count == 0)
            {                
                CompleteQuest();
            }
        }
        if(killsEnemy && questManager.enemyKilled != null)
        {
            for(int i = 0; i<enemies.Count; i++)
            {
                if(enemies[i].enemyName == questManager.enemyKilled.enemyName)
                {
                    numberOfEnemies[i]--;
                    questManager.enemyKilled = null;
                    if (numberOfEnemies[i] <= 0)
                    {
                        enemies.RemoveAt(i);
                        numberOfEnemies.RemoveAt(i);
                    }
                    break;
                }
            }
            if(enemies.Count == 0)
            {
                CompleteQuest();
            }
        }
    }
}
