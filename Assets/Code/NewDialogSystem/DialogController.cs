using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cherrydev;

public class DialogController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] DialogBehaviour dialogBehaviour;
    [SerializeField] DialogNodeGraph graph;
    void Start()
    {
        dialogBehaviour.StartDialog(graph);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
