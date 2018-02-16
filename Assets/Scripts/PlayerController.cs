using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public bool isMoving;
    public GameObject laser;
    public int direction;
    SpriteRenderer flipAnim;
    Animator animator;
    Animator LaserGateAnimator;
    public float moveSpeed;
    Rigidbody2D rigid;
    public float jumpHeight;
    public bool grounded = false;
    public int health;
    public AudioClip laserAudioClip;
    public AudioClip jumpAudioClip;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        bool isMoving = false;
        animator = gameObject.GetComponent<Animator>();
        LaserGateAnimator = GameObject.Find("LaserGate").GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        flipAnim = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (hori > 0.0f)
        {
            direction = 1;
            flipAnim.flipX = false;
            animator.SetBool("isMoving", true);
        }

        if(hori < 0.0f){
            flipAnim.flipX = true;
            direction = -1;
            animator.SetBool("isMoving", true);
        }

        if(hori == 0.0f)
        {
            animator.SetBool("isMoving", false);

        }

        if(Input.GetButtonDown("Fire1")){
            Vector3 spawnPos = transform.position;
            spawnPos.x += 1 * direction;
            Instantiate(laser, spawnPos, Quaternion.identity);
            audioSource.PlayOneShot(laserAudioClip, 0.7F);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.D))
        {
            rigid.AddForce(Vector3.right * moveSpeed);

        }


        if (Input.GetKey(KeyCode.A))
        {
            rigid.AddForce(Vector3.left * moveSpeed);
        }


        if (Input.GetKey(KeyCode.W) && grounded)
        {
            rigid.AddForce(Vector3.up * jumpHeight);
            grounded = false;
            audioSource.PlayOneShot(jumpAudioClip, 0.7F);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Floor"){
            grounded = true;
        }
        if (collision.collider.tag == "unlockKey")
        {
            Destroy(collision.gameObject);
            LaserGateAnimator.enabled = true;
            LaserGateAnimator.SetBool("openGate", true);
            GameObject.Find("LaserGate").GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
