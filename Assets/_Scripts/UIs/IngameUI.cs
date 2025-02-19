using TMPro;
using UnityEngine;

public class IngameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmDistance;
    string Dist;

    void Start()
    {
        tmDistance.text = $"거리 {0}m";
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

    string FormattedFloat(float value)
    {
        int intPortion = (int)value;
        int floatPortion = (int)((value - intPortion) * 10);
        return $"{intPortion}.<size=80%>{floatPortion}</size>";
    }
}
