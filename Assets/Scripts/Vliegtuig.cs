using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vliegtuig : MonoBehaviour
{

    public GameManager manager;

    public float Speed = 6f;
    public float YSpeed = 6f;

    public float LeftLimit;
    public float RightLimit;
    public float UpLimit;
    public float DownLimit;

    public int lives;
    public GameObject[] livesDisplay;
    public GameObject shield;

    public GameObject childSprite;
    public GameObject bulletPrefab;
    public GameObject backgroundPrefab;

    public GameObject rockPref;
    public int amountOfRocks;
    public Sprite[] rocks;

    [Header("Audio")]
    public AudioClip shootSound;

    float posX = 0f;
    float posY = 0f;

    float interval = 0f;
    float liveInterval = 0f;
    float backgroundpos = 10f;

    bool dead = false;

    public bool gainLazer = true;

    public LineRenderer line;
    public Transform rayCaster;

    public Texture2D fadeOutTexture; // the texture that will overlay the screen. This can be a black image or a loading graphic

    private float fadeSpeed = 0.8f;  // the fading speed

    private int drawDepth = -1000;  // the texture's order in the draw hierarchy: a low number means it renders on top
    private float alpha = 1.0f;   // the texture's alpha value between 0 and 1
    private int fadeDir = -1;   // the direction to fade: in = -1 or out = 1

    void Start()
    {
        //Zet de player naar start positie.
        transform.position = new Vector2(posX, posY);

        if (manager.level1)
        {
            InvokeRepeating("SecUpdate", 0.0f, 2f);
        }
        if (manager.level2)
        {
            InvokeRepeating("SecUpdate", 0.0f, 0.9f);
        }
    }
    void SecUpdate()
    {
        //Maak de stenen
        createStones();
    }
    void Update()
    {
        //Move
        move();
        //Rotate
        rotate();
        //When space is pressed shoot.
        shoot();
        //Check the background
        background();

        /* IF STATEMENT OM TE KIJKEN OF JE OMHOOG EN OMLAAG MAG BEWEGEN.
		if (transform.position.y + Input.GetAxis ("BovenOnder") * Time.deltaTime * Speed > DownLimit && transform.position.y + Input.GetAxis ("BovenOnder") * Time.deltaTime * Speed < UpLimit)  {
			//transform.Translate (0, Input.GetAxis ("BovenOnder")*Time.deltaTime*Speed, 0);
		}*/

        //IF STATEMENT OM TE KIJKEN OF JE MAG ROTEREN EN DAN ROTEREN.
        /*if (transform.rotation.z + Input.GetAxis ("LinksRechts")*Time.deltaTime*100 < 30 && transform.rotation.z + Input.GetAxis ("LinksRechts")*Time.deltaTime*100 > -30)  {
			transform.Rotate (0, 0, Input.GetAxis ("LinksRechts")*Time.deltaTime*100); // Links en Rechts Bewegen.
		}	*/
    }
    void shoot()
    {
        if (Time.time > interval && Input.GetAxis("Shoot") > 0 && !manager.level2)
        {
            interval = Time.time + 0.3f;
            var bullet = Instantiate(bulletPrefab, childSprite.transform.position, childSprite.transform.rotation);
            //bullet.GetComponent<Rigidbody> ().AddForce (0, 500, 0);
            bullet.GetComponent<Bullet>().player = this;
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
            Destroy(bullet, 5.0f);
        }
        else
        {
            return;
        }
    }
    void rotate()
    {
        //Part for alligning the sprite towards the mouse.
        var mouse = Input.mousePosition;
        var screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        var offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        var angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        childSprite.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }
    void move()
    {
        //Statement om te kijken of player mag bewegen en dan ook te laten bewegen
        if (transform.position.x + Input.GetAxis("LinksRechts") * Time.deltaTime * Speed > LeftLimit && transform.position.x + Input.GetAxis("LinksRechts") * Time.deltaTime * Speed < RightLimit)
        {
            transform.Translate(Input.GetAxis("LinksRechts") * Time.deltaTime * Speed, 0, 0); // Omhoog en omlaag bewegen.
        }
        transform.Translate(0, Time.deltaTime * YSpeed, 0); //Beweeg vliegtuig omhoog
    }
    void background()
    {
        if (this.transform.position.y > backgroundpos)
        {
            backgroundpos += 9;
            var pos = Vector3.zero;
            pos.y = this.transform.position.y + 45;
            pos.z = 1;
            var parent = GameObject.Find("BackgroundParent").transform;
            Instantiate(backgroundPrefab, pos, this.transform.rotation, parent);
        }
    }
    void createStones()
    {
        if (manager.level2)
        {
            var pos = new Vector3(0, this.transform.position.y + 7, 0);
            int x = -4;
            int step = 12 / amountOfRocks;
            int range = Random.Range(0, 2);
            for (int i = 0; i < amountOfRocks; i++)
            {
                if (range == 1)
                {
                    if (i == 1 || i == 3)
                    {
                        pos.x = x;
                        var rocksprite = rocks[Random.Range(0, rocks.Length)];
                        var rock = Instantiate(rockPref, pos, Quaternion.identity);
                        rock.GetComponent<SpriteRenderer>().sprite = rocksprite;
                        Destroy(rock, 5f);
                    }
                } else
                {
                    if (i == 0 || i == 2)
                    {
                        pos.x = x;
                        var rocksprite = rocks[Random.Range(0, rocks.Length)];
                        var rock = Instantiate(rockPref, pos, Quaternion.identity);
                        rock.GetComponent<SpriteRenderer>().sprite = rocksprite;
                        Destroy(rock, 5f);
                    }
                }
                manager.LazerAdd();
                x += step;
            }
            return;
        }
        else
        {
            var pos = new Vector3(0, this.transform.position.y + 7, 0);
            int x = -4;
            int step = 12 / amountOfRocks;
            for (int i = 0; i < amountOfRocks; i++)
            {
                pos.x = x;
                pos.y = Random.Range(pos.y - 1.5f, pos.y + 1.5f);
                var rocksprite = rocks[Random.Range(0, rocks.Length)];
                var rock = Instantiate(rockPref, pos, Quaternion.identity);
                rock.GetComponent<SpriteRenderer>().sprite = rocksprite;
                Destroy(rock, 5f);
                x += step;
            }
        }
    }
    public void TakeLive(Collider2D other)
    {
        if (liveInterval > Time.time || dead)
        {
            return;
        }
        livesDisplay[lives].SetActive(false);
        lives--;
        if (lives < 0)
        {
            print("ded");
            StartCoroutine(KillPlayer());
            dead = true;
            return;
        }
        liveInterval = Time.time + 2;
        StartCoroutine("ActivateShield");
    }
    IEnumerator KillPlayer()
    {
        BeginFade(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Steen")
        {
            TakeLive(collision);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "BossBullet")
        {
            TakeLive(collision);
            Destroy(collision.gameObject);
        }
    }
    IEnumerator ActivateShield()
    {
        shield.SetActive(true);
        yield return new WaitForSeconds(2f);
        shield.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        Debug.DrawRay(transform.position, transform.up, Color.green);
    }

    void OnGUI()
    {
        // fade out/in the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        // force (clamp) the number to be between 0 and 1 because GUI.color uses Alpha values between 0 and 1
        alpha = Mathf.Clamp01(alpha);

        // set color of our GUI (in this case our texture). All color values remain the same & the Alpha is set to the alpha variable
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;                // make the black texture render on top (drawn last)
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);  // draw the texture to fit the entire screen area
        if (dead)
        {
            var centeredStyle = manager.guiStyle.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 25, 800, 50), "You died. Please try again.", centeredStyle);
        }
    }

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }
}
