using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // 画面をタッチしたらログインする
    public void Login()
    {
        SceneManager.LoadScene("Menu");
    }
}
