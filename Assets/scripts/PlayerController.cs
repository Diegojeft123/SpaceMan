using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //variables del movimiento de personaje
    public float jumpForce = 13f;
    public float runningSpeed = 2f;
    private Rigidbody2D rigidBody;
    Animator animator;
    Vector3 startPosition;

    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";

    public LayerMask groundMask;

    private void Awake()
    {
        rigidBody = GetComponent <Rigidbody2D>();
        animator = GetComponent <Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {

        startPosition = this.transform.position;
    }

    public void StartGame(){
        animator.SetBool(STATE_ALIVE, true);
        animator.SetBool(STATE_ON_THE_GROUND, false);
        Invoke("RestartPosition", 0.1f);
    }

    void RestartPosition(){
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetButtonDown("Jump")) {
            Jump();
       }

        animator.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());

        Debug.DrawRay(this.transform.position, Vector2.down * 1.45f, Color.red);
    }

    void FixedUpdate() 
    {
        if(GameManager.sharedInstance.currentGameState== GameState.inGame)
        {
        if(rigidBody.velocity.x < runningSpeed){
            rigidBody.velocity = new Vector2(runningSpeed, //x
                                            rigidBody.velocity.y //y
                                            );
        }
        }else{//si no estamos en la partida
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    void Jump()
    {
        if(IsTouchingTheGround())
        {

        rigidBody.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
        }
    }
    //nos indica si el personaje esta o no tocando el suelo
    bool IsTouchingTheGround(){
        if(Physics2D.Raycast(this.transform.position,
                             Vector2.down,
                             1.45f, groundMask)){
                            //TODO: programar logica de contacto con el suelo
                           
                            return true;
                        }else {
                            //TODO: programar logica de no contacto
                            
                            return false;
                        }
    }

    public void Die(){
        this.animator.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

}
