using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterStats : MonoBehaviour
{
    public const int MAX_STAT_VAL = 100;
    public const int MAX_HEALTH = 9999;    
    public int level;    
    public int exp;    
    public int[] expToLevelUp;
    [Tooltip("Niveles de vida del jugador")]
    public int[] hpLevels;
    [Tooltip("Fuerza que se suma a la del arma")]
    public int[] strengthLevels;
    [Tooltip("Defensa que divide al daño del enemigo")]
    public int[] defenseLevels;
    [Tooltip("Velocidad de ataque")]
    public int[] speedLevels;
    [Tooltip("Probabilidad de que el enemigo falle")]
    public int[] luckLevels;
    [Tooltip("Probabilidad de que falle el personaje")]
    public int[] accuracyLevels;

    private HealthManager healthManager;
    private PlayerController playerController;
    private PlayerController player;
    private UIManager uIManager;

    public delegate void LevelEvent();
    public event LevelEvent UpdateLevelEvent;

    public delegate void ExpBarEvent();
    public event ExpBarEvent UpdateExpBarEvent;

    private ContinueState continueState;
    private MoneyManager moneyManager;
    private AudioManager audioManager;
    private PlayAudioTrack playAudioTrack;
    private UIManager uiManager;

    
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        playAudioTrack = FindObjectOfType<PlayAudioTrack>();
        audioManager = FindObjectOfType<AudioManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        continueState = FindObjectOfType<ContinueState>();
        playerController = GetComponent<PlayerController>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        healthManager = GetComponent<HealthManager>();
        healthManager.UpdateMaxHealth(hpLevels[level]);
        if (gameObject.tag.Equals("Enemy"))
        {
            EnemyController controller = GetComponent<EnemyController>();
            controller.speed += speedLevels[level] / MAX_STAT_VAL;
        }
        /*Tuve que usar GameObject.Find() porque si solo usaba Get Component no funcionaba lo que queria hacer
        ya que me daba un valor null no referenciado, ya que este script (CharacterStats) esta asociado al player
        y el script UIManager al canvas, y no puede conseguir el script solo mediante el GetComponent que si pudo
        Con los dos scripts de arriba, ya que ellos si que estan en el player.
        */
        uIManager = GameObject.Find("Game Canvas").GetComponent<UIManager>();
        
        if((continueState != null) && continueState.iscontinued == true)
        {
            //Si no ponia a falso el estado de es continuado, al seguir siempre true 
            //y intentar ir a otra escena, se reproducia todo este bloque otra vez
            continueState.iscontinued = false;
            int currentScene = PlayerPrefs.GetInt("CurrentScene");
            Scene scene = SceneManager.GetSceneByBuildIndex(currentScene);
            if (!scene.isLoaded)
            {                
                SceneManager.LoadSceneAsync(currentScene);
            }
            exp = PlayerPrefs.GetInt("Exp");
            level = PlayerPrefs.GetInt("Level");
            healthManager.maxHealth = PlayerPrefs.GetInt("Health");
            healthManager.UpdateMaxHealth(healthManager.maxHealth);
            moneyManager.currentMoney = PlayerPrefs.GetInt("Money");
            playAudioTrack.newTrackID = PlayerPrefs.GetInt("CurrentMusic");
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
                       
        }
        
    }


    public void AddExperience(int exp)
    {
        this.exp += exp;

        if (level >= expToLevelUp.Length)
        {
            return;
        }

        if (this.exp >= expToLevelUp[level])
        {
            level++;            
            healthManager.UpdateMaxHealth(hpLevels[level]);            
            playerController.attackTime -= speedLevels[level]/MAX_STAT_VAL;
            //Cuando subamos de nivel, este bloque hará que se actualice la informacion de la UI inmediatamente
            //de la nueva vida.
            if(uIManager != null)
            {
                uIManager.UpdateHealthBar();
            }

            //Esto manda un evento al UI Manager para que actualice el texto del nivel.
            UpdateLevelEvent?.Invoke();

        }

        //Esto manda un evento al UI Manager para que actualice la barra de EXP.
        UpdateExpBarEvent?.Invoke();
    }

    public void SaveStats()
    {        
        PlayerPrefs.SetInt("Exp", exp);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("Health", healthManager.maxHealth);
        PlayerPrefs.SetInt("Money", moneyManager.currentMoney);
        PlayerPrefs.SetFloat("PlayerX", playerController.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", playerController.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", playerController.transform.position.z);
        PlayerPrefs.SetInt("CurrentScene", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("CurrentMusic", audioManager.currentTrack);
        
    }
}
