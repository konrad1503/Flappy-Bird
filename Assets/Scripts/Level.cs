using System.Runtime.CompilerServices;
using System.Threading;
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
	private const float BIRD_X_POSITION = 0f;

	private static Level instance;

	public static Level GetInstance() {
		return instance;
	}


	private List<Pipe> pipeList;
	private int pipesPassedCount;
    private int pipesSpawned;
	private float pipeSpawnTimer;
	private float pipeSpawnTimermax;
	private float gapSize;
	private State state;

	public enum Difficulty {
		Easy,
		Medium,
		Hard,
		Impossible
	}

	private enum State {
		WaitingToStart,
		Playing,
		BirdDead,
	}

	private void Awake()  {
		instance = this;
		pipeList = new List<Pipe>();
		pipeSpawnTimermax = 1.5f;
		gapSize = 50f;
		state = State.WaitingToStart;
	}

	private void Start() {
		Bird.GetInstance().OnDied += Bird_OnDied;
		Bird.GetInstance().OnStartedPlaying += Bird_OnStartedPlaying;
	}

	private void Bird_OnStartedPlaying(object sender, System.EventArgs e) {
        state = State.Playing;
	}
	
	private void Bird_OnDied(object sender, System.EventArgs e) {
        state = State.BirdDead;
	}

	private void Update() {
		if ( state == State.Playing) {
		HandlePipeMovement();
		HandlePipeSpawning();
		}
	}

	private void HandlePipeSpawning() {
		pipeSpawnTimer -= Time.deltaTime;
		if (pipeSpawnTimer < 0 ) {
			pipeSpawnTimer += pipeSpawnTimermax;

            float heightLLimit = 15f;
			float heightULimit = 10f;
			float totalHeight = CAMERA_OTROGRAPHIC_SIZE * 2f;
			float minHeight = gapSize * .5f + heightLLimit;
			float maxHeight = totalHeight - gapSize * .5f - heightULimit;

			float height = UnityEngine.Random.Range(minHeight, maxHeight);
			CreateGapPipes(height ,gapSize ,PIPE_SPAWN_X_POSITION);
		}
	}

	private void HandlePipeMovement() {
		for(int i = 0; i < pipeList.Count; i++) {
			Pipe pipe = pipeList[i];
			bool isToTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
			pipe.Move();
			if (isToTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION && pipe.IsBottom()) {
				pipesPassedCount++;
			}
			if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION) {
				pipe.DestroySelf();
				pipeList.Remove(pipe);
				i--;
			}
		}
	}

    private void SetDifficulty(Difficulty difficulty) {
		switch (difficulty) {
			case Difficulty.Easy:
				gapSize = 50f;
				break;
			case Difficulty.Medium:
				gapSize = 40f;
				pipeSpawnTimermax = 1.3f;
				break;
			case Difficulty.Hard:
				gapSize =30f;
				pipeSpawnTimermax = 1.1f;
				break;
			case Difficulty.Impossible:
				gapSize = 20f;
				pipeSpawnTimermax = 0.9f;
				break;		

		}
	}

    private Difficulty GetDifficulty() {
		if (pipesSpawned >= 60) return Difficulty.Impossible;
		if (pipesSpawned >= 40)	return Difficulty.Hard;
		if (pipesSpawned >= 20) return Difficulty.Medium;
		return Difficulty.Easy;
	}


	private void CreateGapPipes(float GapY, float gapSize, float xPosition) {
		CreatePipe(GapY - gapSize * .5f, xPosition, true);
		CreatePipe(CAMERA_OTROGRAPHIC_SIZE * 2f - GapY - gapSize * .5f, xPosition, false);
        pipesSpawned++;
		SetDifficulty(GetDifficulty());
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

		Pipe pipe = new Pipe (pipeBody, pipeHead, createBottom);
		pipeList.Add(pipe);
	}

    public int GetPipesSpawned() {
		return pipesSpawned;

	}

	public int GetPipesPassedCount() {
		return pipesPassedCount;
	}

	//Handle signle pipe
	private class Pipe {
		private Transform pipeBodyTransform;
		private Transform pipeHeadTransform;
		private bool isBottom;

		public Pipe (Transform pipeBodyTransform, Transform pipeHeadTransform, bool isBottom) {
			this.pipeBodyTransform = pipeBodyTransform;
			this.pipeHeadTransform = pipeHeadTransform;
			this.isBottom = isBottom;
		}

		public void Move() {
		pipeBodyTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
		pipeHeadTransform.position += new Vector3(-1, 0, 0) * PIPE_MOVE_SPEED * Time.deltaTime;
		}

		public float GetXPosition() {
			return pipeBodyTransform.position.x;
		}

		public bool IsBottom() {
			return isBottom;
		}

		public void DestroySelf() {
			Destroy(pipeBodyTransform.gameObject);
			Destroy(pipeHeadTransform.gameObject);

		}


	}
}


