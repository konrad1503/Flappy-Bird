using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    const float JUMP_AMOUNT = 100f;
    
    public static Bird instance;

    public static Bird GetInstance() {
        return instance;
    }

    public event EventHandler OnDied;
    
    Rigidbody2D birdRigidbody2D;
    
    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent < Rigidbody2D >();
    }
    
    private void Update() { 
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Jump();
        }        
    }
    
    private void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }

}
