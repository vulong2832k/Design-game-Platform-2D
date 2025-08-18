using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanelController : MonoBehaviour
{
    [SerializeField] private Image[] _stars;
    private readonly Color _defaultColor = Color.white;
    private readonly Color _achievedColor = Color.yellow;

    public void ShowStars(int stars)
    {
        Debug.Log($"Hiển thị {stars} sao");

        for (int i = 0; i < _stars.Length; i++)
        {
            _stars[i].color = (i < stars) ? _achievedColor : _defaultColor;
        }
    }
}
