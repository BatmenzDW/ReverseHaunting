using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
public class EnemyController : MonoBehaviour
{
    public List<Transform> points;
    private int _pointIndex = 1;
    private Animator _anim;
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private GhostController player;
    [SerializeField] private GameController gameController;
    public float visionDistance = 5.0f;
    public float speed;
    public float scareMeterMax = 5.0f;
    public float scareMeterIncrement = 10f;
    public float scareMeterDecrement = 1f;
    public float _scareMeter;
    private static readonly int IsMoving = Animator.StringToHash("isMoving");

    private void Start() 
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _anim.SetBool(IsMoving,true);
        _scareMeter = scareMeterMax;
    }

    private void Update()
    {
        var diff = (points[_pointIndex].position - transform.position);
        if (diff.sqrMagnitude > 0)
        {
            var scaredSpeed = speed;
            if (_scareMeter >= scareMeterMax)
                scaredSpeed *= 2;
            
            transform.position =
                Vector3.MoveTowards(transform.position, points[_pointIndex].position,  scaredSpeed * Time.deltaTime);
            _spriteRenderer.flipX = (diff.x < 0);
        }
        else
        {
            // currentPoint = points[pointIndex];
            _pointIndex++;
            if (_pointIndex >= points.Count) _pointIndex = 0;
            
            if (_scareMeter < scareMeterMax)
                gameController.LoseValue(Random.Range(1000, 12500));
        }

        // var angleToPlayer = Vector3.Angle(transform.position, player.transform.position);
        var distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distToPlayer <= visionDistance && (player.Visibility.Equals(VisibilityEnum.Visible) || player.Visibility.Equals(VisibilityEnum.Spotted)))
        {
            player.OnSpotted(true);
        }
        else
        {
            player.OnSpotted(false);
        }

        if (distToPlayer <= visionDistance && (player.Visibility.Equals(VisibilityEnum.NotVisible)))
        {
            _scareMeter += scareMeterIncrement * Time.deltaTime;
        }
        else
        {
            _scareMeter -= scareMeterDecrement * Time.deltaTime;
            if (_scareMeter < 0) _scareMeter = 0;
        }
    }
}
