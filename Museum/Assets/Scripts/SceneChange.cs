using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public GameObject panel;
    public void LoadScene()
    {
        if(panel != null)
        {
            Destroy(panel);
        }
        SceneManager.LoadScene("Museum");
    }
}
