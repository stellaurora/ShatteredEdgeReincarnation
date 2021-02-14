using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    IEnumerator Hanson()
    {
        yield return new WaitForSeconds(1.4f);
        print("hello");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGame()
    {
        animator.SetBool("Fading", true);
        StartCoroutine("Hanson");
    }
}