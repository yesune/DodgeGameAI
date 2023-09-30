using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRigidbody; //�̵��� ����� ������Ʈ
    public float speed = 8f;

    private float xInput = 0.0f;
    private float zInput = 0.0f;

    private static float smoothSpeed = 3.0f;

    

    void Start(){
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        
    }

    void Update()
    {
        //float desiredXInput = Input.GetAxis("Horizontal");
        //float desiredZInput = Input.GetAxis("Vertical");
        
        int[] res = GameManager.instance.playerAI.getAction();
        float xInput = res[0];
        float zInput = res[1];
        

        float xSpeed = xInput * speed;
        float zSpeed = zInput* speed;

        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidbody.velocity = newVelocity;
        
        GameManager.instance.playerAI.updateQ();
    }

    public void Die()
    {
        GameManager.instance.playerAI.onDeath();
        gameObject.SetActive(false);
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EndGame();
    }
}