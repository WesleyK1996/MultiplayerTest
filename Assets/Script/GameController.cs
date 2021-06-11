using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    GameController Instance;

    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }
}
