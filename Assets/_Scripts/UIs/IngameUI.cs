using CustomInspector;
using DG.Tweening;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [HorizontalLine("정보출력")]
    [SerializeField] TextMeshProUGUI tmInformation;
    [SerializeField] MMF_Player feedbackInformation;
    
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmDistance;
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmCoin;
    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmlife;
    Sequence _seqInfo;
    Sequence _seqCoin;
    string Dist;

    void Awake()
    {
        tmInformation.text = "";
    }
    void Start()
    {
        tmDistance.text = $"{0} m";
        tmInformation.gameObject.SetActive(false);
    }
    void Update()
    {
        UpdateDistance();
        UpdateCoins();
        UpdateLife();
    }

    void UpdateDistance()
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

    public void ShowInfo(string info, float duration = 1f)
    {
        tmInformation.transform.localScale = Vector3.zero;
        if (_seqInfo != null)
        {
            _seqInfo.Kill(true);
        }
        _seqInfo = DOTween.Sequence();
        _seqInfo.AppendCallback(() => tmInformation.gameObject.SetActive(true));
        _seqInfo.AppendCallback(() => tmInformation.text = info);
        _seqInfo.Append(tmInformation.rectTransform.DOScale(1.2f, duration * 0.1f));
        _seqInfo.Append(tmInformation.rectTransform.DOScale(1f, duration * 0.2f));
        _seqInfo.AppendInterval(duration * 0.4f);
        _seqInfo.Append(tmInformation.rectTransform.DOScale(0f, duration * 0.5f));
        _seqInfo.AppendCallback(() => tmInformation.gameObject.SetActive(false));
    }
    private uint _coinPrev = 0;
    void UpdateCoins()
    {
        if (_coinPrev == GameManager.Coin)
        {
            return;
        }
        tmCoin.text = GameManager.Coin.ToString("N0");
        if (_seqCoin != null)
        {
            _seqCoin.Kill(true);
        }
        tmCoin.rectTransform.localScale = Vector3.one;
        _seqCoin = DOTween.Sequence();
        _seqCoin.Append(tmCoin.rectTransform.DOPunchScale(Vector3.one * 1.2f, 0.1f, 10, 1f));
        _seqCoin.Append(tmCoin.rectTransform.DOPunchScale(Vector3.one, 0.2f, 10, 1f));
        _coinPrev++;
    }
    private int _lifePrev = 3;
    void UpdateLife()
    {
        if (GameManager.life <= 0)
        {
            tmInformation.gameObject.SetActive(true);
            tmInformation.rectTransform.DOScale(1f,0.5f);
            tmInformation.text = "Game Over!";
            GameManager.IsGameOver=true;
        }
        if (_lifePrev == GameManager.life)
        {
            return;
        }
        tmlife.text = $"{GameManager.life}/{3}";
        _lifePrev = GameManager.life;
        
    }

    string FormattedFloat(float value)
    {
        int intPortion = (int)value;
        int floatPortion = (int)((value - intPortion) * 10);
        return $"{intPortion}.<size=80%>{floatPortion}</size>";
    }
}
