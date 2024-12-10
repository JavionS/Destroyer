using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    
    protected float _speedIncrement = 0.28f;
    protected float _jumpSpeedIncrement = 0.15f;
    
    [SerializeField]protected AudioClip eatSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    
    public virtual float Speed()
    {
        return _speedIncrement;
    }
    
    public virtual float jumpSpeed()
    {
        return _jumpSpeedIncrement;
    }
    
    public virtual AudioClip EatSound()
    {
        return eatSFX;
    }
}
