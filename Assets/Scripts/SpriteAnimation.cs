using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// The SpriteAnimation requires an Image script to render towards.
[RequireComponent(typeof(Image))]
public class SpriteAnimation : MonoBehaviour {

    public Sprite[] sprites;
    public float interval;

    public bool PlayOnAwake = true;
    public bool Loop = false;

	void Start () {
        if (PlayOnAwake)
        {
            PlaySpriteAnimation();
        }
	}
    public void PlaySpriteAnimation()
    {
        StartCoroutine(play());
    }
    IEnumerator play()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            this.GetComponent<Image>().sprite = sprites[i];
            yield return new WaitForSeconds(interval);
        }
        if (Loop)
        {
            StartCoroutine(play());
        }
    }
}
