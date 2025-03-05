using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    public void TapToStart()
    {
        // Debug.Log("TapToStart");

        SceneManager.LoadScene(1);
    }
}
