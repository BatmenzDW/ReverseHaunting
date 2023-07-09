using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class GhostController : MonoBehaviour
{
    [SerializeField] private AttributesScriptableObject attributesScriptableObject;

    [SerializeField] private GameController gameController;
    
    private float _maxGhostStamina;
    private float _currentGhostStamina;
    private float _movementSpeed;
    private float _possessDistance;
    public float possessSpeed = 2.0f;
    public float staminaRecoveryRate = 10;
    public float sprintStaminaDepletionRate = 10;
    public float possessStaminaDepletionRate = 10;
    public SfxController sfkController;

    private VisibilityEnum _viz = VisibilityEnum.Visible;

    private VisibilityEnum Visibility
    {
        get => _viz;
        set
        {
            gameController.SetVisibility(value);
            _viz = value;
        }
    }

    public bool Fatigued { get; set; }
    private bool _ftg;
    
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private static readonly int Haunting = Animator.StringToHash("Haunting");
    private static readonly int SpottedTag = Animator.StringToHash("Spotted");
    private static readonly int FatiguedTag = Animator.StringToHash("Fatigued");
    private Hauntable Haunted { get; set; }

    private void Start()
    {
        gameController.SetVisibility(_viz);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _maxGhostStamina = attributesScriptableObject.Stamina;
        _currentGhostStamina = _maxGhostStamina;
        _movementSpeed = attributesScriptableObject.Speed;
        _possessDistance = attributesScriptableObject.PossesionRange;
    }

    public void OnSpotted(bool isSpotted)
    {
        Visibility = VisibilityEnum.Spotted;
        _animator.SetBool(SpottedTag, isSpotted);
    }

    private static bool IsVisible(VisibilityEnum viz) => viz switch
    {
        VisibilityEnum.Spotted => true,
        VisibilityEnum.Visible => true,
        _ => false
    };
    
    private void Update()
    {
        var distance = _movementSpeed * Time.deltaTime;
        var movement = new Vector3();
        if (Haunted is null && IsVisible(Visibility))
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
            
        }

        if (Input.GetKeyDown(KeyCode.E) && !Fatigued && _ftg)
            if (Haunted is null && IsVisible(Visibility))
                HauntObject();
        
        if (Input.GetKeyUp(KeyCode.E)) UnHauntObject();
        if (Input.GetKey(KeyCode.Q)) Scare();

        if (Input.GetKey(KeyCode.LeftShift) && !Fatigued)
        {
            // Increase movement speed when Left Shift is held down
            movement *= 2f; // You can adjust the multiplier to increase or decrease the speed boost

            _currentGhostStamina -= sprintStaminaDepletionRate * Time.deltaTime;
            
            if (_currentGhostStamina < 0) OnFatigued(true);
        }
        transform.position += movement;

        if (_currentGhostStamina > _maxGhostStamina ) _currentGhostStamina = _maxGhostStamina;
        
        if (IsVisible(Visibility) && !(Haunted is null))
        {
            if (Haunted.transform.position - transform.position != new Vector3())
            {
                transform.position = Vector3.MoveTowards(transform.position, Haunted.transform.position, _movementSpeed * possessSpeed * Time.deltaTime);
            }
            else
            {
                HauntObject();
            }
        }
        
        StaminaRecover(staminaRecoveryRate);
        gameController.SetStaminaPercent(_currentGhostStamina / _maxGhostStamina);
        _ftg = !Fatigued;
    }

    private void OnFatigued(bool isFatigued)
    {
        Fatigued = isFatigued;
        
        _animator.SetBool(FatiguedTag, isFatigued);

        gameController.SetFatigued(isFatigued);
        
        if (isFatigued) sfkController.PlaySound("NoStamina");
    }
    
    private bool StaminaUse()
    {
        if (Haunted is null) return Input.GetKey(KeyCode.LeftShift);
        
        _currentGhostStamina -= possessStaminaDepletionRate * Time.deltaTime;

        if (!(_currentGhostStamina < 0))
        {
            return true;
        }
        
        _currentGhostStamina = 0;
        UnHauntObject();
        OnFatigued(true);

        return true;
    }

    private void StaminaRecover(float recoverValue)
    {
        if (StaminaUse() || _currentGhostStamina >= _maxGhostStamina) return;
        
        _currentGhostStamina += recoverValue * Time.deltaTime;
        _currentGhostStamina = Mathf.Clamp(_currentGhostStamina, 0f, _maxGhostStamina);

        if (_currentGhostStamina >= _maxGhostStamina)
            OnFatigued(false);
    }

    private void HauntObject()
    {
        Haunted = GetNearestInRange();
        if (Haunted is null || !IsVisible(Visibility)) return;

        Haunted.Haunt();
        _animator.SetBool(Haunting,true );
        sfkController.PlaySound("PossessingAnObject");
        Visibility = VisibilityEnum.NotVisible;
    }
     
     private Hauntable GetNearestInRange()
    {
        var pos = transform.position;
        var dist = float.PositiveInfinity;
        Hauntable tar = null;
        foreach (var obj in Hauntable.Furniture)
        {
            var d = (pos - obj.transform.position).sqrMagnitude;
            if (!(d < dist)||d > _possessDistance) continue;
            tar = obj;
            dist = d;
        }

        return tar;
    }

    private void UnHauntObject()
    {
        if (Haunted is null || !AnimatorIsPlaying("Haunting")) return;
        Haunted.UnHaunt();
        _animator.SetBool(Haunting,false );
        Visibility = VisibilityEnum.Visible;
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

    private bool AnimatorIsPlaying(string stateName)
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }
}
