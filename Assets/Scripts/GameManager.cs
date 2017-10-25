using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public Texture2D normalCursor;
    public Vliegtuig vliegtuig;

    [TextArea]
    public string introText;
    public GameObject infoText;

    public Text introtextElement;
    public GameObject introPanel;

    public GameObject boss;

    public bool level1;
    public bool level2;
    public bool level3;

    public GUISkin guiStyle;    //GUI skin used to display text in GUI function.
    public Texture2D fadeOutTexture; // the texture that will overlay the screen. This can be a black image or a loading graphic

    private float fadeSpeed = 0.8f;  // the fading speed

    private int drawDepth = -1000;  // the texture's order in the draw hierarchy: a low number means it renders on top
    private float alpha = 1.0f;   // the texture's alpha value between 0 and 1
    private int fadeDir = -1;   // the direction to fade: in = -1 or out = 1

    bool levelFinished;

    bool finished = false;
    bool started = false;

    public Slider lazerSlider;

    void Awake()
    {
        //BeginFade(-1);
        Cursor.SetCursor(normalCursor, new Vector2(30, 0), CursorMode.Auto);
        StartGame();
    }
    private void Update()
    {
        if (finished)
        {
            if (Input.GetButtonDown("e"))
            {
                StartLevel();
            }
        }
    }
    void StartGame()
    {
        vliegtuig.enabled = false;
        StartCoroutine("TypeWriter");
    }
    IEnumerator TypeWriter()
    {

        for (int i = 0; i < introText.Length; i++)
        {
            introtextElement.text += introText[i];
            yield return new WaitForSeconds(0.05f);
        }
        finished = true;
        while (!started)
        {
            print("Boep");
            infoText.SetActive(!infoText.activeSelf);
            yield return new WaitForSeconds(0.5f);
        }
    }
    void StartLevel()
    {
        started = true;
        vliegtuig.enabled = true;
        introPanel.SetActive(false);
        if (level3)
        {
            boss.SetActive(true);
        }
    }
    public void LazerAdd()
    {
        if (level1) { lazerSlider.value += 0.02f; }
        if (level2) { lazerSlider.value += 0.01f; }
        if (level3) { lazerSlider.value -= 0.1f; print("Hi"); }
        checkLevel();
    }
    public void LazerSlider(float _value)
    {
        var fill = lazerSlider.fillRect;
        fill.GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, _value);
    }
    void checkLevel()
    {
        if (level1)
        {
            if (lazerSlider.value == 1)
            {
                StartCoroutine(levelEnd());
                int lvlid = PlayerPrefs.GetInt("onLvl", 0);
                if (lvlid == 0)
                {
                    PlayerPrefs.SetInt("onLvl", 1);
                }
            }
        }
        if (level2)
        {
            if (lazerSlider.value == 1)
            {
                StartCoroutine(levelEnd());
                int lvlid = PlayerPrefs.GetInt("onLvl", 0);
                if (lvlid == 1)
                {
                    PlayerPrefs.SetInt("onLvl", 2);
                }
            }
        }
        if (level3)
        {
            if (lazerSlider.value == 0)
            {
                StartCoroutine(killBoss());
                int lvlid = PlayerPrefs.GetInt("onLvl", 0);
                if (lvlid == 2)
                {
                    PlayerPrefs.SetInt("onLvl", 3);
                }
            }
        }
    }
    IEnumerator killBoss()
    {

        yield return new WaitForSeconds(2f);
        StartCoroutine(levelEnd());
    }
    IEnumerator levelEnd()
    {
        levelFinished = true;
        BeginFade(1);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
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
        if (levelFinished)
        {
            var centeredStyle = guiStyle.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.fontSize = 25;
            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height / 2 - 25, 500, 50), "Level Complete.", centeredStyle);
        }
    }

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }
}
