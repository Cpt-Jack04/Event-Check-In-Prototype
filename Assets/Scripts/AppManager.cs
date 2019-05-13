using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class AppManager : MonoBehaviour
{
    [SerializeField] private Image fadeImage = null;    // Image that fades in and out between the scene.
    private Animator fadeAnimator = null;               // Animtor that fades the fadeImage.

    public static string currentEventName = "";     // Name of the event begin attended.

    [SerializeField] private TextMeshProUGUI headerText = null;         // Text on screen for the header.
    [SerializeField] private GameObject eventInput = null;              // Entry field object.
    [SerializeField] private TextMeshProUGUI eventName = null;          // Entry field for the events.
    [SerializeField] private TextMeshProUGUI placeHolderText = null;    // Entry field for the events.
    [SerializeField] private Button changeEventButton = null;           // Used to change the event.

    void Awake()
    {
        // Sets up the fadeImage for the beginning of the scene.
        fadeImage.enabled = true;
        fadeAnimator = fadeImage.gameObject.GetComponent<Animator>();
    }

    void Start()
    {
        // Sets up the current the event information if any.
        if (currentEventName == "")
            UpdateEventInputActive(true);
        else
            UpdateEventName(currentEventName);
    }

    /// <summary> Enables the input field for events. </summary>
    public void UpdateEventInputActive(bool isActive)
    {
        eventInput.SetActive(isActive);
        changeEventButton.gameObject.SetActive(!isActive);
    }

    /// <summary>  Updates the name of the event. </summary>
    /// <param name="newEventName"> String of the new event name. </param>
    public void UpdateEventName(string newEventName)
    {
        // Is this a new event?
        if (!currentEventName.Equals(newEventName))
        {
            currentEventName = newEventName;

            GetComponent<DataManager>().UploadToFirebase();
        }
        
        // Updates UI elements
        eventName.text = currentEventName;
        placeHolderText.enabled = false;
        headerText.text += currentEventName + "!";
        UpdateEventInputActive(false);
    }

    /// <summary> Calls the coroutine to start asynchronous loading. </summary>
    public void BeginAsyncLoading()
    {
        // Begins the Fade-In animation.
        fadeAnimator.SetBool("ShouldFadeIn", true);

        // Begins the coroutine to reload the scene.
        StartCoroutine(ReloadScene());
    }

    /// <summary> Reloads scene after data entry. </summary>
    private IEnumerator ReloadScene()
    {
        // Wait for the Fade-In to finish.
        yield return new WaitForSeconds(1f);

        // Begins asynchronous loading.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);

        // If the asynchronous loading isn't done...
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}