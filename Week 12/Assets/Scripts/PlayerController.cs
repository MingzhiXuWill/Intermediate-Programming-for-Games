using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Vector2 moveDirection;
    Rigidbody2D rB;

    [SerializeField]
    Animator anim;

    Transform mainCamera;

    Vector3 cameraOffset;

    private void Start()
    {
        rB = GetComponent<Rigidbody2D>();

        mainCamera = Camera.main.transform;
        cameraOffset = mainCamera.position - transform.position;
    }

    private void Update()
    {
        mainCamera.position = transform.position + cameraOffset;
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveDirection = ctx.ReadValue<Vector2>();

        rB.velocity = moveDirection * 3.5f;

        if (moveDirection != Vector2.zero)
        {
            anim.SetBool("running", true);
            if (moveDirection.x < 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else if (moveDirection.x > 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.transform.parent.GetComponent<Item>();
            item.Pickup();

        }
        
        else if (other.CompareTag("Exit"))
        {
            GameManager.instance.Escape();
        }
    }
}
