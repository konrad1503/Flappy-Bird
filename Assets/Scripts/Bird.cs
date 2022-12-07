using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    const float JUMP_AMOUNT = 100f;
    
    Rigidbody2D birdRigidbody2D;
    
    void Awake() {
        birdRigidbody2D = GetComponent < Rigidbody2D >();
    }
    
    void Update() { 
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
            Jump();
        }        
    }
    void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }


}
