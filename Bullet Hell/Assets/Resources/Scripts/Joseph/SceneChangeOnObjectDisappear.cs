using UnityEngine;
using UnityEngine.SceneManagement;

namespace Joseph
{
    public class SceneChangeOnObjectDisappear : MonoBehaviour
    {
        public GameObject targetObject; // The object to monitor
        public string sceneName;        // Scene to switch to

        void Update()
        {
            // Check if the target object is missing (destroyed or inactive)
            if (targetObject == null || !targetObject.activeInHierarchy)
            {
                ChangeScene();
            }
        }

        void ChangeScene()
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
