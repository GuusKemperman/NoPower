using UnityEngine;
using System.Collections;

using Unity.AI;
using Unity.AI.Navigation;

public class NavMeshUpdate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        StartCoroutine(UpdateNav());
    }

    IEnumerator UpdateNav()
    {
        GetComponent<NavMeshSurface>().BuildNavMesh();

        yield return new WaitForSeconds(2);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
