using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GhostController player;
    public float nightDurationInSeconds;
    public float dayNightMaxRotationAngle = 180;
    public int startingHouseValue;

    [SerializeField] private Transform dayNightWheel;
    [SerializeField] private Image staminaWheel;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Image visibilityIndicator;

    public Sprite visibleSprite;
    public Sprite notVisibleSprite;
    public Sprite spottedSprite;

    public Color unfatiguedColor;
    public Color fatiguedColor;

    private int _houseValue;

    private float _nightTime;

    private bool _nightOver;

    private float _dnWheelSpeed;

    private void Start()
    {
        _houseValue = startingHouseValue;
        _dnWheelSpeed = dayNightMaxRotationAngle / nightDurationInSeconds;
    }

    private void Update()
    {
        if (_nightOver) return;
        _nightTime += Time.deltaTime;
        dayNightWheel.Rotate(0, 0, _dnWheelSpeed * Time.deltaTime);
        if (_nightTime >= nightDurationInSeconds) _nightOver = true;

        valueText.text = $"{_houseValue}";
        
        if (_nightOver && _houseValue > 0) Win(); 
    }

    public void LoseValue(int value)
    {
        _houseValue -= value;
        _houseValue = Mathf.Clamp(_houseValue, 0, startingHouseValue);

        if (_houseValue <= 0) Lose();
    }

    public void SetStaminaPercent(float percent) => staminaWheel.fillAmount = percent;

    public void SetFatigued(bool fatigue) => staminaWheel.color = fatigue ? fatiguedColor : unfatiguedColor;

    public void SetVisibility(VisibilityEnum visibility)
    {
        switch (visibility)
        {
            case VisibilityEnum.Visible:
                visibilityIndicator.sprite = visibleSprite;
                return;
            case VisibilityEnum.Spotted:
                visibilityIndicator.sprite = spottedSprite;
                return;
            case VisibilityEnum.NotVisible:
                visibilityIndicator.sprite = notVisibleSprite;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null);
        }
    }

    public void Win()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Lose()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
}
