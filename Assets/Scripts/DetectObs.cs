using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectObs : MonoBehaviour
{
    public string ObjectTagName = "";
    public bool Obstruction;
    public GameObject Object;
    private Collider colnow;
    public PlayerController playercontroller;
    public GameObject stoppuwatcho;

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
                        if (gameObject.name == "DetectGround") { playercontroller.previousobject = null; }
                        if(Object.tag == "DeathBarrier"){
                            if (SceneManager.GetActiveScene().buildIndex == 1)
                            {
                                SceneManager.MoveGameObjectToScene(stoppuwatcho, SceneManager.GetActiveScene());
                            }
                            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        
        if(Object == null || !colnow.enabled)
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
        }

    }

}
