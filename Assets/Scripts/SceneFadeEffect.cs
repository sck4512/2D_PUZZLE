using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFadeEffect : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();   
    }

    public void LoadScene(int _SceneIndex)
    {
        StartCoroutine(FadeOut(_SceneIndex));
    }

    IEnumerator FadeOut(int _SceneIndex)
    {
        animator.SetTrigger("FadeOut");
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(_SceneIndex);
    }
}
