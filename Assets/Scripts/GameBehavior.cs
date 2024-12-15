using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    public Utilities.GameplayState State = Utilities.GameplayState.Play;
    public Player player;
    public int MaxScore = 15000;
    
    //Player State
    public bool isAlive = true;
    
    public int MaxEnergy = 3;

    public int InitialHeight = 0;
    
    //lazer Eye?
    public bool LaserEye = false;
    public bool hasEnoughEnergy = false;
    
    //GUI
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private TextMeshProUGUI _stateMessage;
    
    //Score
    private int _totalScores;
    private int _totalTargets;
    [HideInInspector] public int _destroyedTargets = 0;
    
    [SerializeField] GameObject _ghostPrefab;
    private GameObject _ghost;
    private float timer = 0f;
    
    private AudioSource _audio;
    [SerializeField] private AudioClip[] _collisionSFX = new AudioClip[4];
    [SerializeField] private AudioClip _laserSFX;
    [SerializeField] private AudioClip _winSFX;
    [SerializeField] private AudioClip _loseSFX;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }
    
    void Start()
    {
        Time.timeScale = 1; 
        
        _stateMessage.enabled = false;
        _restartButton.SetActive(false);

        _totalTargets = GameObject.FindGameObjectsWithTag("Destructible").Length;
        Debug.Log( _totalTargets + " Buildings in total.");
        Debug.Log( CalculateScore());

        player.Energy = 0;
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        _audio = GetComponent<AudioSource>();
    }

    float CalculateScore()
    {
        float total = 0;
        MeshDestroy[] Targets = GameObject.FindObjectsByType<MeshDestroy>(FindObjectsSortMode.None);
        foreach (MeshDestroy target in Targets)
        {
            total += target.score;
        }
        return total;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchState();
        }
        
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.L))
        {
            Lose();
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
        {
            player.Energy += 1;
        }

        if(Input.GetMouseButtonDown(0) && State == Utilities.GameplayState.Play)
        {
            if (player.Energy > 0)
            {
                hasEnoughEnergy = true;
                StartCoroutine(LaserEyeTrigger());
                player.Energy -= 1;
                _audio.PlayOneShot(_laserSFX);
            }
            else
            {
                hasEnoughEnergy = false;
            }
            
        }
        
        if (GameObject.FindGameObjectsWithTag("Ghost").Length <= 10)
        {
            timer += Time.deltaTime;
            
            if (timer >= 10f)
            {
                Vector3 randomPosition = new Vector3(
                    Random.Range(1f, 238f),
                    0f,
                    Random.Range(-1f, 220f)
                );
                _ghost = Instantiate(
                    _ghostPrefab,                                    // Prefab
                    randomPosition,                           // Position
                    Quaternion.Euler(0, Random.Range(0, 360), 0)   // Rotation
                );
                timer = 0f;
            }
           
        }
        else
        {
            timer = 0;
        }
    }
    
    IEnumerator LaserEyeTrigger()
    {
        LaserEye = true;
        yield return new WaitForSeconds(0.8f);
        LaserEye = false;
    }

    public void Score(int score)
    {
        player.Score += score;
        _destroyedTargets += 1;
        
        _audio.PlayOneShot(_collisionSFX[Random.Range(0,_collisionSFX.Length)]);
        

        //check win?
        if (_destroyedTargets >= _totalTargets || player.Score >= MaxScore)
        {
            _stateMessage.text = "Congrats!\nYou've totally wiped out this city!";
            _stateMessage.fontSize = 48;
            _stateMessage.enabled = true;
            Debug.Log("[GameState Update] Win!");
            _audio.PlayOneShot(_winSFX);
            GameOver();
        }
    }
    
    public void Lose()
    {
        isAlive = false;
        _stateMessage.text = "You Die";
        _stateMessage.enabled = true;
        Debug.Log("[GameState Update] Lose..");
        _audio.PlayOneShot(_loseSFX);
        GameOver();
    }

    //Game State
    private void SwitchState()
    {
        if (State == Utilities.GameplayState.Play)
        {
            State = Utilities.GameplayState.Pause;
            _stateMessage.enabled = true;
            _restartButton.SetActive(true);
            Time.timeScale = 0; 
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            State = Utilities.GameplayState.Play;
            _stateMessage.enabled = false;
            _restartButton.SetActive(false);
            Time.timeScale = 1; 
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    public void GameOver()
    {
        StopAllCoroutines();
        State = Utilities.GameplayState.Gameover;
        _restartButton.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Restart()
    {
        Destroy(this);
        Destroy(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


}

