using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour {

    public float speed;

    public float LeftLimit;
    public float RightLimit;

    public Transform[] barrels;

    public GameObject Bullet;

    public bool left = true;
    public bool right = false;

    private void Start()
    {
        InvokeRepeating("UpdateOnce", 0.0f, 1f);
    }
    private void Update()
    {
        if (left)
        {
            Vector3 pos = new Vector3();
            pos.x = (transform.position.x + -1 * Time.deltaTime * speed);
            pos.y = 13.14f;
            this.transform.position = pos;
            if(pos.x <= LeftLimit)
            {
                left = false;
                right = true;
            }
        }
        if (right)
        {
            Vector3 pos = new Vector3();
            pos.x = (transform.position.x + 1 * Time.deltaTime * speed);
            pos.y = 13.14f;
            this.transform.position = pos;
            if (pos.x >= RightLimit)
            {
                left = true;
                right = false;
            }
        }
    }
    private void UpdateOnce()
    {
        Shoot();
    }
    private void Shoot()
    {
        print("shoot");
        for (int i = 0; i < barrels.Length; i++)
        {
            Vector3 pos = barrels[i].transform.position;
            var bullet = Instantiate(Bullet, pos, this.transform.rotation, this.transform.parent);
            Destroy(bullet, 5f);
        }
    }
}
