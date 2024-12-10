using UnityEngine;

public class DrinkBehavior : FoodBehavior
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    private float _bonusSpeed = 0.2f;
    private float _bonusJumpSpeed = 0.1f;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    public override float Speed()
    {
        return _speedIncrement + _bonusSpeed;
    }

    public override float jumpSpeed()
    {
        return _jumpSpeedIncrement + _bonusJumpSpeed;
    }
    
    public override AudioClip EatSound()
    {
        return eatSFX;
    }
}
