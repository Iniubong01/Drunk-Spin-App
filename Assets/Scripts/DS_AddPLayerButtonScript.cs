using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DS_AddPlayerButtonScript : MonoBehaviour
{
    [SerializeField] private Text instructionText;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject inputFieldPrefab;
    [SerializeField] private Transform inputFieldsParent;
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] GameObject instructionArrow;

    private int playerCount = 0;
    private List<string> playerNames = new List<string>(); // List to store player names

    void Start()
    {
        playButton.interactable = false; // Initially make the play button non-interactable
        playButton.onClick.AddListener(OnPlayButtonPressed);
        //gameUI.SetActive(false);
        playerCount = 0;
    }

    public void OnAddPlayerButtonPressed()
    {
        if (playerCount <= 8)
        {
            GameObject newInputField = Instantiate(inputFieldPrefab, inputFieldsParent);
            newInputField.SetActive(true);
            playerCount++;

            // Set up the delete button inside the new input field
            Button deleteButton = newInputField.transform.Find("DeleteButton").GetComponent<Button>();
            deleteButton.onClick.AddListener(() => OnDeleteButtonPressed(newInputField));

            // Place the new input field at the top and scroll to the top
            newInputField.transform.SetAsFirstSibling();
            if (scrollView != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollView.verticalNormalizedPosition = 1f;
            }

            // Check player count and adjust UI
            CheckPlayerCount();
        }
    }

    public void OnDeleteButtonPressed(GameObject inputField)
    {
        playerCount--;
        playerNames.Remove(inputField.GetComponentInChildren<InputField>().text); // Remove the player's name
        Destroy(inputField);
        CheckPlayerCount();
    }

        private void CheckPlayerCount()
    {
        // Check if there are at least 2 valid (non-empty) player names
        int validPlayerCount = 0;
        foreach (Transform inputField in inputFieldsParent)
        {
            InputField playerInputField = inputField.GetComponentInChildren<InputField>();
            if (playerInputField != null && !string.IsNullOrWhiteSpace(playerInputField.text))
            {
                validPlayerCount++;
            }
        }

        // Update the interactable state of the play button
        playButton.interactable = validPlayerCount >= 2;

        // Show or hide the instruction text
        instructionText.gameObject.SetActive(validPlayerCount < 2);

        // Show or hide the instruction arrow
        instructionArrow.SetActive(validPlayerCount == 0);
    }


        public void OnPlayButtonPressed()
    {
        Debug.Log("Game Started with " + playerCount + " players!");
        gameUI.SetActive(true); // Show the game UI

        // Gather player names from the input fields
        playerNames.Clear(); // Clear previous names
        foreach (Transform inputField in inputFieldsParent)
        {
            InputField playerInputField = inputField.GetComponentInChildren<InputField>();
            if (playerInputField != null)
            {
                string playerName = playerInputField.text.Trim();
                if (!string.IsNullOrEmpty(playerName)) // Only add non-empty names
                {
                    playerNames.Add(playerName);
                }
            }
        }

            while (playerNames.Count < 9)
        {
            for (int i = 0; i < 9; i++)
            {
                // Use modulo to cycle through the playerNames list
                playerNames.Add ( playerNames[i % playerNames.Count]);
            }
        }

        playerNames.Add("All Players"); // Add the "All Players" entry as the 10th segment

        // Pass the player names to the DS_SpinWheel script
        DS_SpinWheel spinWheel = FindObjectOfType<DS_SpinWheel>(); // Assuming only one SpinWheel script exists
        spinWheel.SetPlayerNames(playerNames); // Set the player names
    }
}
