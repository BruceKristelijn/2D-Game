using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Mainmenu : MonoBehaviour {

    public Texture2D cursor; // Get a cursor to use

    public GameObject current; // The current active UI screen.
    public GameObject[] screens; // An array containing the UI screens.

    public Button[] buttons; // Array with buttons to use to play a level.

    public Texture2D fadeOutTexture; // the texture that will overlay the screen. This can be a black image or a loading graphic

    public AudioSource Alert1;	// Sound Source to play Alert1
    public AudioSource Alert2;	// Sound Source to play Alert2

    //Start method
    void Start () {
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);        //Set cursor to fancy cursor.
        SetLevels();	// A function to set all the level buttons.
	}
    void SetLevels()
    {
        int onLvl = PlayerPrefs.GetInt("onLvl", 0)+1;
        Debug.Log(onLvl.ToString());
        for (int i = 0; i < onLvl; i++)
        {
            buttons[i].interactable = true;
        }
    }
    public void LoadScene(int _sceneId)
    {
        StartCoroutine(SwitchScene(_sceneId));
        Alert2.Play();
    }
	//Function called from button to start the game.
	public void StartGame(int _lvl)
    {
        StartCoroutine(SwitchScreen(_lvl));    //Call another function to switch the screens
        switch (_lvl)
        {
            case 0:
                Alert1.Play();
                break;
            case 1:
                Alert2.Play();
                break;
            default:
                Alert1.Play();
                break;
        }
    }
	public void ResetData(){
		PlayerPrefs.DeleteAll ();
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}

    IEnumerator SwitchScene(int _scene)
    {
        BeginFade(1);
        yield return new WaitForSeconds(0.7f);
        SceneManager.LoadScene(_scene);
    }
    IEnumerator SwitchScreen(int _level)
    {
        BeginFade(1);
        yield return new WaitForSeconds(0.7f);
        current.SetActive(false);
        screens[_level].SetActive(true);
        current = screens[_level];
        yield return new WaitForSeconds(0.5f);
        BeginFade(-1);
    }

    public float fadeSpeed = 0.8f;  // the fading speed

    private int drawDepth = -1000;  // the texture's order in the draw hierarchy: a low number means it renders on top
    private float alpha = 1.0f;   // the texture's alpha value between 0 and 1
    private int fadeDir = -1;   // the direction to fade: in = -1 or out = 1

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
    }

    // sets fadeDir to the direction parameter making the scene fade in if -1 and out if 1
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }
}
