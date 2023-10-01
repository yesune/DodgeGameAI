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

    private PlayerAI playerAI;

    private NeuralNetwork.Network network;
    private NeuralNetwork.Network targetNetwork;

    

    void Start(){
        playerRigidbody = GetComponent<Rigidbody>();
        playerRigidbody.freezeRotation = true;
        playerAI = GameManager.instance.playerAI;
        network = GameManager.instance.persistent.GetNetwork();
        targetNetwork = GameManager.instance.persistent.getTargetNetwork();
    }

    void Update()
    {
        //float desiredXInput = Input.GetAxis("Horizontal");
        //float desiredZInput = Input.GetAxis("Vertical");
        
        //int[] res = playerAI.getAction();
        double action = playerAI.agent.play_step(network, playerAI.getEpsilon());
        playerAI.decrementEpsilon();
        int[] res = convertAction(action);
        float xInput = res[0];
        float zInput = res[1];
        
        

        float xSpeed = xInput * speed;
        float zSpeed = zInput * speed;

        Vector3 newVelocity = new Vector3(xSpeed, 0f, zSpeed);
        playerRigidbody.velocity = newVelocity;

        // We are going to stray away from the typical algorithm
        // We will train on the mini-batch first, and then add the future state
        // Hopefully the amount of time this takes to calculate will be enough time to generate a new state :)

        
        // doing nextState stuff
        playerAI.agent.readState();
        Vector3 targetPosition = new Vector3(0, 1, 0); // The target position you want to calculate the distance to
        float dstToTarget = Vector3.Distance(playerRigidbody.transform.position, targetPosition);
        double reward = 0.01f * 1 / (1 + dstToTarget);
        double isOver = 0.0;
        if (GameManager.instance.getIsGameOver()) { // useless code
            reward = -1f;
            isOver = 1.0;
        }
        playerAI.agent.post_step(playerAI.agent.getState(), (float)reward, isOver, action);
        playerAI.updateQ();
    }

    public void Die()
    {
        GameManager.instance.playerAI.onDeath();
        gameObject.SetActive(false);
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.EndGame();
    }

    public int[] convertAction(double action) {
        int xInput = 0;
        int zInput = 0;
        
        switch(action) 
        {
            case 0:
                break;
            case 1:
                zInput = 1;
                break;
            case 2:
                xInput = 1;
                zInput = 1;
                break;
            case 3:
                xInput = 1;
                break;
            case 4:
                xInput = 1;
                zInput = -1;
                break;
            case 5:
                zInput = -1;
                break;
            case 6:
                xInput = -1;
                zInput = -1;
                break;
            case 7:
                xInput = -1;
                break;
            case 8:
                xInput = -1;
                zInput = 1;
                break;
        }
        int[] res = new int[2];
        res[0] = xInput;
        res[1] = zInput;
        return res;
    }
}