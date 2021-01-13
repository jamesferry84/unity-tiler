using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    [SerializeField] float verticalVelocity = 1f;
    Rigidbody2D myRigidBody;
    
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        //myRigidBody.velocity = new Vector2(0,verticalVelocity);
        transform.Translate(new Vector2(0,verticalVelocity * Time.deltaTime));
    }
}
