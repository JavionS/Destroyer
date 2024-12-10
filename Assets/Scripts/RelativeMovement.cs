using UnityEngine;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine.VFX;
public class RelativeMovement : MonoBehaviour
{
    [SerializeField] Transform _target;
    
    [SerializeField] float _moveSpeed = 5.0f;
    [SerializeField] float _rotSpeed = 15.0f;

    [SerializeField] float _jumpSpeed = 9.0f;
    [SerializeField] float _gravity = -9.8f;
    [SerializeField] float _terminalVelocity = -20.0f;
    [SerializeField] float _minFall = -4f;
    
    [SerializeField] private LayerMask _buildingLayer;
    //[SerializeField] private float _rayDistance = 1.0f;
    
    float _vertSpeed;
    float _groundCheckDistance;
    ControllerColliderHit _contact;

    CharacterController _charController;
    Animator _animator;
    private Transform _character;

    private float _defaultSpeed;
    private Quaternion _lockedRotation;
    private bool _attack;
    
    [SerializeField] private VisualEffect _vfxDetect;
    [SerializeField] private VisualEffect[] _vfxHits = new VisualEffect[2];
    private float _detectSize = 0.1f;
    private float _hitSize = 0.15f;
    
    private AudioSource _audio;
    
    
    [SerializeField] private CinemachineOrbitalFollow _cinemachineOrbital;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        
        _character = GetComponent<Transform>();
        _charController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _animator.SetBool("Die", false);

        _defaultSpeed = _moveSpeed;
        
        _vertSpeed = _minFall;
        _groundCheckDistance =
            (_charController.height + _charController.radius) /
            _charController.height * 0.1f;
        
        _vfxDetect.SetFloat("Size", _detectSize);
        foreach (var lazer in _vfxHits)
        {
            lazer.SetFloat("Size", _hitSize);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = Vector3.zero;
    
    
        float horInput = Input.GetAxis("Horizontal");
        float vertInput = Input.GetAxis("Vertical");
        
        _animator.SetBool("Attack", GameBehavior.Instance.LaserEye ? true:false);
        
        if (horInput != 0 || vertInput != 0)
        {
            Vector3 right = _target.right;
            Vector3 forward = Vector3.Cross(right, Vector3.up);

            movement = (right * horInput) + (forward * vertInput);
            movement *= _moveSpeed;
            movement = Vector3.ClampMagnitude(movement, _moveSpeed);

            if (GameBehavior.Instance.isAlive && !GameBehavior.Instance.LaserEye)
            {
                Quaternion direction = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation, direction, _rotSpeed * Time.deltaTime);
            }
            
        }
        _animator.SetFloat("Speed", movement.sqrMagnitude);
        
        bool hitGround = false;
        if (_vertSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            hitGround = hit.distance <= _groundCheckDistance;
        }
            

        if (hitGround)
            if (Input.GetButtonDown("Jump"))
            {
                _vertSpeed = _jumpSpeed;
                _animator.SetBool("Jumping", true);
            }
            
            else
            {
                _vertSpeed = _minFall;
                _animator.SetBool("Jumping", false);
            }

        else
        {_vertSpeed += _gravity * 3 * Time.deltaTime;

            if (_vertSpeed < _terminalVelocity)
                _vertSpeed = _terminalVelocity;

            if (_charController.isGrounded)
            {
                if (Vector3.Dot(movement, _contact.normal) < 0)
                    movement = _contact.normal * _moveSpeed;
                else
                    movement += _contact.normal * _moveSpeed;
            }
        }
        movement.y = _vertSpeed;

        if (GameBehavior.Instance.isAlive && !GameBehavior.Instance.LaserEye /*&& !IsObstacleAhead(movement)*/)
        {
            _charController.Move(movement * Time.deltaTime);
        }

        if (!GameBehavior.Instance.isAlive)
        {
            _moveSpeed = 0;
            _animator.SetBool("Die", true);
        }
        
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contact = hit;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyDrink"))
        {
            DrinkBehavior drink = other.gameObject.GetComponent<DrinkBehavior>();
            _moveSpeed += drink.Speed();
            _jumpSpeed += drink.jumpSpeed();
            
            _audio.PlayOneShot(drink.EatSound());
            
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Fruit"))
        {
            FoodBehavior food = other.gameObject.GetComponent<FoodBehavior>();
            // Destroy(other.gameObject);
            float maxScale = (Vector3.one * 20f).magnitude;
            Debug.Log(maxScale);
            if (transform.localScale.magnitude < maxScale)
            {
                Debug.Log(transform.localScale.magnitude);
                //Scale
                transform.localScale += Vector3.one * 0.3f;

                _moveSpeed += food.Speed();
                _jumpSpeed += food.jumpSpeed();
                
                //Camera
                _cinemachineOrbital.Radius += 0.8f;
                _cinemachineOrbital.TargetOffset.y += 0.3f;
                
                _charController.center = new Vector3(
                    _charController.center.x,
                    _charController.height / 2f,
                    _charController.center.z);
                
                //vfx
                _detectSize += 0.02f;
                _hitSize += 0.02f;
                _vfxDetect.SetFloat("Size", _detectSize);
                foreach (var lazer in _vfxHits)
                {
                    lazer.SetFloat("Size", _hitSize);
                }
                
                //Audio
                _audio.PlayOneShot(food.EatSound());
                
                Destroy(other.gameObject);
            }
            

            
        }
    }
}
