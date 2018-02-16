using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
    bool seePlayer;
    public int moveRange;
    public float speed;
    public int health;
    float tmpSpeed;
    public int direction;
    public float shootTime;
    float tmpTime;
    Vector3 startPos;
    GameObject player;
    public GameObject laser;
    SpriteRenderer flipAnim;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        seePlayer = false;
        startPos = transform.position;
        flipAnim = GetComponent<SpriteRenderer>();
        tmpTime = shootTime;
	}
	
	// Update is called once per frame
	void Update () {
        if(Vector3.Distance(transform.position, startPos) > (moveRange / 2)){
            speed = -speed;
        }

        if (speed < 0) {
            flipAnim.flipX = true;
            direction = -1;
        } 

        else {
            flipAnim.flipX = false;
            direction = 1;
        }

        transform.Translate(new Vector3(speed, 0, 0));

        if(seePlayer){
            if(shootTime <= 0){
                Vector3 spawnPos = transform.position;
                spawnPos.x += 1 * direction;
                Instantiate(laser, spawnPos, Quaternion.identity);
                shootTime = tmpTime;
            }

            shootTime -= Time.deltaTime;
        }

        if(Vector3.Distance(transform.position, player.transform.position) < 10){
            seePlayer = true;
        }
        else{
            seePlayer = false;
        }

        if(health <= 0){
            Destroy(gameObject);
        }
	}
}
