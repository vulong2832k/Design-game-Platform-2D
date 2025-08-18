using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class LightSwitchMap3 : MonoBehaviour
{
    [SerializeField] private Light2D _outsideLight;
    [SerializeField] private Light2D _pyramidLight;


    [SerializeField] private float _outsideLightIntensity = 1f;
    [SerializeField] private float _pyramidLightIntensity = 0.8f;
    [SerializeField] private float _darkIntensity = 0.01f;

    private void Start()
    {
        _outsideLight.intensity = 1f;
        _pyramidLight.intensity = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _outsideLight.intensity = _darkIntensity;
            _pyramidLight.intensity = _pyramidLightIntensity;
            
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _outsideLight.intensity = _outsideLightIntensity;
            _pyramidLight.intensity = 0f;

        }
    }
}
