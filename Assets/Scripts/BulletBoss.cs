using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBoss : MonoBehaviour
{

    public float speed = 1f;
    public Vliegtuig player;
    public GameObject destroyParticle;

    public AudioClip explosion;

    void Update()
    {
        this.transform.Translate(0, -speed, 0);
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D botsing)
    {
        print(botsing.tag);
        if (botsing.gameObject.tag == "Player")
        {
            print("Hit");
            Destroy(this.gameObject);
            var particle = Instantiate(destroyParticle, botsing.transform.position, botsing.transform.rotation);
            Destroy(particle, 4f);
            botsing.gameObject.GetComponent<Vliegtuig>().TakeLive(GetComponent<Collider2D>());
            AudioSource.PlayClipAtPoint(explosion, transform.position, 0.7f);
        }
    }
}