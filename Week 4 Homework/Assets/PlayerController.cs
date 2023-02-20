using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;



public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;

    public Camera mainCamera;

    [SerializeField]
    Animator anim;

    [SerializeField]
    TextMeshProUGUI text_Score;

    private float fireTimer;
    [SerializeField]
    public float fireTotal;

    [HideInInspector]
    public int score = 0;

    public float bulletSpeed = 10f;

    public GameObject bulletPrefab;
    public GameObject bulletStart;

    struct BoxStructExample
    {
        public int score;
        public BoxStructExample(int s)
        {
            score = s;
        }
    }

    BoxStructExample boxType1 = new BoxStructExample(10);
    BoxStructExample boxType2 = new BoxStructExample(20);
    BoxStructExample boxType3 = new BoxStructExample(30);

    BoxStructExample[] Boxes = new BoxStructExample[3];

    void Start()
    {
        Boxes[0] = boxType1;
        Boxes[1] = boxType2;
        Boxes[2] = boxType3;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Box"))
        {
            Destroy(collision.gameObject);

            score += Boxes[Random.Range(0, Boxes.Length)].score;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    void fireBullet(Vector3 direction)
    {
        GameObject b = Instantiate(bulletPrefab);
        b.transform.position = bulletStart.transform.position;
        b.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;
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

        //Debug.Log(score);
        text_Score.text = "Score: " + score;

        // Move Camera
        mainCamera.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z - 10);

        // Face Mouse
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;
        Vector3 pointToLook = Vector3.zero;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            pointToLook = cameraRay.GetPoint(rayLength);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }

        if (fireTimer >= 0)
        {
            fireTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (fireTimer <= 0)
            {
                Vector3 forward = transform.TransformDirection(Vector3.forward);

                fireBullet(forward);

                fireTimer = 0;
            }
        }
    }
}

