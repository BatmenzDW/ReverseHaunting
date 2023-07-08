using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GhostController player;
    public float nightDuration;
    public int startingHouseValue;

    private int _houseValue;

    private float _nightTime = 0f;
    // Start is called before the first frame update
    private void Start()
    {
        _houseValue = startingHouseValue;
    }

    // Update is called once per frame
    private void Update()
    {
        _nightTime += Time.deltaTime
    }
}
