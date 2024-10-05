using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resistor : MonoBehaviour
{
    // Node Info
    public GameObject self;
    [SerializeField] GameObject text = null;
    public string type = "Resistor";
    public int grade;

    //Incoming Stats
    public double incomingResistance = 0;

    //Stats
    public double resistance;

    //Outgoing Stats
    public double outgoingResistance;

    //Connections
    public List<GameObject> rightObjects;
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
        //If multiple right connections grade increases
        if (rightObjects.Count > 1)
        {
            string tag;
            foreach (var obj in rightObjects)
            {
                tag = obj.tag;

                //Replace with more elegant version
                if (tag == "Resistor")
                {
                    obj.GetComponent<Resistor>().grade = grade + 1;

                    obj.GetComponent<Resistor>().setGrades();
                }
                else if (tag == "End")
                {
                    obj.GetComponent<EndNode>().grade = grade + 1;

                    obj.GetComponent<EndNode>().setGrades();
                }
                else Debug.Log("Something is Wrong");
            }
        }
        else
        {
            string tag;
            var obj = rightObjects[0];
            tag = obj.tag;
            //Replace with more elegant version
            if (tag == "Resistor")
            {
                obj.GetComponent<Resistor>().grade = grade;
                obj.GetComponent<Resistor>().setGrades();
            }
            else if (tag == "End")
            {
                obj.GetComponent<EndNode>().grade = grade;
                obj.GetComponent<EndNode>().setGrades();
            }
            else Debug.Log("Something is Wrong");
        }
    }


    public void calculateResistance()
    {
        outgoingResistance = resistance + incomingResistance;

        if (rightObjects[0].tag == "End")
        {
            rightObjects[0].GetComponent<EndNode>().incomingResistance += outgoingResistance;
        }

        //If series
        else if (rightObjects.Count == 1 && grade == rightObjects[0].GetComponent<Resistor>().grade)
        {
            rightObjects[0].GetComponent<Resistor>().incomingResistance += outgoingResistance;
            rightObjects[0].GetComponent<Resistor>().calculateResistance();
        }
        else if (grade == rightObjects[0].GetComponent<Resistor>().grade + 1)
        {
            var obj = rightObjects[0];
            if (self == obj.GetComponent<Resistor>().leftObjects[obj.GetComponent<Resistor>().leftObjects.Count - 1])
            {
                double calculate;
                double sum = 0;
                foreach (var value in obj.GetComponent<Resistor>().leftObjects)
                {
                    sum += 1f / value.GetComponent<Resistor>().outgoingResistance;
                }
                calculate = 1f / sum;
                obj.GetComponent<Resistor>().incomingResistance += calculate;
                obj.GetComponent<Resistor>().calculateResistance();
            }
            else return;
        }
        else
        {
            var obj = rightObjects[0];
            while (grade != obj.GetComponent<Resistor>().grade)
            {
                obj = obj.GetComponent<Resistor>().rightObjects[0];
            }
            obj.GetComponent<Resistor>().incomingResistance += outgoingResistance;

            foreach (var resistor in self.GetComponent<Resistor>().rightObjects)
            {
                resistor.GetComponent<Resistor>().calculateResistance();
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(text !=null)
        {
            text.GetComponent<Text>().text = "R = " + string.Format("{0:0.00}", resistance);
        }
    }
}
