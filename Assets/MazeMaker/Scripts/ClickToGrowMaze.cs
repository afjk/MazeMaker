using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToGrowMaze : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GameObject obj = getClickMazeMakerGrowObject();
        if( obj)
        {
            Debug.Log( obj.name );

            MazeMakerGrowInObj mmgo = obj.GetComponent<MazeMakerGrowInObj>();
            if (mmgo)
            {
                Debug.Log("MazeMakerGrowInObj Grow!");
                mmgo.Grow();
            }
        }
    }

    public GameObject getClickMazeMakerGrowObject()
    {
        GameObject result = null;

        RaycastHit[] hits;
        List<GameObject> gameObjects = new List<GameObject>();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray, 100.0F);

            foreach (RaycastHit hit in hits)
            {
                MazeMakerGrowInObj mmgo = hit.collider.gameObject.GetComponent<MazeMakerGrowInObj>();
                if (mmgo)
                {
                    gameObjects.Add(hit.collider.gameObject);
                }
            }

            if (gameObjects.Count > 0)
            {
                result = gameObjects[0];
                float minDistance = Vector3.Distance(Camera.main.transform.position, result.transform.position);

                for (int i = 1; i < gameObjects.Count; i++)
                {
                    float distance = Vector3.Distance(Camera.main.transform.position, gameObjects[i].transform.position);
                    if (minDistance > distance)
                    {
                        minDistance = distance;
                        result = gameObjects[i];
                    }
                }
            }
        }
        return result;
    }
}
