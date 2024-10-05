using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartNode : MonoBehaviour
{
    // Start is called before the first frame update

    // Node Info
    public GameObject self;
    public string type = "Start";
    public int grade = 0;


    ///Temp Placeholder
    public double resistance = 0;

    //Outgoing Stats
    public double outgoingResistance = 0;

    //Right Connections
    public List<GameObject> rightObjects;

    public void setGrades()
    {
        //If multiple right connections grade increases
        if(rightObjects.Count > 1)
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
                else if (tag == "EndNode")
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
                obj.GetComponent <Resistor>().setGrades();
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
        outgoingResistance = resistance;

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
                Debug.Log("Enter2");
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
                //Debug.Log(resistor);
                resistor.GetComponent<Resistor>().calculateResistance();
            }
        }
    }


    void Start()
    {
        setGrades();
        calculateResistance();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
