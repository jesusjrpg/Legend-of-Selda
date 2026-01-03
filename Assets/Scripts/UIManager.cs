using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public Slider playerHealthBar;
    public Text playerHealthText;
    public Text playerLevelText;
    public Slider playerExpBar;
    public Image playerAvatar;

    public HealthManager playerHealthManager;
    public CharacterStats playerStats;    
    private WeaponManager weaponManager;
    private ItemsManager itemManager;
    

    private void Start()
    {
        UpdateHealthBar();
        UpdateExpBar();
        playerLevelText.text = "Nivel: " + playerStats.level;
        playerHealthManager.PlayerDamageEvent += UpdateHealthBar;
        weaponManager = FindObjectOfType<WeaponManager>();
        itemManager = FindObjectOfType<ItemsManager>();
        inventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        EscPanel.SetActive(false);
        GameOverPanel.SetActive(false);
        inventoryText.gameObject.SetActive(false);
        
        playerStats.UpdateLevelEvent += UpdateLevelText;
        playerStats.UpdateExpBarEvent += UpdateExpBar;
    }

   
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (EscPanel.activeInHierarchy)
            {
                EscPanel.SetActive(false);
            }
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MenuPanel.activeInHierarchy || inventoryPanel.activeInHierarchy || inventoryText.gameObject.activeInHierarchy)
            {
                MenuPanel.SetActive(false);
                inventoryPanel.SetActive(false);
                inventoryText.gameObject.SetActive(false);
            }
            ToggleSettings();
        }
    }

    public GameObject inventoryPanel, MenuPanel, EscPanel, GameOverPanel;
    public Button inventoryButton;
    public Text inventoryText;

    public void ToggleInventory()
    {
        inventoryText.gameObject.SetActive(!inventoryText.gameObject.activeInHierarchy);
        inventoryText.text = "";
        inventoryPanel.SetActive(!inventoryPanel.activeInHierarchy);
        MenuPanel.SetActive(!MenuPanel.activeInHierarchy);        


        if (inventoryPanel.activeInHierarchy)
        {
            foreach(Transform t in inventoryPanel.transform)
            {
                Destroy(t.gameObject);
            }
            FillInventory();
        }
    }

    public void FillInventory()
    {
        List<GameObject> weapons = weaponManager.GetAllWeapons();
        int i = 0;

        foreach(GameObject w in weapons)
        {
            AddItemToInventory(w, InventoryButton.ItemType.WEAPON, i);
            i++;
        }

        i = 0;
        List<GameObject> keyItems = itemManager.GetQuestItems();        
        foreach(GameObject item in keyItems)
        {
            AddItemToInventory(item, InventoryButton.ItemType.SPECIAL_ITEM, i);
            i++;
        }
    }

    private void AddItemToInventory(GameObject item, InventoryButton.ItemType type, int pos)
    {
        Button tempB = Instantiate(inventoryButton, inventoryPanel.transform);
        tempB.GetComponent<InventoryButton>().type = type;
        tempB.GetComponent<InventoryButton>().itemIdx = pos;
        tempB.onClick.AddListener(() => tempB.GetComponent<InventoryButton>().ActivateButton());
        tempB.image.sprite = item.GetComponent<SpriteRenderer>().sprite;
        
    }

    public void ShowOnly(int type)
    {
        foreach(Transform t in inventoryPanel.transform)
        {
            t.gameObject.SetActive((int)t.GetComponent<InventoryButton>().type == type);
        }
    }
    
    public void ShowAll()
    {
        foreach(Transform t in inventoryPanel.transform)
        {
            t.gameObject.SetActive(true);
        }
    }

    public void ChangeAvatarImage(Sprite sprite)
    {
        playerAvatar.sprite = sprite;
    }

    public void UpdateHealthBar()
    {
        playerHealthBar.maxValue = playerHealthManager.maxHealth;
        playerHealthBar.value = playerHealthManager.Health;
        StringBuilder stringBuilder = new StringBuilder().
            Append("HP: ").
            Append(playerHealthManager.Health).
            Append(" / ").
            Append(playerHealthManager.maxHealth);

        playerHealthText.text = stringBuilder.ToString();
    }
    public void UpdateLevelText()
    {
        if (playerStats.level >= playerStats.expToLevelUp.Length)
        {
            playerExpBar.enabled = false;
            return;
        }
        playerLevelText.text = "Nivel: " + playerStats.level;
    }

    public void UpdateExpBar()
    {
        playerExpBar.maxValue = playerStats.expToLevelUp[playerStats.level];
        playerExpBar.minValue = playerStats.expToLevelUp[playerStats.level - 1];
        playerExpBar.value = playerStats.exp;
    }

    public void ToggleSettings()
    {
        EscPanel.SetActive(!EscPanel.activeInHierarchy);
    }

    public void QuitGame()
    {        
        QuitGame();        
    }        
   
}
