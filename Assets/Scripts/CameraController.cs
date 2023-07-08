using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    private Vector3 PlayerPos => player.position;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3 (PlayerPos.x + offset.x, PlayerPos.y + offset.y, offset.z); // Camera follows the player with specified offset position
    }
}
