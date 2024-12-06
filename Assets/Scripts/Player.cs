using System;
using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private int _score = 0;
    private int _energy = 0;
    private int _maxEnergy;
    private float _height;

    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private Transform _eyePositionL;
    [SerializeField] private Transform _eyePositionR;
    [SerializeField] private GameObject[] Lazers = new GameObject[2];
    
    [SerializeField] private TextMeshProUGUI _scoreGUI;
    [SerializeField] private TextMeshProUGUI _EnergyGUI;
    
    private CharacterController _charController;
    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        
        _maxEnergy = GameBehavior.Instance.MaxEnergy;

        _height = GameBehavior.Instance.InitialHeight;
        
        Lazers[0] = Instantiate(_laserPrefab, _eyePositionL) as GameObject;
        Lazers[1] = Instantiate(_laserPrefab, _eyePositionR) as GameObject;
        foreach (var lazer in Lazers)
        {
            lazer.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnergyDrink"))
        {
            if (_energy < _maxEnergy)
            {
                Energy += 1;
            }
        }
    }
    
    private void Update()
    {
        if (GameBehavior.Instance.isAlive && GameBehavior.Instance.LaserEye)
        {
            if (GameBehavior.Instance.hasEnoughEnergy)
            {
                foreach (var lazer in Lazers)
                {
                    lazer.SetActive(true);
                }
            }
        }
        else
        {
            foreach (var lazer in Lazers)
            {
                lazer.SetActive(false);
            }
        }

        _height = _charController.bounds.size.y;
    }

    private void OnControllerColliderHit (ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Destructible"))
        {
            MeshDestroy meshDestroy = hit.gameObject.GetComponent<MeshDestroy>();
            if (Height > meshDestroy._buildingHeight)
            {
                meshDestroy.DestroyMesh();
                
                //My original Building Destruction Code:
                //Destroy(hit.gameObject);
                //GameObject instance = Instantiate(building._destroyedVersion, hit.transform.position, Quaternion.identity);
                //GameBehavior.Instance.Score((int)building.score);
            }
        }
    }

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            _scoreGUI.text =  _score.ToString();
        }
    }
    
    public int Energy
    {
        get => _energy;
        set
        {
            _energy = value;
            _EnergyGUI.text = "Energy : " + _energy + " / " + _maxEnergy;
        }
    }
    
    public float Height
    {
        get => _height;
        set
        {
            _height = value;
        }
    }
}
