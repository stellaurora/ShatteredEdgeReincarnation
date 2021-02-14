using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plsnokill : MonoBehaviour
{
    public GameObject heyahanson;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(heyahanson);
    }

}
