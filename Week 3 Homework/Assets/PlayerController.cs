using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;


namespace CharacterSpace
{
    public class PlayerController : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 6f;

        public Camera mainCamera;

        [SerializeField]
        Animator anim;

        [SerializeField]
        TextMeshProUGUI text_Score;

        int coinNumber;

        void Start()
        {
            coinNumber = GameObject.FindGameObjectsWithTag("Coin").Length;
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Coin"))
            {
                Destroy(collision.gameObject);

                coinNumber--;
            }
            else if (collision.gameObject.CompareTag("Enemy"))
            {
                SceneManager.LoadScene("SampleScene");
            }
        }

        void Update()
        {
            // Direction
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            // Move
            if (direction.magnitude >= 0.1f)
            {
                controller.Move(direction * speed * Time.deltaTime);

                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

                anim.SetBool("isWalking", true);
            }
            else
            {
                anim.SetBool("isWalking", false);
            }

            text_Score.text = "Remaining Coins: " + coinNumber;

            // Move Camera
            mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z - 10);

            // Face Mouse
            /*
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;

            if (groundPlane.Raycast(cameraRay, out rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
            */
        }
    }
}

