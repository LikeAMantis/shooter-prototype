using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity
{
    static readonly Vector3 GravityVector = new Vector3(0, -9.83f, 0);
    readonly Rigidbody rb;

    public Gravity(Rigidbody rb)
    {
        this.rb = rb;
        rb.useGravity = false;
    }

    public static void UseGravity(Rigidbody rb, float gravityScale)
    {
        rb.AddForce(GravityVector * gravityScale, ForceMode.Acceleration);
    }
    
    public void UseGravity(float gravityScale)
    {
        // rb.AddForce(GravityVector * gravityScale, ForceMode.Acceleration);
        rb.velocity += GravityVector * (gravityScale * Time.deltaTime);
    }
}
