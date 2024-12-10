using System;
using Sample;
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
    
    
    [SerializeField] private TextMeshProUGUI _scoreGUI;
    [SerializeField] private TextMeshProUGUI _EnergyGUI;
    
    private CharacterController _charController;
    
    private void Start()
    {
        _charController = GetComponent<CharacterController>();
        
        _maxEnergy = GameBehavior.Instance.MaxEnergy;

        _height = GameBehavior.Instance.InitialHeight;
        
        _EnergyGUI.text = "Energy : " + _energy + " / " + _maxEnergy;
        
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
        if (other.CompareTag("Ghost"))
        {
            GhostScript ghost = other.GetComponent<GhostScript>();
            if (ghost._isAlive)
            {
                GameBehavior.Instance.Lose();
            }
            
        }
    }
    
    private void Update()
    {
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
                GameBehavior.Instance.Score(meshDestroy.score);
                
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
            _scoreGUI.text =  _score + " / " + GameBehavior.Instance.MaxScore;
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
