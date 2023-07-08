using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
[RequireComponent (typeof(Animator))]
public class GhostController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float possessDistance = 5.0f;
    public float possessSpeed = 2.0f;
    
    public SfxController sfkController;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    
    private static readonly int Haunting = Animator.StringToHash("Haunting");
    private Hauntable Haunted { get; set; }
    private bool _startHaunt;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        // Sets Rotation to 0 to avoid weird spin glitch
        transform.rotation = new Quaternion();
        
        var distance = movementSpeed * Time.deltaTime;
        var movement = new Vector3();
        if (Haunted is null && AnimatorIsPlaying("GhostIdle"))
        {
            if (Input.GetKey(KeyCode.W))
            {
                    movement = transform.up * distance;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                movement = -transform.up * distance;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                movement = -transform.right * distance;
                _spriteRenderer.flipX = false;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                movement = transform.right * distance;
                _spriteRenderer.flipX = true;
            }
            transform.position += movement;
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (Haunted is null && AnimatorIsPlaying("GhostIdle"))
            {
                _startHaunt = false;
                HauntObject();
            }
            else if (AnimatorIsPlaying("Haunting"))
            {
                UnHauntObject();
            }
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Scare();
        }

        if (_startHaunt && !(Haunted is null))
        {
            if (Haunted.transform.position - transform.position != new Vector3())
            {
                transform.position = Vector3.MoveTowards(transform.position, Haunted.transform.position, movementSpeed * possessSpeed * Time.deltaTime);
            }
            else
            {
                HauntObject();
            }
        }
    }

    private Hauntable GetNearestInRange()
    {
        var pos = transform.position;
        var dist = float.PositiveInfinity;
        Hauntable targ = null;
        foreach (var obj in Hauntable.Furniture)
        {
            var d = (pos - obj.transform.position).sqrMagnitude;
            if (!(d < dist)||d > possessDistance) continue;
            targ = obj;
            dist = d;
        }

        return targ;
    }

    private void HauntObject()
    {
        Haunted = GetNearestInRange();
        if (Haunted is null) return;
        if (!_startHaunt)
        {
            _startHaunt = true;
            return;
        }

        Haunted.Haunt();
        _animator.SetBool(Haunting,true );
        sfkController.PlaySound("PossessingAnObject");
        _startHaunt = false;
    }

    private void UnHauntObject()
    {
        if (Haunted is null) return;
        Haunted.UnHaunt();
        _animator.SetBool(Haunting,false );
        Haunted = null;
        sfkController.PlaySound("LeavingAnObject");
    }

    private void Scare()
    {
        if (Haunted is null) return;
        
        Haunted.ObjectScare();
    }

    private void Suprise()
    {

    }

    private bool AnimatorIsPlaying(string stateName){
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
