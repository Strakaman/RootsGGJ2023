using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    bool tryingToLoad = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartHunt(InputAction.CallbackContext callbackContext)
    {
        if (!tryingToLoad)
        {
            tryingToLoad = true;
            SceneManager.LoadScene(1);
        }
    }
}
