using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuwu : MonoBehaviour
{
    public Wowzas wowha;
    public GameObject tima;
    public Animator animator;
    public TMPro.TextMeshProUGUI hey;
    IEnumerator Hanson()
    {
        yield return new WaitForSeconds(1.4f);
        print("hello");
        SceneManager.LoadScene(1);
    }
    IEnumerator Hansonduo()
    {
        yield return new WaitForSeconds(0.001f);
        hey.text = wowha.final.ToString();
        Destroy(tima);
    }
    public void Meowster()
    {
        print("hello");
        animator.SetBool("Fading", true);
        StartCoroutine("Hanson");
    }
    public void Nyaster()
    {
        print(";w;");
        Application.Quit();
    }
    public void Start()
    {
        tima = GameObject.Find("TimeyWimey");
        wowha = tima.GetComponent<Wowzas>();
        wowha.gettime = true;
        SceneManager.MoveGameObjectToScene(tima, SceneManager.GetActiveScene());
        StartCoroutine("Hansonduo");
    }
}