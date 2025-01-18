using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DS_LoadingScene : MonoBehaviour
{
    [SerializeField] Text loadingText;
    private string baseText = "Loading Party";  // This sha remains constant
    int dotCount = 0;  // Number of dots added
    float timeBetweenDots = 1f;  // Time delay between adding a dot (1 second)
    [SerializeField] GameObject gameScene;
    private UIManager uiManager;
 
    void Awake()
    {
        // Start the coroutine to add dots to the loading text
        StartCoroutine(UpdateLoadingText());
    }
    void Start()
    {  
        float invokeDelay = Random.Range(7, 14);
        Invoke("LoadNextScene", invokeDelay);
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    IEnumerator UpdateLoadingText()
    {
        while (true)
        {
            // Wait for the specified time before adding a dot
            yield return new WaitForSeconds(timeBetweenDots);

            // Add a dot based on the current dot count
            dotCount++;

            // Update the text by appending dots
            loadingText.text = baseText + new string('.', dotCount);

            // If there are 3 dots, reset the count back to 1
            if (dotCount >= 3)
            {
                dotCount = 0;  // Reset to 1
            }
        }
    }

    public void LoadNextScene()
    {
        //gameScene.SetActive(true);
        uiManager.slideMainMenu();
        this.gameObject.SetActive(false);
    }
}
