using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class beweegBal : MonoBehaviour {

	public Text scoreDisplay;
	public hoofdScript hoofd;

	float snelheid = 4f;
	int score = 0;

	void Start(){
		hoofd = Camera.main.gameObject.GetComponent<hoofdScript> ();
	}

	void Update () {
		transform.Translate (Input.GetAxis ("LinksRechts") * Time.deltaTime * snelheid, Input.GetAxis ("BovenOnder") * Time.deltaTime * snelheid, 0);
	}

	void OnCollisionEnter2D(Collision2D botsing){
		print (botsing.gameObject.tag);
		Destroy (botsing.gameObject);
		giveScore ();
	}
	void giveScore(){
		score++;
		scoreDisplay.text = "Score: "+score.ToString ();
		hoofd.createBlokje ();
	}
}
