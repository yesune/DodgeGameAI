using UnityEngine;
using System.Collections.Generic;

// https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague
// for the cone vision i wanted very badly

// Strategy here is to have one circular vision instead, and to store the 8 closest bullets

// I could use a K-D tree, but honsetly there aren't that many bullets so I should be fine
public class Vision: MonoBehaviour
{
    // You can add some code later if you want to track the position of the closest bullet
    // If you ever do that, change this to ontriggerstay
    // For now, we'll see how the AI does with just the vision
    public bool collision;
    public int counter;
    public HashSet<Bullet> bullets;

    public void Awake() {
        collision = false;
        counter = 0;
        bullets = new HashSet<Bullet>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Bullet(Clone)")
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet != null) {
                //Debug.Log("bullet found" + bullet.transform.position.x);
                bullets.Add(bullet);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name == "Bullet(Clone)")
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            if (bullet != null) {
                //Debug.Log("bullet left" + bullet.transform.position.x);
                bullets.Remove(bullet);
            }
        }
    }

    // who needs k-d trees amiright
    public bool getCollision() {
        float closestDistance = float.MaxValue;
        Bullet closestBullet = null;
        foreach (var bullet in bullets) {
            if (bullet == null){ // can't believe i spent 30 minutes just for this line
                continue;
            }
            else if(Vector3.Distance(bullet.transform.position,
             FindObjectOfType<PlayerController>().transform.position) < closestDistance) {
                closestBullet = bullet;
                closestDistance = Vector3.Distance(bullet.transform.position,
                 FindObjectOfType<PlayerController>().transform.position);
             }
        }
        return bullets.Count > 0;
    }
}