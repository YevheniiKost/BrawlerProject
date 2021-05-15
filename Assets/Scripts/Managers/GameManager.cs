using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<HeroSelecterData> HeroSelector = new List<HeroSelecterData>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        ServiceLocator.Register(this);
    }
}

[Serializable]
public class HeroSelecterData
{
    public CharacterName Name;
    public GameObject Prefab;
}
