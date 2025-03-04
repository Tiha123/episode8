using MoreMountains.Feedbacks;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PopupUI : MonoBehaviour
{
    [SerializeField] MMF_Player quitOpen;
    [SerializeField] MMF_Player quitClose;
    [SerializeField] GameObject quit;
    [SerializeField] GameObject dimmer;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        quit.SetActive(false);
        dimmer.SetActive(false);
    }
    void Update()
    {
        if(Input.GetButtonDown("Escape"))
        {
            if(quit.activeSelf==false)
            {
                quitOpen.PlayFeedbacks();
                dimmer.SetActive(true);
            }
            else if(quit.activeSelf==true)
            {
                quitClose.PlayFeedbacks();
                dimmer.SetActive(false);
            }
        }
    }

    public void QuitOK()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();// 에디터
        #else
            Application.Quit();// 빌드
        #endif

    }
    public void QuitCancel()
    {
        quitClose?.PlayFeedbacks();
    }


}
