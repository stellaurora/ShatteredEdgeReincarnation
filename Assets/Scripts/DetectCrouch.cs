using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectCrouch : MonoBehaviour
{
    public string ObjectTagName = "";
    public bool Obstruction;
    public GameObject Object;
    private Collider colnow;
    public bool Crouchable;
    public Animator animator;
    IEnumerator Hanson()
    {
        yield return new WaitForSeconds(1.2f);
        print("hello");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    void OnTriggerStay(Collider col)
    {
        if (ObjectTagName != "" && !Obstruction)
        {
            if (col.GetComponent<CustomTag>())
            {
                if (col.GetComponent<CustomTag>().IsEnabled)
                {
                    if (col != null && !col.isTrigger && col.GetComponent<CustomTag>().HasTag(ObjectTagName)) // checks if the object has the right tag
                    {
                        
                        Obstruction = true;
                        Object = col.gameObject;
                        if (Object.tag == "NextLevel")
                        {
                            animator.SetBool("Fading", true);
                            StartCoroutine("Hanson");
                        }  
                        if (Object.tag == "UncrouchPrevent")
                        {
                            Crouchable = false;
                        }
                        colnow = col;
                    }
                }
            }

        }


        if (ObjectTagName == "" && !Obstruction)
        {
            if (col != null && !col.isTrigger)
            {
                Obstruction = true;
                colnow = col;
            }

        }



    }


    private void Update()
    {

        if (Object == null || !colnow.enabled)
        {
            Obstruction = false;
        }
        if (Object != null)
        {
            if (!Object.activeInHierarchy)
            {
                Obstruction = false;
            }
        }
    }





    void OnTriggerExit(Collider col)
    {
        if (col == colnow)
        {
            Obstruction = false;
            Crouchable = true;
        }


    }

}
