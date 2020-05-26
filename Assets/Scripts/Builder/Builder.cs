using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VentureVeilStructures;
using Random = UnityEngine.Random;

public class Builder : MonoBehaviour
{
    //WorldMap 
    private GameObject worldMap;
    private Mesh worldMapMesh;

    //Route
    private List<Tuple<Vector3, float>> route; //Speed, time
    private Vector3 speed;
    private float timeLeft;
    private int currentStop;

    //API
    private GetInstance getInstance;

    //Pause 
    private bool isPaused, isBuilding;

    // Start is called before the first frame update
    void Start()
    {
        //get components
        getInstance = GameObject.Find("GameManager").GetComponent<GetInstance>();
        worldMap = getInstance.WorldMap;
        worldMapMesh = worldMap.GetComponent<MeshFilter>().mesh;

        //configure and create route
        route = new List<Tuple<Vector3, float>>();
        route.Add(new Tuple<Vector3, float>(transform.position, 0));
        CreateRoute();

        isPaused = false;
        isBuilding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && !isBuilding)
        {
            if (timeLeft > 0)
            {
                transform.position = transform.position + speed * Time.deltaTime;
                timeLeft -= Time.deltaTime;
            }
            else
            {
                if (currentStop == (route.Count - 1))
                    Destroy(gameObject);
                else
                {
                    currentStop++;
                    speed = route[currentStop].Item1;
                    timeLeft = route[currentStop].Item2;
                }

            }
        }
    }


    private void CreateRoute()
    {
        float x, z, statsSpeedFactor, time;
        int i;
        Vector3 speed, distance;

        Vector3 lastPoz = transform.position;
        int stops = Random.Range(5, 10);
        for (i = 0; i < stops; i++)
        {
            //Get a random position on the world map
            x = Random.Range(worldMap.transform.position.x - worldMap.transform.localScale.x * worldMapMesh.bounds.size.x * 0.5f,
                                  worldMap.transform.position.x + worldMap.transform.localScale.x * worldMapMesh.bounds.size.x * 0.5f);
            z = Random.Range(worldMap.transform.position.z - worldMap.transform.localScale.z * worldMapMesh.bounds.size.z * 0.5f,
                                  worldMap.transform.position.z + worldMap.transform.localScale.z * worldMapMesh.bounds.size.z * 0.5f);

            Ray ray = new Ray(new Vector3(x, 10, z), Vector3.down);
            RaycastHit hit = new RaycastHit();
            Physics.Raycast(ray, out hit);

            //Debug.DrawLine(lastPoz , hit.point, Color.red, 100); // See the route
            statsSpeedFactor = 1; //default speed
            distance = hit.point - lastPoz; // gets the distance 
            speed = statsSpeedFactor * distance.normalized; //calculate the speed based on statsSpeedFactor
            time = distance.magnitude / speed.magnitude; // calculate time
            route.Add(new Tuple<Vector3, float>(speed, time));

            lastPoz = hit.point; // updates last known poz
        }

        //first time preparation
        currentStop = 0;
        speed = route[currentStop].Item1;
        timeLeft = route[currentStop].Item2;
    }
    public void BuyBuilder()
    {
        BuildingPause();
        getInstance.GameManager.ChangeMouseMode(MouseModes.UI);

        getInstance.GameManager.BuyBuilder(gameObject); 
    }

    public void BuildingPause()
    {
        isBuilding = true;
    }

    public void BuildingUnpause()
    {
        isBuilding = false;
    }
    public void Pause()
    {
        isPaused = true;
    }
    public void Unpause()
    {
        isPaused = false;
    }
}
