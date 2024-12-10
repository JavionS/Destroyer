using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] private AudioClip Enter;
    private AudioSource _source;
    [SerializeField] private int sceneNumber;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            StopAllCoroutines();
            _source.PlayOneShot(Enter);
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
            
        }
    }
}
