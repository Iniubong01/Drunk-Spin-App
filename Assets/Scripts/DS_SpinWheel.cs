using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DS_SpinWheel : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 1000f;
    [SerializeField] private Text resultText, yourTurnText, displayText, gameText;
    [SerializeField] private Button spinButton, doneButton;
    [SerializeField] private int numberOfSections = 10;  // 10 segments: 9 for players, 1 for "All Players"
    [SerializeField] private Transform wheelTransform, indicator;
    public Transform indImage;
    [SerializeField] private float highlightScale = 1.2f;
    [SerializeField] private float highlightDuration = 0.3f;
    [SerializeField] private Vector3 positionOffset = new Vector3(-0.5f, 0, 0);
    [SerializeField] private float indicatorOffsetAngle; // Offset for North-East alignment
    [Tooltip("Game Scene")]
    [SerializeField] private RectTransform wheelParent;
    private DS_QuestionScript questionScript;
    private UIManager uiManager;

    private bool isSpinning = false;
    private float currentSpeed, sectionAngle;
    private Transform winningSegmentTransform;
    private List<string> playerNames = new List<string>(); // Declare the list of player names

    [Header("Sway Settings")]
    public float swayAmount = 0.05f, swaySpeed = 1f, rotationAmount = 2f, rotationSpeed = 1f;

    private Vector3 initialPosition;     // The initial position of the object
    private Quaternion initialRotation;  // The initial rotation of the object
    private bool isSwaying = true;

    private Transform detectedSegment; // To store the detected segment
    private bool isNormalMode;
    private bool isSpicyMode;
    private bool isWeirdMOde;
    private bool isLadiesNightMode;
    public Button menuButton;
    public Button settingsButton;



    void Start()
    {
        isNormalMode = true;
        questionScript = GameObject.Find("Question Manager").GetComponent<DS_QuestionScript>();
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        sectionAngle = 360f / numberOfSections;

        resultText = GameObject.Find("Result").GetComponent<Text>();
        spinButton.onClick.AddListener(() => StartCoroutine(SpinWheel()));
    }

    public void WheelSway()
    {
        if (!isSwaying) return;

        float swayX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        transform.localPosition = initialPosition + new Vector3(swayX, 0f, 0f);
        float rotation = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        transform.localRotation = initialRotation * Quaternion.Euler(0f, 0f, rotation);
    }

    public void StopSway()
    {
        isSwaying = false;
    }

    public void ResumeSway()
    {
        isSwaying = true;
    }

    public void SetPlayerNames(List<string> names)
    {
        playerNames = names;
        List<string> segments = new List<string>();
        int numberOfPlayerSections = numberOfSections - 1;

        for (int i = 0; i < numberOfPlayerSections; i++)
        {
            segments.Add(playerNames[i % playerNames.Count]);
        }

        segments.Add("All Players");

        for (int i = 0; i < segments.Count; i++)
        {
            Transform segmentTransform = wheelTransform.GetChild(i);
            Text segmentText = segmentTransform.GetComponentInChildren<Text>();
            if (segmentText != null)
            {
                segmentText.text = segments[i];
            }
            else
            {
                Debug.LogError($"Text component missing on segment {i}");
            }
        }
    }

    private void DetectWinningSegment()
    {
        float detectionRadius = 0.1f; // Radius for detecting nearby colliders
        Collider2D[] detectedColliders = Physics2D.OverlapCircleAll(indicator.position, detectionRadius);

        if (detectedColliders.Length > 0)
        {
            // Filter colliders to only include those tagged as "WheelSegment"
            List<Collider2D> segmentColliders = new List<Collider2D>();
            foreach (var collider in detectedColliders)
            {
                if (collider.CompareTag("WheelSegment"))
                {
                    segmentColliders.Add(collider);
                }
            }

            if (segmentColliders.Count > 0)
            {
                // Find the segment closest to the indicator
                Collider2D closestCollider = null;
                float closestDistance = float.MaxValue;

                foreach (var collider in segmentColliders)
                {
                    float distance = Vector2.Distance(indicator.position, collider.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestCollider = collider;
                    }
                }

                // Use the closest valid segment
                detectedSegment = closestCollider.transform;
                Debug.Log("Closest segment detected: " + detectedSegment.name);
            }
            else
            {
                Debug.LogWarning("No valid 'WheelSegment' colliders detected within radius.");
                detectedSegment = null;
            }
        }
        else
        {
            Debug.LogWarning("No colliders detected near indicator.");
            detectedSegment = null;
        }
    }

        private void ResetWheelState()
    {
        spinButton.gameObject.SetActive(true);
        yourTurnText.gameObject.SetActive(false);
        resultText.text = "";
        gameText.text = "";
        displayText.text = "";
        DS_SoundManager.instance.Stop("Spin");

        wheelParent.DOAnchorPosX(0, 0.25f);
        wheelParent.DOScale(new Vector3(1, 1, 1f), 0.25f);
    }

    public void ResetButtonState()
    {
        menuButton.interactable = true;
        settingsButton.interactable = true;
    }

     public void ResetSpin()
    {
        StopAllCoroutines(); // Stop all active coroutines
        isSpinning = false;
        currentSpeed = 0;
        ResetWheelState();
    }

    
    public void ResetSpinSettings()
    {
        StopAllCoroutines();
        isSpinning = false;
        currentSpeed = 0;
        ResetWheelState();
    }


    public void Reset()
    {
        spinButton.gameObject.SetActive(true);
        yourTurnText.gameObject.SetActive(false);
        resultText.text = "";
        gameText.text = "";
        displayText.text = "";
        DS_SoundManager.instance.Stop("Spin");
        isSpinning = false;
        // Rescale the wheelParent
        wheelParent.DOAnchorPosX((0), 0.25f);
        // Rescale the wheelParent
        wheelParent.DOScale(new Vector3(1, 1, 1f), 0.25f);
    }
    
    public void wheelAnim()
    {       
        // Position Animation with smooth easing
        wheelParent.DOAnchorPosX(25, 0.5f).SetEase(Ease.OutQuad); // Smooth move to X = 25
        wheelParent.DOAnchorPosX(-600, 0.5f).SetDelay(0.5f).SetEase(Ease.OutQuad); // Smooth move to X = -600
        wheelParent.DOAnchorPosX(-400, 0.7f).SetDelay(1f).SetEase(Ease.InOutQuad); // Smooth move to X = -450

        // Scale Animation with smooth easing
        wheelParent.DOScale(new Vector3(0.9f, 0.9f, 1f), 0.5f).SetEase(Ease.OutQuad); // Scale down smoothly
        wheelParent.DOScale(new Vector3(1.5f, 1.5f, 1f), 0.25f).SetDelay(0.5f).SetEase(Ease.InOutQuad); // Scale up smoothly

        // Rotation Animation
        wheelParent.DORotate(new Vector3(0, 0, -40f), 0.5f).SetEase(Ease.OutQuad); // Slight rotation clockwise
        wheelParent.DORotate(new Vector3(0, 0, 20f), 0.25f).SetDelay(0.5f).SetEase(Ease.InOutQuad); // Reset rotation smoothly
        wheelParent.DORotate(Vector3.zero, 0.7f).SetDelay(1f).SetEase(Ease.InOutQuad); // Reset rotation smoothly            
    }

    private IEnumerator SpinWheel()
    {
        if (isSpinning) yield break;

        isSpinning = true;
        spinButton.gameObject.SetActive(false);
        currentSpeed = initialSpeed;
        float decelerationRate = Random.Range(150f, 250f);

        // Play spin sound during spin
        DS_SoundManager.instance.Play("Spin");

        while (currentSpeed > 0)
        {
            wheelTransform.Rotate(0, 0, -currentSpeed * Time.deltaTime);
            currentSpeed -= decelerationRate * Time.deltaTime;
            yield return null;
        }

        // Spin is completed, so stop the spin sound
        DS_SoundManager.instance.Stop("Spin");

        menuButton.interactable = false;
        settingsButton.interactable = false;
       
        Invoke("wheelAnim", 1.2f);
        
        yield return new WaitForSeconds(0.1f); // Delay before detection

        // Detect the winning segment after the wheel stops
        DetectWinningSegment();

        if (detectedSegment != null)
        {
            Debug.Log($"Winning segment detected: {detectedSegment.name}");
            winningSegmentTransform = detectedSegment;

            yield return new WaitForSeconds(2.5f); // Delay before higlighting winning segment

            StartCoroutine(HighlightWinningSegment(winningSegmentTransform));
            DS_SoundManager.instance.Play("Highlight");

            Text segmentText = winningSegmentTransform.GetComponentInChildren<Text>();
            if (segmentText != null)
            {
                resultText.text = segmentText.text;

                // Check if the segment is "All Players"
                if (segmentText.text == "All Players")
                {
                    // - This will handle the question asked depending on the theme selected,
                    // this is when the Highlighted Segment is All Players, so make sure to do as well for single players turn
                    // - You can as well add the coin logic here adding && to the if conditions to enable unlocking the locked themes
                    // - Meanwhile since you said you'll handle that, I have not coded any to be in locked state,
                    // so all the themes will play regardless 
                    
                    if(isNormalMode == true)
                    {
                        gameText.text = "Everyone, " + questionScript.callQuestion();
                    }
                    else if(isSpicyMode == true)
                    {
                        gameText.text = "Everyone, " + questionScript.callSpiceUpMode();
                    }
                    else if(isWeirdMOde == true)
                    {
                        gameText.text = "Everyone, " + questionScript.callWeirdQuestionMode();
                    }
                    else if(isLadiesNightMode == true)
                    {
                        gameText.text = "Everyone, " + questionScript.callLadiesNightMode();
                    }

                    yourTurnText.text = questionScript.AllPlayersTurn();
                    displayText.text = "If " + segmentText.text + " don't get it, they " + questionScript.callAllInstructionText();
                }
                else
                {
                    // If not All Players, that's if the highlighted player is a single player
                    if(isNormalMode == true)
                    {
                        gameText.text = segmentText.text + ", " + questionScript.callQuestion();                        
                    }
                    else if(isSpicyMode == true)
                    {
                        gameText.text = segmentText.text + ", " + questionScript.callSpiceUpMode();
                    }
                    else if(isWeirdMOde == true)
                    {
                        gameText.text = segmentText.text + ", " + questionScript.callWeirdQuestionMode();
                    }
                    else if(isLadiesNightMode == true)
                    {
                       gameText.text = segmentText.text + ", " + questionScript.callLadiesNightMode();
                    }

                    yourTurnText.text = questionScript.callYourTurnText();
                    displayText.text = "If " + segmentText.text + " doesn't get it, " + segmentText.text + " " + questionScript.callInstructionText();
                }

                yourTurnText.gameObject.SetActive(true);
            }


        }
        else
        {
            Debug.LogWarning("No segment detected.");
        }

        isSpinning = false;
        StopSway();
        LoadGame();
    }

     public void LoadGame()
    {
        uiManager.undoSpinMenu();
    }

    private IEnumerator HighlightWinningSegment(Transform segment)
    {
        if (segment == null)
        {
            Debug.LogError("No segment provided for highlighting!");
            yield break;
        }

        Debug.Log($"Highlighting segment: {segment.name}");
        int originalSiblingIndex = segment.GetSiblingIndex();
        segment.SetAsLastSibling();

        Vector3 originalScale = segment.localScale;
        segment.localScale = originalScale * highlightScale;

        yield return new WaitForSeconds(highlightDuration);

        segment.localScale = originalScale;
        segment.SetSiblingIndex(originalSiblingIndex);
    }

    private void Update()
    {
        if (!isSpinning)
        {
            WheelSway();
        }
    }


    // Bools to control the theme selected
    public void normalTheme()
    {
        isNormalMode = true;
        isSpicyMode = false;
        isWeirdMOde = false;
        isLadiesNightMode = false;
    }

    public void spiceUpTheme()
    {
        isNormalMode = false;
        isSpicyMode = true;
        isWeirdMOde = false;
        isLadiesNightMode = false;
    }

    public void weirdTheme()
    {
        isNormalMode = false;
        isSpicyMode = false;
        isWeirdMOde = true;
        isLadiesNightMode = false;
    }

    public void LadiesNightTheme()
    {
        isNormalMode = false;
        isSpicyMode = false;
        isWeirdMOde = false;
        isLadiesNightMode = true;
    }


    



    //////////////
    ///


    /////////////////
    ///
}


