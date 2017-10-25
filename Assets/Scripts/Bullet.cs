using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = 1f;
	public Vliegtuig player;
	public GameObject destroyParticle;

    public AudioClip explosion;

	void Update () {
		this.transform.Translate (0,speed,0);
	}
	void OnBecameInvisible() {
		Destroy(gameObject);
	}
    private void OnTriggerEnter2D(Collider2D botsing)
    {
		if (botsing.gameObject.tag == "Steen") {
			Destroy (this.gameObject);
			var particle = Instantiate (destroyParticle, botsing.transform.position, botsing.transform.rotation);
			Destroy (botsing.gameObject);
			Destroy (particle, 5f);
            AudioSource.PlayClipAtPoint(explosion, transform.position, 0.7f);
            player.manager.LazerAdd();
        }
        if (botsing.gameObject.tag == "Boss")
        {
            Destroy(this.gameObject);
            var particle = Instantiate(destroyParticle, botsing.transform.position, botsing.transform.rotation);
			Destroy (particle, 5f);
            AudioSource.PlayClipAtPoint(explosion, transform.position, 0.7f);
            player.manager.LazerAdd();
        }
    }
}