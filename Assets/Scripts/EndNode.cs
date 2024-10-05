using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndNode : MonoBehaviour
{
    [SerializeField] GameObject I;
    [SerializeField] GameObject U;
    [SerializeField] GameObject R;

    //Node Info
    [SerializeField] GameObject self;
    public string type = "End";
    public int grade = 0;

    //Incoming Stats
    public double incomingResistance = 0;

    //Circuit Values
    public double Intensity;
    public double Tension;

    //Right Connections
    public List<GameObject> leftObjects;

    public void setGrades()
    {
        //If multiple left connections [this] grade decreases
        if (leftObjects.Count > 1)
        {
            string tag;
            var obj = leftObjects[0];
            tag = obj.tag;

            //Replace with more elegant version
            if (tag == "Resistor")
            {
                grade = obj.GetComponent<Resistor>().grade - 1;
            }
            else if (tag == "Start")
            {
                grade = obj.GetComponent<StartNode>().grade - 1;
            }
            else Debug.Log("Something is Wrong");
        }
    }

    public void updateIntensity()
    {
        if (incomingResistance > 0)
        {
            Intensity = Tension / incomingResistance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateIntensity();
        I.GetComponent<Text>().text = "I = " + string.Format("{0:0.00}", Intensity);
        R.GetComponent<Text>().text = "R = " + string.Format("{0:0.00}", incomingResistance);
        U.GetComponent<Text>().text = "U = " + string.Format("{0:0.00}", Tension);
    }
}
