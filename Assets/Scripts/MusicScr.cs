using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScr : MonoBehaviour
{
    private static MusicScr audioInstance;
    void Awake()
    {
        DontDestroyOnLoad(this);
        if (audioInstance == null)
        {
            audioInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}
