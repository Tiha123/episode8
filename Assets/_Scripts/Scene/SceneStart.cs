using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    public string sceneName;
    public void TapToStart()
    {
        // Debug.Log("TapToStart");

        SceneManager.LoadScene(sceneName);
    }
}
