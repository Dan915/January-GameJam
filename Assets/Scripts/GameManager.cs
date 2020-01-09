using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int startingIndex = 1;
    public int currentIndex = 1;
    public int finalIndex = 1;

    public bool isRedInPortal = false;
    public bool isBlueInPortal = false;

    void Start()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void NextLevel()
    {
        isRedInPortal = false;
        isBlueInPortal = false;
        if(currentIndex == finalIndex) return;
        currentIndex++;
            UnityEngine.SceneManagement.SceneManager.LoadScene(currentIndex);
    }
}
