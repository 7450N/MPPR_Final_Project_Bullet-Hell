using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTeleporter : MonoBehaviour
{
    public void tpToScene(int id)
    {
        if (id >= 0)
        {
            SceneManager.LoadScene(id);
        }
        else
        {
            Debug.LogError("Invalid Scene ID");
        }
    }
}