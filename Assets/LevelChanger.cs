using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator anim;

    private int levelToLoad;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeToLevel(int index)
    {
        levelToLoad = index;
        anim.SetTrigger("fadeOut");
    }

    public void FadeOut()
    {
        anim.SetTrigger("fadeOut");
    }

    public void FadeIn()
    {
        anim.SetTrigger("fadeIn");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
