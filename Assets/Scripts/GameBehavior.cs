using UnityEngine;
using System.Collections;
using TMPro;
public class GameBehavior : MonoBehaviour
{
    public static GameBehavior Instance;
    public Utilities.GameplayState State = Utilities.GameplayState.Play;
    public Player player;
    
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
    private int _totalBuildings;
    [HideInInspector] public int _destroyedBuildings = 0;
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

        _totalBuildings = GameObject.FindGameObjectsWithTag("Buildings").Length;
        Debug.Log(_totalBuildings + " Buildings in total.");

        player.Energy = 0;
        
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SwitchState();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            player.Height += 10;
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            Lose();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            player.Energy += 1;
        }

        if(Input.GetMouseButtonDown(0) && State == Utilities.GameplayState.Play)
        {
            StartCoroutine(LaserEyeTrigger());
            if (player.Energy > 0)
            {
                player.Energy -= 1;
                hasEnoughEnergy = true;
            }
            else
            {
                hasEnoughEnergy = false;
            }
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
        _destroyedBuildings += 1;

        //check win?
        // if (_destroyedBuildings >= _totalBuildings)
        // {
        //     _stateMessage.text = "Congrats! You've totally wiped out this city!";
        //     _stateMessage.fontSize = 50;
        //     _stateMessage.enabled = true;
        //     Debug.Log("[GameState Update] Win!");
        //     GameOver();
        // }
    }
    
    public void Lose()
    {
        isAlive = false;
        _stateMessage.text = "Oh no!!";
        _stateMessage.enabled = true;
        Debug.Log("[GameState Update] Lose..");
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
        }
        else
        {
            State = Utilities.GameplayState.Play;
            _stateMessage.enabled = false;
            _restartButton.SetActive(false);
            Time.timeScale = 1; 
        }
    }
    
    public void GameOver()
    {
        StopAllCoroutines();
        State = Utilities.GameplayState.Gameover;
        _restartButton.SetActive(true);
    }
    
    public void Restart()
    {
        Destroy(this);
        Destroy(gameObject);
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


}

