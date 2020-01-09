using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System.Linq;

public enum DreamType
{
    GOOD_DREAM,
    BAD_DREAM
}

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    public uint playerId = 0;
    public DreamType dreamType;
    
    public float moveSpeed = 3.0f;
    public float jumpStrength = 5.0f;
    public float rayLength = 0.2f;
    public LayerMask GroundMask;
    public Sprite interactionSprite;
    public bool isLookingRight = true;

    public float interactionRadius = 1.0f;

    private float _horizontalInput, _gravity;
    private bool _jump, _interact, _isGrounded, _canInteract, _bounced;
    private Player _player;
    private Rigidbody2D _rigidBody;
    public GameObject _interactionObject;
    public GameObject touchingParticleEffect;

    public enum DreamState { Neutral, GOOD, BAD };

    public DreamState _playerState;

    private Animator heroAnim;

    private void Start()
    {

        heroAnim = gameObject.GetComponent<Animator>();
        _player = ReInput.players.GetPlayer((int)playerId);
        _rigidBody = GetComponent<Rigidbody2D>();
        _gravity = Physics.gravity.y;
        if (playerId == 0) _playerState = DreamState.GOOD;
        else if (playerId == 1) _playerState = DreamState.BAD;
        else _playerState = DreamState.Neutral;


        _interactionObject = transform.GetComponentsInChildren<Transform>(true).FirstOrDefault(x=>x.name == "Interaction_Indicator").gameObject;
        if(interactionSprite != null) _interactionObject.GetComponent<SpriteRenderer>().sprite = interactionSprite;
        _interactionObject.SetActive(false);
    }

    private void Update()
    {
        
        GetInput();
        ProcessInput();
        ForwardFacingRay();
    }

    private void ForwardFacingRay()
    {
        if(_bounced) return;
        var dist = 0.65f;
        Debug.DrawRay(transform.position, transform.right.normalized * dist, Color.red);
        var hit = Physics2D.Raycast(transform.position, transform.right, dist);
        if(hit && hit.transform.tag == "Player" && !hit.transform.GetComponent<PlayerController>().IsGrounded() && !_isGrounded)
        {
            
            // var hitRigid = hit.transform.GetComponent<Rigidbody2D>();
            // if(_rigidBody.velocity.y < 0 || _rigidBody.velocity.y > 0.5) return;
            // if(hitRigid.velocity.y < 0 || hitRigid.velocity.y > 0.5) return;

            _rigidBody.velocity = new Vector2((_rigidBody.velocity.x * -0.5f), _rigidBody.velocity.y);
            _bounced = true;
             var particlePosition = new Vector2(transform.position.x + (hit.transform.position.x - transform.position.x) / 2, transform.position.y);

            var particles = Instantiate(touchingParticleEffect, particlePosition, Quaternion.identity);
            Destroy(particles, particles.GetComponent<ParticleSystem>().main.duration);
        }
    }

    public void CanInteract(bool state)
    {
        _canInteract = state;
        _interactionObject.SetActive(state);
    }

    private void FixedUpdate()    
    {
            if(!_bounced)
                _rigidBody.velocity = new Vector3(_horizontalInput * moveSpeed, _rigidBody.velocity.y, 0.0f);
    } 
    

    private void GetInput()
    {
        _horizontalInput = _player.GetAxis("Move Horizontal");

        heroAnim.SetFloat("Movement", Mathf.Abs(_player.GetAxis("Move Horizontal")));

        _jump = _player.GetButtonDown("Jump");
        _interact = _player.GetButtonDown("Interact");
    }

    private void ProcessInput()
    {
        _isGrounded = Physics2D.Linecast(transform.position, transform.position + new Vector3(0.0f, rayLength), GroundMask);
        if(_isGrounded && _bounced) _bounced = false;

        if(_horizontalInput != 0.0f)
        {
            transform.right = new Vector2(_horizontalInput, 0.0f);
            isLookingRight = _horizontalInput > 0;
        }

        if(_jump && _isGrounded)
        {
            _rigidBody.AddForce(Vector3.up * Mathf.Sqrt(jumpStrength * -2f * Physics.gravity.y), ForceMode2D.Impulse);
            heroAnim.SetBool("Jump", true);
        }
        else if (!_isGrounded)
            heroAnim.SetBool("Jump", true);
        else 
            heroAnim.SetBool("Jump", false);

        if(_interact && _canInteract)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
            foreach(var item in colliders.Where(x=>x.tag == "Interactable"))
            {
                item.GetComponent<InteractableObject>().Use(gameObject);
            }
        }
    }

    public bool IsGrounded()
        => _isGrounded;
}
