using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Variables related to player character movement
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    public float speed = 3.0f;    


    // Variables related to animation
    Animator animator;
    Vector2 moveDirection = new Vector2(1, 0);
    

    // Variables related to NPC dialogue
    public InputAction talkAction;

    public InputAction Inventory;

    // Start is called before the first frame update
    void Start()
    {
        MoveAction.Enable();
        

        talkAction.Enable();
        talkAction.performed += FindFriend;

        Inventory.Enable();
        Inventory.performed += ShowInventory;

        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        
    }
    // Update is called once per frame
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();


        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }

        animator.SetFloat("Move X", moveDirection.x);
        animator.SetFloat("Move Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);        


    }


    // FixedUpdate has the same call rate as the physics system
    void FixedUpdate()
    {
        Vector2 position = (Vector2)rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }    
    
    void FindFriend(InputAction.CallbackContext context)
    {
        Vector2 origin = rigidbody2d.position + Vector2.up * 0.2f;
        float InteractionDistance = 1.5f;
        RaycastHit2D hit = Physics2D.Raycast(origin, moveDirection, InteractionDistance, LayerMask.GetMask("Crops"));

        Vector2 end = origin + moveDirection * InteractionDistance;
        Debug.DrawLine(new Vector3(origin.x,origin.y, -1.0f),new Vector3(end.x,end.y, -1.0f), Color.red, 3.0f, false);
        
        if (hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();            
            if (character != null)
            {
                UIHandler.instance.DisplayDialogue();
            }

        }

    }

    void ShowInventory(InputAction.CallbackContext context)
    {
        UIHandler.instance.DisplayDialogue();
    }

}