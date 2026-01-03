using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public int maxHealth;
    public delegate void DamageEvent();
    public event DamageEvent PlayerDamageEvent;    
    //public CharacterStats characterStats; creo que no vale

    [SerializeField]
    private int currentHealth;
    public int Health
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if(value < 0)
            {
                currentHealth = 0;
            }
            else
            {
                currentHealth = value;
            }
        }
    }

    public bool flashActive;
    public float flashLenght;
    private float flashCounter;

    private SpriteRenderer _characterRenderer;
    public SpriteRenderer _axeRenderer;
    public SpriteRenderer _swordRenderer;
    public SpriteRenderer _knifeRenderer;

    public int expWhenDefeated;

    private QuestEnemy quest;
    private QuestManager questManager;
    private UIManager uImanager;
    public GameObject player;
    public GameObject SpawnPoint;
    private PlayerController playerController;
    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        questManager = FindObjectOfType<QuestManager>();
        quest = GetComponent<QuestEnemy>();
        _characterRenderer = GetComponent<SpriteRenderer>();        
        player = GameObject.Find("Player");
        //characterStats = GetComponent<CharacterStats>(); creo que no vale 
        uImanager = FindObjectOfType<UIManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _animator = GameObject.Find("Player").GetComponent<Animator>();
    }

    public void ToggleColor(bool visible)
    {
        _characterRenderer.color = new Color(_characterRenderer.color.r,
                                             _characterRenderer.color.g,
                                             _characterRenderer.color.b,
                                             (visible ? 1.0f : 0.0f));
        if(_knifeRenderer != null && _knifeRenderer.gameObject.activeInHierarchy)
        {
            _knifeRenderer.color = new Color(_knifeRenderer.color.r, _knifeRenderer.color.g, _knifeRenderer.color.b, (visible ? 1.0f : 0.0f));            
        }
        else if(_axeRenderer != null && _axeRenderer.gameObject.activeInHierarchy)
        {
            _axeRenderer.color = new Color(_axeRenderer.color.r, _axeRenderer.color.g, _axeRenderer.color.b, (visible ? 1.0f : 0.0f));
        }
        else if (_swordRenderer != null && _swordRenderer.gameObject.activeInHierarchy)
        {
            _swordRenderer.color = new Color(_swordRenderer.color.r, _swordRenderer.color.g, _swordRenderer.color.b, (visible ? 1.0f : 0.0f));
        }


    }
    private void Update()
    {
        if (flashActive)
        {
            flashCounter -= Time.deltaTime;
            if (flashCounter > flashLenght * 0.66f)
            {
                ToggleColor(false);
            }else if(flashCounter > flashLenght * 0.33f)
            {
                ToggleColor(true);
            }else if(flashCounter > 0)
            {
                ToggleColor(false);
            }
            else
            {
                ToggleColor(true);
                flashActive = false;
                /*GetComponent<BoxCollider2D>().enabled = true;
                GetComponent<PlayerController>().canMove = true;
                */


            }
        }
                
    }

    public void DamageCharacter(int damage)
    {
        SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.HIT);
        Health -= damage;
        if (Health <= 0)
        {
            if (gameObject.tag.Equals("Enemy"))
            {
                GameObject.Find("Player").
                    GetComponent<CharacterStats>().
                    AddExperience(expWhenDefeated);
                questManager.enemyKilled = quest;
            }
            if (gameObject.name.Equals("Player"))
            {
                SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.DIE);
                gameObject.SetActive(false);

                if (!uImanager.GameOverPanel.activeInHierarchy)
                {
                    uImanager.GameOverPanel.SetActive(true);
                }
                Invoke("LoadCurrentScene", 4);
                
                //TODO: Implementar Game Over
            }
            gameObject.SetActive(false);
        }

        if (flashLenght > 0)
        {
            /*GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<PlayerController>().canMove = false;
            */
            playerController.isAttacking = false;
            _animator.SetBool("Attacking", false);
            flashActive = true;
            flashCounter = flashLenght;            
        }

        PlayerDamageEvent?.Invoke();       
    }

    public void UpdateMaxHealth(int NewMaxHealth)
    {
        maxHealth = NewMaxHealth;
        Health = maxHealth;
    }

    public void LoadCurrentScene()
    {  
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        uImanager.GameOverPanel.SetActive(false);
        player.SetActive(true);
        UpdateMaxHealth(maxHealth);
        uImanager.UpdateExpBar();
        uImanager.UpdateHealthBar();
        uImanager.UpdateLevelText();
        if(SpawnPoint != null)
        {
            this.transform.position = SpawnPoint.transform.position;
        }


    }
    
   
}
