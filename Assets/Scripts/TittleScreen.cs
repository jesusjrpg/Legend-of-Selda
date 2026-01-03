using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class TittleScreen : MonoBehaviour
{
    private bool loadScene = false;    
    public int scene;
    public GameObject All;
    [SerializeField]
    private Text loadingText;
    private ContinueState continueState;
    public Button buttonContinue;
    public Text textContinue;
    
    private void Start()
    {
        continueState = FindObjectOfType<ContinueState>();
        All.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(false);
        if(PlayerPrefs.HasKey("Money") || PlayerPrefs.HasKey("Exp") || PlayerPrefs.HasKey("Level")
            || PlayerPrefs.HasKey("PlayerX") || PlayerPrefs.HasKey("PlayerY") || PlayerPrefs.HasKey("Playerz"))
        {
            buttonContinue.interactable = true;
            textContinue.color = new Color(255f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            buttonContinue.interactable = false;
            textContinue.color = new Color(80f / 255f, 80f / 255f, 80f / 255f);
        }
    }

   

    public void LoadScene()
    {
        continueState.iscontinued = false;
        // ...set the loadScene boolean to true to prevent loading a new scene more than once...
        loadScene = true;
        All.gameObject.SetActive(false);

        // ...change the instruction text to read "Loading..."
        if (!loadingText.gameObject.activeInHierarchy)
        {
            loadingText.gameObject.SetActive(true);
        }
        loadingText.text = "Cargando...";

        // ...and start a coroutine that will load the desired scene.
        StartCoroutine(LoadNewScene());
        

    }
    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene()
    {

        // This line waits for 3 seconds before executing the next line in the coroutine.
        // This line is only necessary for this demo. The scenes are so simple that they load too fast to read the "Loading..." text.
        yield return new WaitForSeconds(3);

        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone)
        {
            yield return null;
        }

    }


    private void Update()
    {
        // If the new scene has started loading...
        if (loadScene == true)
        {

            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));

        }
    }

    public void QuitGame()
    {
        QuitGame();
    }

    public void ContinueGame()
    {
        continueState.iscontinued = true;
        // ...set the loadScene boolean to true to prevent loading a new scene more than once...
        loadScene = true;
        All.gameObject.SetActive(false);

        // ...change the instruction text to read "Loading..."
        if (!loadingText.gameObject.activeInHierarchy)
        {
            loadingText.gameObject.SetActive(true);
        }
        loadingText.text = "Cargando...";

        // ...and start a coroutine that will load the desired scene.
        StartCoroutine(LoadNewScene());
    }


}
