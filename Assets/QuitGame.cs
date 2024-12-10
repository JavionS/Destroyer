using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class QuitGame : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitApp();
        }
    }
    
    void QuitApp()
    {
        #if UNITY_EDITOR
        // Set variable if game is running from Unity
        EditorApplication.isPlaying = false;
        #else 
            Application.Quit();
        #endif
    }
}
