using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] Text speedometer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedometer.GetComponent<Text>().text = (Mathf.Round((Mathf.Ceil(player.GetComponent<Rigidbody>().velocity.sqrMagnitude))/5)*5).ToString() + " mph";
    }
}
