using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MovePattern {Teleport, Straight, Lerp}
    [SerializeField]
    MovePattern movePattern = MovePattern.Teleport;
    [SerializeField]
    GameManager gameManager;

    Vector3  CubeHeight = new Vector3(0, 0.5f, 0);

    Vector3 currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = Vector3.zero;
        if (movePattern == MovePattern.Straight)
        {
            Vector3 targetDirection = currentTarget - transform.position;
            Vector3 moveDirection = targetDirection.normalized;
            Vector3 moveVector = moveDirection * 10f * Time.deltaTime;

            if (moveVector.sqrMagnitude > targetDirection.sqrMagnitude)
            {
                newPos = currentTarget;
            }
            else{
                newPos = transform.position + moveVector;
            }

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            }

            transform.position = newPos + CubeHeight;
        }
        else if (movePattern == MovePattern.Lerp)
        {
            newPos = Vector3.Lerp(transform.position, currentTarget, 2f * Time.deltaTime);

            Vector3 targetDirection = currentTarget - transform.position;
            Vector3 moveDirection = targetDirection.normalized;
            Vector3 moveVector = moveDirection * 10f * Time.deltaTime;

            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            }

            transform.position = newPos + CubeHeight;
        }

    }

    /// <summary>
    /// Move to the new point
    /// </summary>
    /// <param name="newPointTarget">New targeted position the player gonna move to</param>
    public void MoveTo(Vector3 newPointTarget)
    {
        if (movePattern == MovePattern.Teleport)
        {
            transform.position = newPointTarget + CubeHeight;
        }
        else if (movePattern == MovePattern.Straight)
        {
            currentTarget = newPointTarget;
        }
        else if (movePattern == MovePattern.Lerp)
        {
            currentTarget = newPointTarget;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {   
            Destroy(collision.gameObject);
            gameManager.score++;
        }
    }
}
