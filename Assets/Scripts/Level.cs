using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	private const float CAMERA_OTROGRAPHIC_SIZE = 50f;
	private const float PIPE_WIDTH = 7.8f;
	private const float PIPE_HEAD_HEIGHT = 3.8f;
	private const float PIPE_MOVE_SPEED = 30f;
	private const float PIPE_DESTROY_X_POSITION = -100f;
	private const float PIPE_SPAWN_X_POSITION = +100f;

	private List<Pipe> pipeList;

	private float pipeSpawnTimer;
	private float pipeSpawnTimermax;
	private float gapSize;


	private void Awake()  {
		pipeList = new List<Pipe>();
		pipeSpawnTimermax = 1.5f;
		gapSize = 50f;
	}

	private void Start() {
		/*
		CreatePipe(40f, 20f, true);
		CreatePipe(40f, 20f, false);
		CreateGapPipes(50f, 10f, 40f);
		*/
	}

	private void Update() {
		HandlePipeMovement();
		HandlePipeSpawning();
	}

	private void HandlePipeSpawning() {
		pipeSpawnTimer -= Time.deltaTime;
		if (pipeSpawnTimer < 0 ) {
			pipeSpawnTimer += pipeSpawnTimermax;

            float heightEdgeLimit = 10f;
			float totalHeight = CAMERA_OTROGRAPHIC_SIZE * 2f;
			float minHeight = gapSize * .5f + heightEdgeLimit;
			float maxHeight = totalHeight - gapSize * .5f - heightEdgeLimit;

			float height = UnityEngine.Random.Range(minHeight, maxHeight);
			CreateGapPipes(50f ,gapSize ,PIPE_SPAWN_X_POSITION);
		}
	}

	private void HandlePipeMovement() {
		for(int i = 0; i < pipeList.Count; i++) {
			Pipe pipe = pipeList[i];
			pipe.Move();
			if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION) {
				pipe.DestroySelf();
				pipeList.Remove(pipe);
				i--;
			}
		}

	}

	private void CreateGapPipes(float GapY, float gapSize, float xPosition) {
		CreatePipe(GapY - gapSize * .5f, xPosition, true);
		CreatePipe(CAMERA_OTROGRAPHIC_SIZE * 2f - GapY - gapSize * .5f, xPosition, false);

	}



	private void CreatePipe(float height, float xPosition, bool createBottom) {
		// Pipe Head Set up
		Transform pipeHead = Instantiate(GameAssets.GetInstance().pfPipeHead);
		float pipeHeadYPosition;
		if (createBottom) {
			pipeHeadYPosition = -CAMERA_OTROGRAPHIC_SIZE + height  - PIPE_HEAD_HEIGHT * .5f;
		} else {
			pipeHeadYPosition = +CAMERA_OTROGRAPHIC_SIZE - height  + PIPE_HEAD_HEIGHT * .5f;
		}
		pipeHead.position = new Vector2(xPosition, pipeHeadYPosition);
		

        // Pipe Body Set up
        Transform pipeBody = Instantiate(GameAssets.GetInstance().pfPipeBody);
		float pipeBodyYPosition;
		if (createBottom) {
			pipeBodyYPosition = -CAMERA_OTROGRAPHIC_SIZE;
		} else {
			pipeBodyYPosition = +CAMERA_OTROGRAPHIC_SIZE;
			pipeBody.localScale = new Vector3(1, -1, 1);
		}
		pipeBody.position = new Vector2(xPosition, pipeBodyYPosition);
		
		
		SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
		pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height );

		BoxCollider2D pipeBodyBoxCollider = pipeBody.GetComponent<BoxCollider2D>();
		pipeBodyBoxCollider.size = new Vector2(PIPE_WIDTH, height);
		pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f); //zaczepienie punktu odniesienia do spodu pipe

		Pipe pipe = new Pipe (pipeBody, pipeHead);
		pipeList.Add(pipe);
	}


	//Handle signle pipe
	private class Pipe {
		private Transform pipeBodyTransform;
		private Transform pipeHeadTransform;

		public Pipe (Transform pipeBodyTransform, Transform pipeHeadTransform) {
			this.pipeBodyTransform = pipeBodyTransform;
			this.pipeHeadTransform = pipeHeadTransform;
		}

		public void Move() {
		pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
		pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
		}

		public float GetXPosition() {
			return pipeBodyTransform.position.x;
		}

		public void DestroySelf() {
			Destroy(pipeBodyTransform.gameObject);
			Destroy(pipeHeadTransform.gameObject);

		}


	}
}


