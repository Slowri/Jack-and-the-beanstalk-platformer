using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenGoose : MonoBehaviour
{
    public GameObject GM;

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        GM.GetComponent<GameManager>().NextLevel();
    }

}