using UnityEngine;
using UnityEngine.SceneManagement;

namespace Joseph
{
    public class SceneChanger : MonoBehaviour
    {
        public float delay = 5f; // Time in seconds before changing the scene
        public string sceneName; // Name of the scene to load

        void Start()
        {
            // Invoke the scene change after the specified delay
            Invoke("ChangeScene", delay);
        }

        void ChangeScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
