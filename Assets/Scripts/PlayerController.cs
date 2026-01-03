using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    /* Las variables para Moviles
    public Joystick joystick;
    float horizontalTouchMove;
    float verticalTouchMove;
    */
    public bool canMove = true;
    public bool isTalking;
    public static bool playerCreated = false;
    

    public float speed = 5.0f;
    
    private bool walking = false;
    public bool Walking
    {
        get
        {
            return walking;
        }
        set
        {
            walking = value;
        }
    }
    private bool attacking = false;
    public bool isAttacking
    {
        get
        {
            return attacking;
        }
        set
        {
            attacking = value;
        }
    }
    public Vector2 lastMovement = Vector2.zero;

    private const string AXIS_H = "Horizontal",
                         AXIS_V = "Vertical",
                         WALK = "Walking",
                         ATT = "Attacking",
                         LAST_H = "LastH", 
                         LAST_V = "LastV";

    private Animator _animator;
    private Rigidbody2D _rigidbody;
    public string nextUuid;

    public float attackTime;
    private float attackTimeCounter;
    private WeaponManager weaponManager;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        weaponManager = GetComponentInChildren<WeaponManager>();

        playerCreated = true;
        isTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*Se inicializan las variables para moviles
        horizontalTouchMove = joystick.Horizontal;
        verticalTouchMove = joystick.Vertical;
        */

        if (isTalking)
        {
            _rigidbody.velocity = Vector2.zero;
            return;
        }


        walking = false;
        if (!canMove)
            return;

        if (attacking)
        {
            attackTimeCounter -= Time.deltaTime;
            if (attackTimeCounter < 0)
            {
                attacking = false;
                _animator.SetBool(ATT, false);
            }

        }
        else
        {
            if (Input.GetMouseButtonDown(0) && weaponManager.WeaponEquipped && !EventSystem.current.IsPointerOverGameObject()
)
            {
                SFXManager.SharedInstance.PlaySFX(SFXType.SoundType.ATTACK);
                attacking = true;
                attackTimeCounter = attackTime;
                _rigidbody.velocity = Vector2.zero;
                _animator.SetBool(ATT, true);
            }
        }


        
        //S = V*t

        // EL MOVIMIENTO DE ORDENADOR
            
            if (Mathf.Abs(Input.GetAxisRaw(AXIS_H)) > 0.2)
            {
                //Vector3 translation = new Vector3(Input.GetAxisRaw(AXIS_H)* speed * Time.deltaTime, 0, 0);
                //this.transform.Translate(translation);
                _rigidbody.velocity = new Vector2(Input.GetAxisRaw(AXIS_H), _rigidbody.velocity.y).normalized * speed;
                this.walking = true;
                lastMovement = new Vector2(Input.GetAxisRaw(AXIS_H), 0);


                if (Mathf.Abs(Input.GetAxisRaw(AXIS_V)) < 0.2 && (Mathf.Abs(Input.GetAxisRaw(AXIS_H)) > 0.2))
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0).normalized * speed;
                }



            }

            if (Mathf.Abs(Input.GetAxisRaw(AXIS_V)) > 0.2)
            {
                //Vector3 translation = new Vector3(0, Input.GetAxisRaw(AXIS_V) * speed * Time.deltaTime, 0);
                //this.transform.Translate(translation);
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Input.GetAxisRaw(AXIS_V)).normalized * speed;
                this.walking = true;
                lastMovement = new Vector2(0, Input.GetAxisRaw(AXIS_V));

                if (Mathf.Abs(Input.GetAxisRaw(AXIS_H)) < 0.2 && (Mathf.Abs(Input.GetAxisRaw(AXIS_V)) > 0.2))
                {
                    _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y).normalized * speed;
                }

            }

        /* EL SIGUIENTE CONTROL ES EL DE MOVILES    
        //EL MOVIMIENTO DE MOVILES
            if (Mathf.Abs(horizontalTouchMove) > 0.2)
            {
                //Vector3 translation = new Vector3(Input.GetAxisRaw(AXIS_H)* speed * Time.deltaTime, 0, 0);
                //this.transform.Translate(translation);
                _rigidbody.velocity = new Vector2(horizontalTouchMove, _rigidbody.velocity.y).normalized * speed;
                this.walking = true;
                lastMovement = new Vector2(horizontalTouchMove, 0);


                if (Mathf.Abs(verticalTouchMove) < 0.2 && (Mathf.Abs(horizontalTouchMove) > 0.2))
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0).normalized * speed;
                }



            }

            if (Mathf.Abs(verticalTouchMove) > 0.2)
            {
                //Vector3 translation = new Vector3(0, Input.GetAxisRaw(AXIS_V) * speed * Time.deltaTime, 0);
                //this.transform.Translate(translation);
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, verticalTouchMove).normalized * speed;
                this.walking = true;
                lastMovement = new Vector2(0, verticalTouchMove);

                if (Mathf.Abs(horizontalTouchMove) < 0.2 && (Mathf.Abs(verticalTouchMove) > 0.2))
                {
                    _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y).normalized * speed;
                }

            }
            */         
    }

    private void LateUpdate()
    {
        if (!walking)
        {
            _rigidbody.velocity = Vector2.zero;
        }
        _animator.SetFloat(AXIS_H, Input.GetAxisRaw(AXIS_H));
        _animator.SetFloat(AXIS_V, Input.GetAxisRaw(AXIS_V));
        //Las dos siguientes lineas son para el control de moviles, hara que las animaciones se reproduzcan bien hacia cada lado
        //_animator.SetFloat(AXIS_H, horizontalTouchMove);
        //_animator.SetFloat(AXIS_V, verticalTouchMove);
        _animator.SetBool(WALK, walking);
        _animator.SetFloat(LAST_H, lastMovement.x);
        _animator.SetFloat(LAST_V, lastMovement.y);
    }
}
