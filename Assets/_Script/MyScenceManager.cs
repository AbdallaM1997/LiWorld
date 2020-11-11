using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyScenceManager : MonoBehaviour
{
    public void SelectScence(string scenceName)
    {
        SceneManager.LoadScene(scenceName);
    }
}
