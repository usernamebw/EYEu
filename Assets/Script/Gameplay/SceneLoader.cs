using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("Scene Transition Settings")]
    public string sceneToLoad = "PlayScene";
    public Image fadeImage;
    public float fadeDuration = 1f;

    private bool isFading = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("[SceneLoader] Awake - Singleton created");
    }

    private void Start()
    {
        if (fadeImage != null)
        {
            DontDestroyOnLoad(fadeImage.transform.root.gameObject);
            Debug.Log("[SceneLoader] Start - Begin initial FadeIn");
            StartCoroutine(FadeIn());
        }
        else
        {
            Debug.LogWarning("[SceneLoader] Start - No fadeImage assigned!");
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[SceneLoader] Scene Loaded: {scene.name} - Begin FadeIn");
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true); 
            StartCoroutine(FadeIn());
        } else {
          Debug.LogWarning("[SceneLoader] No fadeImage found on scene load!");
        }
    }

    public void LoadSceneWithFade()
    {
        if (!isFading)
        {
            Debug.Log("[SceneLoader] LoadSceneWithFade - Begin FadeAndLoadScene");
            StartCoroutine(FadeAndLoadScene());
        }
        else
        {
            Debug.LogWarning("[SceneLoader] LoadSceneWithFade - Already fading!");
        }
    }

    private IEnumerator FadeAndLoadScene()
    {
        isFading = true;
        Debug.Log("[SceneLoader] FadeAndLoadScene - Fading to white");

        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            color.a = timer / fadeDuration;
            fadeImage.color = color;
            timer += Time.deltaTime;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
        Debug.Log("[SceneLoader] FadeAndLoadScene - Fade to white complete");

        yield return new WaitForSeconds(0.1f);

        Debug.Log($"[SceneLoader] Loading scene: {sceneToLoad}");
        SceneManager.LoadScene(sceneToLoad);
    }

    private IEnumerator FadeIn()
    {
        Debug.Log("[SceneLoader] FadeIn - Start fading from white");

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

        Debug.Log("[SceneLoader] FadeIn - Fade from white complete");
    }
}
