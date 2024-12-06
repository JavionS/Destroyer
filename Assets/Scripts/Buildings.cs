using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class Buildings : MonoBehaviour
{
    public float _buildingHeight;
    public GameObject _destroyedVersion;
    public float score;
    
    private BoxCollider building;
    // [SerializeField] private Dictionary<Collider, float> _buildingHeights = new();
    void Start()
    {
        building = GetComponent<BoxCollider>();
        string objectName = gameObject.name;
        _buildingHeight = building.bounds.size.y;
        //Debug.Log("Total height of " + objectName + " is " + _buildingHeight);
        score = _buildingHeight * 10;
    }
    
    
    // private void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log(other.gameObject.name);
    //     if (other.CompareTag("Player"))
    //     {
    //         Player player = other.gameObject.GetComponent<Player>();
    //         if (player.Height > _buildingHeight)
    //         {
    //             Destroy(gameObject);
    //             GameObject instance = Instantiate(_destroyedVersion, transform.position, Quaternion.identity);
    //             GameBehavior.Instance.Score((int)_score);
    //         }
    //         else
    //         {
    //             
    //         }
    //     }
    //
    //     if (other.CompareTag("Projectile"))
    //     {
    //         Destroy(gameObject);
    //         GameObject instance = Instantiate(_destroyedVersion, transform.position * 1.2f, Quaternion.identity);
    //         GameBehavior.Instance.Score((int)_score);
    //     }
    // }
    //

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var building in GetComponentsInChildren<Collider>())
        {
            Gizmos.DrawCube(building.bounds.center, building.bounds.size);
        }
    }
    
}
