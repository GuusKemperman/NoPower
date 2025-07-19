using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
