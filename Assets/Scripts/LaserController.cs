using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
    public float move;
    float posMove;
    bool setDir = false;
    Vector3 playerPos;
    GameObject temp;
    PlayerController cont;
    int direction;
	// Use this for initialization
	void Start () {
        temp = GameObject.FindGameObjectWithTag("Player");
        cont = temp.GetComponent<PlayerController>();
        playerPos = temp.transform.position;
        posMove = move;

	}
	
	// Update is called once per frame
	void Update () {

        direction = cont.direction;
        if(direction == -1 && !setDir){
            move = -move;
            setDir = true;
        }
        else if(direction == 1 && !setDir){
            move = posMove;
            setDir = true;
        }

        transform.Translate(new Vector3(move,0 ,0));

        if(Vector3.Distance(transform.position, playerPos) > 20){
            Destroy(gameObject);
        }
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Enemy"){
            EnemyController ene = collision.collider.GetComponent<EnemyController>();
            ene.health -= 5;
            Destroy(gameObject);
        }
    }
}
