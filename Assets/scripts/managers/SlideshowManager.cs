using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class SlideshowManager : MonoBehaviour
{
    [SerializeField] private GameObject[] images;

    [SerializeField] private AudioClip[] audioClips;

    [SerializeField] private StringArray[] textParts;

    [SerializeField] private FloatArray[] textPartDurations;

    [SerializeField] private TextMeshProUGUI textDisplay;

    [SerializeField] private GameObject textPanel;

    [SerializeField] private UI_Manager UImanager;

    private AudioSource audioSource;
    private int currentSlideIndex = 0;
    private Coroutine slideshowCoroutine;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        HideAllImages();

        slideshowCoroutine = StartCoroutine(StartSlideshowCoroutine());
    }

    private void HideAllImages()
    {
        foreach (GameObject img in images)
        {
            if (img != null)
            {
                img.SetActive(false);
            }
        }
        textPanel.SetActive(false);
    }

    public void NextSlide()
    {
        if (slideshowCoroutine != null)
        {
            StopCoroutine(slideshowCoroutine);
            audioSource.Stop();
        }

        if (currentSlideIndex < images.Length && images[currentSlideIndex] != null)
        {
            images[currentSlideIndex].SetActive(false);
            textPanel.SetActive(false);
            textDisplay.text = "";
        }

        currentSlideIndex++;

        if (currentSlideIndex < images.Length)
        {
            slideshowCoroutine = StartCoroutine(StartSlideshowCoroutine(currentSlideIndex));
        }
        else
        {
            Debug.Log("Slideshow has finished.");
            int index = SceneManager.GetActiveScene().buildIndex + 1;
            if (index < SceneManager.sceneCountInBuildSettings)
                UImanager.StartCoroutine(UImanager.Transition_in(index));
            else
                UImanager.StartCoroutine(UImanager.Transition_in(0));
        }
    }

    private IEnumerator StartSlideshowCoroutine(int startIndex = 0)
    {
        // A brief initial delay to ensure everything is ready before the slideshow starts.
        yield return new WaitForSeconds(0.75f);

        // Validate that the number of images, audio clips, and dialogue arrays match.
        // This is a critical check to prevent errors at runtime.
        if (images.Length != audioClips.Length || images.Length != textParts.Length || images.Length != textPartDurations.Length)
        {
            Debug.LogError("The number of images, audio clips, text part arrays, and duration arrays must be the same!");
            yield break;
        }

        // Begin the slideshow loop from the specified starting index.
        currentSlideIndex = startIndex;

        for (int i = currentSlideIndex; i < images.Length; i++)
        {
            // Display the current slide's image.
            if (images[i] != null)
            {
                images[i].SetActive(true);
                textPanel.SetActive(true);
            }

            // Play the audio clip that corresponds to the current slide.
            if (audioClips[i] != null)
            {
                audioSource.clip = audioClips[i];
                audioSource.Play();
            }

            // Handle the dialogue text for the current slide.
            // First, check if the dialogue and duration arrays are valid and have matching lengths for the current slide.
            if (textParts[i] != null && textPartDurations[i] != null && textParts[i].parts.Length == textPartDurations[i].durations.Length)
            {
                // Iterate through each part of the dialogue and its corresponding duration.
                for (int j = 0; j < textParts[i].parts.Length; j++)
                {
                    // Update the TextMeshPro display with the current dialogue part.
                    textDisplay.text = textParts[i].parts[j];

                    // Pause the coroutine for the specified duration to keep the text in sync with the audio.
                    yield return new WaitForSeconds(textPartDurations[i].durations[j]);
                }
            }
            else
            {
                // If the dialogue arrays are not set up correctly, log a warning and just wait for the full audio clip to finish.
                Debug.LogWarning("Text parts and/or durations for slide " + i + " are not configured correctly. Awaiting full audio clip duration.");
                yield return new WaitForSeconds(audioClips[i].length);
            }

            // Hide the image and clear the text before moving to the next slide.
            if (images[i] != null)
            {
                images[i].SetActive(false);
            }
            textPanel.SetActive(false);
            textDisplay.text = "";

            // Update the current index so `NextSlide()` can function correctly if called during this wait period.
            currentSlideIndex = i;
        }

        // The slideshow is complete. Trigger the scene transition.
        Debug.Log("Slideshow has finished.");
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            UImanager.StartCoroutine(UImanager.Transition_in(nextSceneIndex));
        else
            // Loop back to the beginning if this is the last scene.
            UImanager.StartCoroutine(UImanager.Transition_in(0));
    }
}
[System.Serializable]
public class StringArray
{
    public string[] parts;
}

[System.Serializable]
public class FloatArray
{
    public float[] durations;
}