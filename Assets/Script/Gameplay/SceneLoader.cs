using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Scene Transition Settings")]
    public string sceneToLoad = "PlayScene";
    public Image fadeImage;           // Assign your UI full-screen image here
    public float fadeDuration = 1f;

    private bool isFading = false;

    private void Awake()
    {
        // Singleton pattern to persist across scenes
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (fadeImage != null)
        {
            // Ensure the fade image canvas also persists
            DontDestroyOnLoad(fadeImage.transform.root.gameObject);
            StartCoroutine(FadeIn()); // Fade from white on scene start
        }

        // Subscribe to scene change to auto fade in on new scene load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (fadeImage != null)
        {
            StartCoroutine(FadeIn());
        }
    }

    public void LoadSceneWithFade()
    {
        if (!isFading)
        {
            StartCoroutine(FadeAndLoadScene());
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;

        float timer = 0f;
        Color color = fadeImage.color;

        // Fade to white
        while (timer < fadeDuration)
        {
            color.a = timer / fadeDuration;
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        yield return new WaitForSeconds(0.1f);

        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;
        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;

        while (timer < fadeDuration)
        {
            color.a = 1f - (timer / fadeDuration);
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = 0f;
        fadeImage.color = color;
        isFading = false;
    }
}
