using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CustomInspector;
using MoreMountains.Feedbacks;
using System.Collections.Generic;

public class IngameUI : MonoBehaviour
{

    [HorizontalLine("정보출력")]
    [SerializeField] TextMeshProUGUI tmInformation;
    [SerializeField] MMF_Player feedbackInformation;


    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmDistance;
    [SerializeField] Slider sliderDistance;
    [SerializeField] SliderUI sliderDistanceUI;


    [HorizontalLine]
    [SerializeField] TextMeshProUGUI tmlife;
    [SerializeField] TextMeshProUGUI tmCoin;

    Sequence _seqCoin;

    void Awake()
    {
        tmInformation.text = "";
    }

    void Update()
    {
        UpdateDistance();
        UpdateCoins();
        UpdateLife();
    }

    public void setDistance(List<PhaseSO> phase)
    {
        phase.ForEach(v => sliderDistanceUI.AddIcon(v.Icon, (float)v.Distance / GameManager.distanceFinish));
    }
    public void SetPhase(PhaseSO phase)
    {
        ShowInfo(phase.profileName, 3f);
    }

    public void ShowInfo(string info, float duration)
    {
        if (feedbackInformation.IsPlaying == true)
        {
            feedbackInformation.StopFeedbacks();
        }
        tmInformation.text = info;
        feedbackInformation.GetFeedbackOfType<MMF_Pause>().PauseDuration = duration;
        feedbackInformation?.PlayFeedbacks();
    }

    void UpdateDistance()
    {
        if (GameManager.IsPlaying == false)
        {
            return;
        }
        if (GameManager.MoveDistance < 1000f)
        {
            long intPortion = (long)GameManager.MoveDistance;
            int floatPortion = (int)((GameManager.MoveDistance - intPortion) * 10);
            tmDistance.text = $"{intPortion}.<size=80%>{floatPortion}</size>";
        }
        else
        {
            ((long)GameManager.MoveDistance).ToStringKilo(out string intPart, out string decPart, out string unitPart);
            tmDistance.text = $"{intPart}<size=70%>{decPart}{unitPart} m</size>";
        }
        sliderDistance.value = (float)(GameManager.MoveDistance / GameManager.distanceFinish);
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
        if (GameManager.life <= 0 && GameManager.IsGameOver == false)
        {
            ShowInfo("Game Over", 3f);
            GameManager.IsGameOver = true;
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
