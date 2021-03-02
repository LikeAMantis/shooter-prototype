using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity
{
    public float gravityScale;
    const float GravityConstant = 9.81f;
    readonly Rigidbody rb;

    public Gravity(Rigidbody rb, float gravityScale)
    {
        this.rb = rb;
        this.gravityScale = gravityScale;
        
        rb.useGravity = false;
    }

    public static void UseGravity(Rigidbody rb, float gravityScale)
    {
        rb.AddForce(Vector3.down * (GravityConstant * gravityScale), ForceMode.Acceleration);
    }
    
    public void UseGravity()
    {
        rb.AddForce(Vector3.down * (GravityConstant * gravityScale), ForceMode.Acceleration);
        // rb.velocity += Vector3.down * (GravityConstant * gravityScale);
    }
}
