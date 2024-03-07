using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateObstacle : MonoBehaviour
{
    //instance variables
    public GameObject[] availableObjects;
    public List<GameObject> objects; //will store created coins, so that you can check if you need to add more ahead of the player or remove them when they have left the screen.

    public float objectsMinDistance = 0.0f;
    public float objectsMaxDistance = 3.0f;

    public float objectsMinY = 5.0f;
    public float objectsMaxY = -5.0f;

    private float screenWidthInPoints;


    void AddObject(float lastObjectX)
    {
        //1
        int randomIndex = Random.Range(0, availableObjects.Length);
        //2
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);
        //3
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);
        objects.Add(obj);
    }

    void GenerateObjectsIfRequired()
    {
        //1
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;
        //2
        List<GameObject> objectsToRemove = new List<GameObject>();
        foreach (var obj in objects)
        {
            //3
            float objX = obj.transform.position.x;
            //4
            farthestObjectX = Mathf.Max(farthestObjectX, objX);
            //5
            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }
        //6
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }
        //7
        if (farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
    }


    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            //GenerateRoomIfRequired();
            GenerateObjectsIfRequired();
            yield return new WaitForSeconds(0.0f);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update()
    {

    }

}
