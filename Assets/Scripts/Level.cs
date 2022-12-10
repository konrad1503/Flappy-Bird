using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	private const float CAMERA_OTROGRAPHIC_SIZE = 50f;
	private const float PIPE_WIDTH = 7.8f;
	private const float PIPE_HEAD_HEIGHT = 3.8f;
	private List<Transform> pipeList;
/*
	private void Awake() 
	{
		pipeList = new List<Transform>();
	}
*/
	private void Start() {
		CreatePipe(40f, 20f, true);
		CreatePipe(40f, 20f, false);
		CreateGapPipes(50f, 10f, 40f);
	}

	private void CreateGapPipes(float GapY, float gapSize, float xPosition)
	{
		CreatePipe(GapY - gapSize * .5f, xPosition, true);
		CreatePipe(CAMERA_OTROGRAPHIC_SIZE * 2f - GapY - gapSize * .5f, xPosition, false);

	}



	private void CreatePipe(float height, float xPosition, bool createBottom)
	{
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
		pipeBodyBoxCollider.offset = new Vector2(0f, height * .5f);
	}
}
