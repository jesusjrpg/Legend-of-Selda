using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    public Text dialogueText;
    public Image AvatarImage;
    public bool dialogueActive;
    public bool JustEndedDialogue;
    public bool firstDialogue;

    public string[] dialogueLines;
    public int currentDialogueLine;

    private PlayerController playerController;
    private Animator playerAnimator;
    private NPCDialogue npcDialogue;

    private void Start()
    {
        dialogueActive = false;
        dialogueBox.SetActive(false);
        playerController = FindObjectOfType<PlayerController>();
        playerAnimator = GameObject.Find("Player").GetComponent<Animator>();
        npcDialogue = FindObjectOfType<NPCDialogue>();
    }
    // Update is called once per frame
    void Update()
    {
        if (dialogueActive && Input.GetMouseButtonDown(1))
        {
            currentDialogueLine++;
            //Si no se hace asi, cuando inicies dialogo se salta siempre la primera linea
            if (firstDialogue)
            {
                currentDialogueLine = 0;
                firstDialogue = false;
            }



            if (currentDialogueLine >= dialogueLines.Length)
            {
                playerController.isTalking = false;
                playerAnimator.enabled = true;
                dialogueActive = false;
                AvatarImage.enabled = false;
                dialogueBox.SetActive(false);
                JustEndedDialogue = true;
                Invoke("TempDialogFalse", 0.1f);
                currentDialogueLine = 0;
                firstDialogue = false;
            }
            else
            {
                dialogueText.text = dialogueLines[currentDialogueLine];
                dialogueActive = true;
                JustEndedDialogue = false;
                firstDialogue = false;

            }
        }
    }

    public void ShowDialogue(string[] lines)
    {
        currentDialogueLine = 0;
        firstDialogue = true;
        dialogueLines = lines;
        dialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueText.text = dialogueLines[currentDialogueLine];
        playerController.isTalking = true;
        playerController.Walking = false;
        playerAnimator.enabled = false;
    }

    public void ShowDialogue(string[] lines, Sprite sprite)
    {
        ShowDialogue(lines);
        AvatarImage.enabled = true;
        AvatarImage.sprite = sprite;
    }

    public void TempDialogFalse()
    {
        JustEndedDialogue = false;
    }
}
