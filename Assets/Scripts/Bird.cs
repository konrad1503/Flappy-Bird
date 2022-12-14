using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

    const float JUMP_AMOUNT = 100f;
    const float SKY_LIMIT = 50f;
    
    public static Bird instance;

    public static Bird GetInstance() {
        return instance;
    }

    public event EventHandler OnDied;
    public event EventHandler OnStartedPlaying;
    
    private Rigidbody2D birdRigidbody2D;
    private State state;

    private enum State {
		WaitingToStart,
        Playing,
		BirdDead,
	}
    
    private void Awake() {
        instance = this;
        birdRigidbody2D = GetComponent < Rigidbody2D >();
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        state = State.WaitingToStart;
    }
    
    private void Update() { 
        OutOfMap();
        switch (state) {
        default:
        case State.WaitingToStart:
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                state = State.Playing;
                birdRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                Jump();
                if (OnStartedPlaying != null) OnStartedPlaying(this, EventArgs.Empty);
            }   
            break;
        case State.Playing:
            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) {
                Jump();
            }   
            break;
        case State.BirdDead:
            break; 
        }                   
    }
    
    private void Jump() {
        birdRigidbody2D.velocity = Vector2.up * JUMP_AMOUNT;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        birdRigidbody2D.bodyType = RigidbodyType2D.Static;
        if (OnDied != null) OnDied(this, EventArgs.Empty);
    }

    private void OutOfMap() {
        if (gameObject.transform.position.y >= SKY_LIMIT) {
            birdRigidbody2D.bodyType = RigidbodyType2D.Static;
            if (OnDied != null) OnDied(this, EventArgs.Empty);       
        }
    }

}
