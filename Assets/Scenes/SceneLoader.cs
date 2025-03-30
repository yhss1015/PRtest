using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TimeScaleChnage(float scale)
    {
        Time.timeScale = scale;
    }

    public void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중이면 플레이 중지
#else
            Application.Quit(); // 빌드된 게임에서는 종료
#endif
    }
}
