using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    static BackgroundMusic instance;
    void Start()
    {
        if (!instance) instance = this;
        else if(instance!=this) Destroy(this.gameObject);
        
        DontDestroyOnLoad(this.gameObject);
    }

 
   
}
