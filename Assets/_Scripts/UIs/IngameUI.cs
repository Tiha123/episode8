using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;
    string Dist;

    void Start()
    {
        tmDistance.text=$"거리 {0}m";
    }
    void Update()
    {
        if (GameManager.IsPlaying==false)
        {
            return;
        }
            Dist=$"{((int)GameManager.MoveDistance).ToStringKilo()} <size=75%>km</size>";
        tmDistance.text=$"{Dist}";
    }

    string FormattedFloat(float value)
    {   
        int intPortion=(int)value;
        int floatPortion=(int)((value-intPortion)*10);
        return $"{intPortion}.<size=80%>{floatPortion}</size>";
    }
}
