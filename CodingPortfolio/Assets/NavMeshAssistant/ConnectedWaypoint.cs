using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum PatrolType {PointToPoint, PatrolArea};

public class ConnectedWaypoint : MonoBehaviour
{

    [SerializeField]
    protected float debugDrawRadius = 1.0f;
    [SerializeField]
    protected float connectivityRadius = 50f;

    List<ConnectedWaypoint> connections;


    [MenuItem("NavMesh Manager/Create New Waypoint &w")]
    private static void CreateNavMeshManager()
    {
        if (GameObject.Find("WayPoint Manager") == null)
        {
            GameObject wayPointManager = new GameObject("WayPoint Manager");

            GameObject waypoint = new GameObject("Waypoint");
            waypoint.transform.SetParent(wayPointManager.transform);
            waypoint.AddComponent<ConnectedWaypoint>();
            waypoint.transform.tag = "Waypoint";
        }
        else
        {
            GameObject wayPointManager = GameObject.Find("WayPoint Manager");
            GameObject waypoint = new GameObject("Waypoint");
            waypoint.transform.SetParent(wayPointManager.transform);
            waypoint.AddComponent<ConnectedWaypoint>();
        }

        
    }

    public void Start()
    {

        GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag("Waypoint");

        connections = new List<ConnectedWaypoint>();

        for (int i = 0; i < allWaypoints.Length; i++)
        {
            ConnectedWaypoint nextWaypoint = allWaypoints[i].GetComponent<ConnectedWaypoint>();

            if (nextWaypoint != null)
            {
                if (Vector3.Distance(this.transform.position, nextWaypoint.transform.position) <= connectivityRadius && nextWaypoint != this)
                {
                    connections.Add(nextWaypoint);
                }
            }
        }
    }
    public void OnDrawGizmos()
    {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, debugDrawRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, connectivityRadius);
    }
    public ConnectedWaypoint nextWaypoint(ConnectedWaypoint previousWaypoint)
    {

        if (connections.Count == 0)
        {

            return null;

        }
        else if (connections.Count == 1 && connections.Contains(previousWaypoint))
        {

            return previousWaypoint;

        }
        else
        {

            ConnectedWaypoint nextWaypoint;
            int nextIndex = 0;

            do
            {
                nextIndex = UnityEngine.Random.Range(0, connections.Count);
                nextWaypoint = connections[nextIndex];
            } while (nextWaypoint == previousWaypoint);

            return nextWaypoint;
        }
    }
}