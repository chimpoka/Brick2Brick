using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMatrix
{
    List<List<int>> Matrix;
    int size;
}

public class Field : MonoBehaviour 
{
    static public int fieldSize;



    #region MonoBehaviour_Methods

    #endregion



    #region User_Methods

    public static Field Create(int size, int emptySquares)
	{
        fieldSize = size;
		Vector3 fielsPosition = Vector3.zero;

		if (size % 2 == 0) 
		{
			fielsPosition = new Vector3 (0.5f, 0.5f, 0.0f);
		}
		GameObject field = Instantiate (Resources.Load ("Prefabs/Field") as GameObject, fielsPosition, Quaternion.identity) as GameObject;
	
		Vector3 scale = Vector3.one * size;
		scale.z = 1;
		field.transform.localScale = scale * 1.04f;

		Vector3 cameraPosition = field.transform.position;
		cameraPosition.z = -10;
		Camera.main.transform.position = cameraPosition;

		Camera.main.orthographicSize = (float)size;

        //field.GetComponent<Renderer>().material.mainTextureScale = Vector2.one * size;
        field.gameObject.GetComponent<Field>().CreateTokens(size, emptySquares);

		return field.gameObject.GetComponent<Field> ();
	}

	private void CreateTokens(int size, int emptySquares)
	{
		//Debug.Log ("size: " + size + "   emptySquares: " + emptySquares);
		float offset = ((float)size - 1f) / 2f;
		Vector3 startPosition = new Vector3 (transform.position.x - offset, transform.position.y - offset, transform.position.z - 2);
	
		for (int i = 0; i < size; i++) 
		{
			for (int j = 0; j < size; j++) 
			{
				if ((i * size) + j >= (size * size) - emptySquares) 
				{
					emptySquares--;
				}
				else
				{
					if ((emptySquares == 0) || (Random.Range (0, size * size / emptySquares) > 0)) 
					{
						GameObject newTokenGameObject = Instantiate (Resources.Load ("Prefabs/Token2") as GameObject, 
							new Vector3 (startPosition.x + i, startPosition.y + j, startPosition.z), Quaternion.identity) as GameObject;
                        newTokenGameObject.transform.eulerAngles = new Vector3(0, 180, 0);
					}
					else
					{
						emptySquares--;
					}
				}
			}
		}
	}

   

    public static Field CreateNormalMode(int level, int difficulty)
    {
        List<List<int[,]>> MatrixesList = Controller.Instance.Matrixes;
        //List<List<LevelTurns>> LevelTurnsList = Controller.Instance.LevelTurnsList;
        List<int[,]> Matrixes = new List<int[,]>();

        Matrixes = MatrixesList[difficulty];
        

        level -= 1;
        float size = Mathf.Sqrt(Matrixes[level].Length);

        Controller.Instance.Level.FieldSize = (int)size;

        // Create Field
        Vector3 fieldPosition = Vector3.zero;

        if (size % 2 == 0)
        {
            fieldPosition = new Vector3(0.5f, 0.5f, 0.0f);
        }
        GameObject field = Instantiate(Resources.Load("Prefabs/Field") as GameObject, fieldPosition, Quaternion.identity) as GameObject;

        Vector3 scale = Vector3.one * size;
        scale.z = 1;
        field.transform.localScale = scale * 1.04f;

        // Create StarIndicator
        //field.gameObject.GetComponent<Field>().CreateStarIndicator((int)size, level, difficulty);

        // Camera settings
        Vector3 cameraPosition = new Vector3(field.transform.position.x, field.transform.position.y + 0.5f, -10f);
        //cameraPosition.z = -10;
        Camera.main.transform.position = cameraPosition;

        Camera.main.orthographicSize = (float)size;

        // Create tokens
        field.gameObject.GetComponent<Field>().CreateTokensNew(level, Matrixes);

        return field.gameObject.GetComponent<Field>();
    }

    private void CreateTokensNew(int level, List<int[,]> Matrixes)
    {
        float size = Mathf.Sqrt(Matrixes[level].Length);
       
        float offset = ((float)size - 1f) / 2f;
        Vector3 startPosition = new Vector3(transform.position.x - offset, transform.position.y + offset, transform.position.z - 2);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {          
                if (Matrixes[level][i, j] == 0)
                {

                }
                else
                {
                    GameObject newTokenGameObject = Instantiate(Resources.Load("Prefabs/Token2") as GameObject,
                        new Vector3(startPosition.x + j, startPosition.y - i, startPosition.z), Quaternion.identity) as GameObject;
                    newTokenGameObject.transform.eulerAngles = new Vector3(0, 180, 0);
                    newTokenGameObject.GetComponent<Renderer>().material = Controller.Instance.TokenMaterials[Matrixes[level][i, j] - 1];
                    Controller.Instance.TokensByTypes[Matrixes[level][i, j] - 1].Add(newTokenGameObject.GetComponent<Token>());
                }
            }
        }
    }

    //private void CreateStarIndicator(int size, int level, int difficulty)
    //{
    //    Vector3 starIndicatorPosition = Vector3.zero;
        
    //    if (size % 2 == 0)
    //    {
    //        starIndicatorPosition = new Vector3(0.5f, 0.5f, 0.0f);
    //    }
    //    starIndicatorPosition.y += size * 0.56f;

    //    GameObject starIndicator = Instantiate(Resources.Load("Prefabs/StarIndicator") as GameObject, starIndicatorPosition, Quaternion.identity) as GameObject;

    //    Vector3 scale = Vector3.one * size;
    //    scale.z = 1;
    //    scale.x = 0.24f;
    //    scale.y = 0.008f;
    //    starIndicator.transform.localScale = scale * size / 3;

    //    CreateStars(starIndicator, level, difficulty, size);
    //}

    //private void CreateStars(GameObject starIndicator, int level, int difficulty, int size)
    //{
        
    //    Transform[] points = starIndicator.GetComponentsInChildren<Transform>();

    //    float startPoint = points[1].position.x;
    //    float endPoint = points[2].position.x;
    //    float starIndicatorLength = Controller.Instance.LevelTurnsList[difficulty][level].OneStar;

    //    float[] starPositions = new float[3];
    //    starPositions[0] = Controller.Instance.LevelTurnsList[difficulty][level].OneStar;
    //    starPositions[1] = Controller.Instance.LevelTurnsList[difficulty][level].TwoStars;
    //    starPositions[2] = Controller.Instance.LevelTurnsList[difficulty][level].ThreeStars;

    //    for (int i = 0; i < 3; i++)
    //    {
    //        starPositions[i] = starIndicatorLength - starPositions[i];
    //        starPositions[i] = Mathf.InverseLerp(0, starIndicatorLength, starPositions[i]);
    //        starPositions[i] = Mathf.Lerp(startPoint, endPoint, starPositions[i]);

    //        Vector3 starPosition = starIndicator.transform.position;
    //        starPosition.x = starPositions[i];

    //        GameObject star = Instantiate(Resources.Load("Prefabs/Coin") as GameObject, starPosition, Quaternion.identity) as GameObject;

    //        Vector3 scale = star.transform.localScale;

    //        Debug.Log(star.transform.localScale);
    //        star.transform.localScale = scale * size / 3;
    //        Debug.Log(star.transform.localScale);
    //    }
    //}

    #endregion

}
