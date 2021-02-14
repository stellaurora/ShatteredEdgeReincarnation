using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class Wowzas : MonoBehaviour
{
    public static float tsll;
    public GameObject parent;
    public bool gettime = false;
    public float curn = 0;
    public float final = 0;
    public TMPro.TextMeshProUGUI textmpro;
    // Start is called before the first frame update
    void Start()
    {
        tsll = Time.time;
        DontDestroyOnLoad(parent);
        
    }

    // Update is called once per frame
    void Update()
    {
        curn = Time.time - tsll;
        if(gettime){
            final = Time.time - tsll;
        }
        else{
            curn = Time.time - tsll;
            textmpro.text = curn.ToString();
        }
    }
}
