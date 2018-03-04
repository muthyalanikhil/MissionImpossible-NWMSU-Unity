using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isMoving;
    public GameObject laser;
    public int direction;
    SpriteRenderer flipAnim;
    Animator animator;
    Animator LaserGateAnimator;
    public float moveSpeed;
    public int health;
    Rigidbody2D rigid;
    public float jumpHeight;
    public bool grounded = false;
    public AudioClip laserAudioClip;
    public AudioClip jumpAudioClip;
    public AudioClip coinAudioClip;
    AudioSource audioSource;
    Text healthCount;
    Text coins;
    float currentHealth;
    float currentCoins;
    public bool canMove;
    public GameObject background;


    // Use this for initialization
    void Start()
    {
        canMove = true;
        bool isMoving = false;
        animator = gameObject.GetComponent<Animator>();
        LaserGateAnimator = GameObject.Find("LaserGate").GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        flipAnim = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        GameObject health = GameObject.FindGameObjectWithTag("health");
        GameObject coinsGameObject = GameObject.FindGameObjectWithTag("coinsCount");
        healthCount = health.GetComponent<Text>();
        coins = coinsGameObject.GetComponent<Text>();
        currentHealth = Convert.ToInt32(healthCount.text);
        currentCoins = Convert.ToInt32(coins.text);
    }

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        if (hori > 0.0f)
        {
            direction = 1;
            flipAnim.flipX = false;
            animator.SetBool("isMoving", true);
        }

        if (hori < 0.0f)
        {
            flipAnim.flipX = true;
            direction = -1;
            animator.SetBool("isMoving", true);
        }

        if (hori == 0.0f)
        {
            animator.SetBool("isMoving", false);

        }

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 spawnPos = transform.position;
            spawnPos.x += 1 * direction;
            Instantiate(laser, spawnPos, Quaternion.identity);
            audioSource.PlayOneShot(laserAudioClip, 0.7F);
        }
    }

    private void FixedUpdate()
    {
        if (currentHealth <= 0)
        {
            healthCount.text = "0";
            GameObject.FindGameObjectWithTag("gameOver").GetComponent<Text>().enabled = true;
            background.GetComponent<AudioSource>().enabled = false;
            canMove = false;
        }
        else
        {
            healthCount.text = currentHealth.ToString();
            coins.text = currentCoins.ToString();
        }

        if (Input.GetKey(KeyCode.D) && canMove)
        {
            rigid.AddForce(Vector3.right * moveSpeed);

        }
        if (Input.GetKey(KeyCode.A) && canMove)
        {
            rigid.AddForce(Vector3.left * moveSpeed);
        }


        if (Input.GetKey(KeyCode.W) && grounded && canMove)
        {
            rigid.AddForce(Vector3.up * jumpHeight);
            grounded = false;
            audioSource.PlayOneShot(jumpAudioClip, 0.7F);
        }
    }
    private void OnTriggerEnter2D (Collider2D target)
    {
        if (target.gameObject.tag == "coins")
        {
            target.gameObject.SetActive(false);
            audioSource.PlayOneShot(coinAudioClip, 0.3F);
            currentCoins = currentCoins + 1;
        }
        else if (target.gameObject.tag == "unlockKey")
        {
            target.gameObject.SetActive(false);
            LaserGateAnimator.enabled = true;
            LaserGateAnimator.SetBool("openGate", true);
            GameObject.Find("LaserGate").GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (target.gameObject.tag == "Goal")
        {
            if (currentCoins >= 19)
            {
                GameObject.FindGameObjectWithTag("WinTXT").GetComponent<Text>().enabled = true;
                canMove = false;
                background.GetComponent<AudioSource>().enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Floor")
        {
            grounded = true;
        }
        if (collision.collider.tag == "redLaser")
        {
            currentHealth = currentHealth - 1;
        }
    }
}

