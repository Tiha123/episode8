using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStart : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmVersion;

    void OnValidate()
    {
        if(tmVersion!=null)
        {  
            tmVersion.text=$"v{Application.version}";
        }
    }

    public void TapToStart()
    {
        // Debug.Log("TapToStart");

        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
}
