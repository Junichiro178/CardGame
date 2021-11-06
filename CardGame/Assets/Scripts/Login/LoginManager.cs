using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    // 画面をタッチしたらログインする
    // コールバック関数で、FadeIOManagerにつなげる
    public void CallBackLogin()
    {
        FadeIOManager.instance.FadeOutToIn(() => Login());
    }

    void Login()
    {
        SceneManager.LoadScene("Menu");
    }
}
