using UnityEngine;
using UnityEngine.SceneManagement;

public class J_SceneChanger : MonoBehaviour
{
    public float delay = 5f; // Time in seconds before the scene changes.
    public string sceneName; // Name of the scene to load.

    void Start()
    {
        // Changes the scene after said time.
        Invoke("ChangeScene", delay);
    }

    void ChangeScene()
    {
        // Load the scene with the specified name.
        SceneManager.LoadScene(sceneName);
    }
}
