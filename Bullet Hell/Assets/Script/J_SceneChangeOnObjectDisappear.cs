using UnityEngine;
using UnityEngine.SceneManagement;

public class J_SceneChangeOnObjectDisappear : MonoBehaviour
{
    public GameObject targetObject;
    public string sceneName; // The name of the scene to load when the GameObject disappears.

    void Update()
    {
        // This checks if the GameObkect is still in the scene.
        if (targetObject == null || !targetObject.activeInHierarchy)
        {
            ChangeScene(); // Changes the scene if the GameObekct is gone.
        }
    }

    void ChangeScene()
    {
        // Load the specified scene.
        SceneManager.LoadScene(sceneName);
    }
}