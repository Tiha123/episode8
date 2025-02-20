using DG.Tweening;
using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;
    [SerializeField] TextMeshProUGUI tmInformation;
    Sequence _seqInfo;
    string Dist;

    void Awake()
    {
        tmInformation.text="";
    }
    void Start()
    {
        tmDistance.text = $"{0} m";
        tmInformation.gameObject.SetActive(false);
    }
    void Update()
    {
        if (GameManager.IsPlaying == false)
        {
            return;
        }
        if (GameManager.MoveDistance < 1000f)
        {
            int intPortion = (int)GameManager.MoveDistance;
            int floatPortion = (int)((GameManager.MoveDistance - intPortion) * 10);
            tmDistance.text = $"{intPortion}.<size=80%>{floatPortion}</size>";
        }
        ((long)GameManager.MoveDistance).ToStringKilo(out string intPart, out string decPart, out string unitPart);
        tmDistance.text = $"{intPart}<size=70%>{decPart}{unitPart} m</size>";
    }

    void UpdateDistance()
    {

    }

    public void ShowInfo(string info, float duration = 1f)
    {
        tmInformation.transform.localScale=Vector3.zero;
        if (_seqInfo != null)
        {
            _seqInfo.Kill(true);
        }
        _seqInfo = DOTween.Sequence();
        _seqInfo.AppendCallback(() => tmInformation.gameObject.SetActive(true));
        _seqInfo.AppendCallback(() => tmInformation.text = info);
        _seqInfo.Append(tmInformation.transform.DOScale(1.2f, duration*0.1f));
        _seqInfo.Append(tmInformation.transform.DOScale(1f, duration*0.2f));
        _seqInfo.AppendInterval(duration*0.4f);
        _seqInfo.Append(tmInformation.transform.DOScale(0f, duration*0.5f));
        _seqInfo.AppendCallback(() => tmInformation.gameObject.SetActive(false));
    }

    string FormattedFloat(float value)
    {
        int intPortion = (int)value;
        int floatPortion = (int)((value - intPortion) * 10);
        return $"{intPortion}.<size=80%>{floatPortion}</size>";
    }
}
