/**************************************************************************
* Copyright (C) echoAR, Inc. 2018-2020.                                   *
* echoAR, Inc. proprietary and confidential.                              *
*                                                                         *
* Use subject to the terms of the Terms of Service available at           *
* https://www.echoar.xyz/terms, or another agreement                      *
* between echoAR, Inc. and you, your company or other organization.       *
***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.Networking;

public class CustomBehaviour : MonoBehaviour
{
    [HideInInspector]
    public Entry entry;

    /// <summary>
    /// EXAMPLE BEHAVIOUR
    /// Queries the database and names the object based on the result.
    /// </summary>
    private string[] allLines = System.IO.File.ReadAllLines(@"D:\@font-face\a\stateOfVariables.txt");

    public string APIKey = "noisy-salad-1375";


    private Dictionary<string, Data[]> dataMap = new Dictionary<string, Data[]>();
    private int scale = 2;

        public class Data
        {
            public string value;
            public GameObject cube;

            public Data(GameObject cube, string value)
            {
                this.cube = cube;
                this.value = value;
            }
        }

    // Use this for initialization
    void Start()
    {
        // Add RemoteTransformations script to object and set its entry
        this.gameObject.AddComponent<RemoteTransformations>().entry = entry;

        // Qurey additional data to get the name
        string value = "";
        if (entry.getAdditionalData() != null && entry.getAdditionalData().TryGetValue("name", out value))
        {
            // Set name
            this.gameObject.name = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (entry.getAdditionalData() != null)
        {
            string scaleString = "";

            if (entry.getAdditionalData().TryGetValue("scale", out scaleString))
            {
                scale = int.Parse(scaleString);
            }
            int x = -9;
            float xx= -9.25f;
            float y = 0.25f;
            float yy = 0.5f;
            foreach(string continent in allLines)
            {             
                //Thread.Sleep(3000);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.AddComponent<MeshCollider>();
                cube.transform.position = new Vector3(x, y, 8.5f);

                //printing the digits
                GameObject text = new GameObject();
                TextMesh t = text.AddComponent<TextMesh>();
                t.text = continent;
                t.fontSize = 100;
                text.name = "Text " + continent;
                text.transform.localScale = 0.1f * Vector3.one;
                text.GetComponent<Renderer>().material.color = new Color(1,0,0,0.1f);
                text.transform.position = new Vector3(xx, yy, 8.5f);
                //x += 2;
                y+=1.0f;
                yy+=1.25f;
            }
        }



            // ---------------------------
            // --------------------------
            //          NEW CODE for api calls
        string[] allnewLines = System.IO.File.ReadAllLines(@"D:\@font-face\a\stateOfVariables.txt");
        // string temp2 = "";
        // // if (File.Exists(textFile)) 
        // // {  
        //     // Read file using StreamReader. Reads file line by line  
        //     using(StreamReader file = new StreamReader(textFile)) {   
        //     string ln;  
        
        //     while ((ln = file.ReadLine()) != null) {  
        //     temp2+= ln +";"; 
        //     }  
        //     file.Close();  
            
        //     }  
        // // } 
        // string[] allnewLines = temp2.Split(";") ;



        foreach(string line in allnewLines)
        {
            string[] subs = line.Split('=');
            string dataType = subs[0];
            string rhs = subs[1];
            Data[] oldDataAll;

            if(dataMap.TryGetValue(line, out oldDataAll))
            {
                //1) if ", " doesn't exist
                if(!rhs.Contains(", "))
                {
                    if(oldDataAll[0].value!=rhs)
                    {
                        // send for updation
                        updateEntryData(entry.getId(), dataType, rhs);
                    }
                }

                //2) if ", " exists, split it
                if(rhs.Contains(", "))
                {
                    string[] values = rhs.Split(',');
                    // now iterate over each value and corresponding oldData
                    for(int i=0; i<oldDataAll.Length; i=i+1)
                    {
                        // if any one value is different, send whole rhs for updation
                        if(oldDataAll[i].value!=values[i])
                        {
                            updateEntryData(entry.getId(), dataType, rhs);
                            break;
                        }
                    }

                }

            }


        }
        // iteration over each line ends





    }  
    /// update method ends

    public void updateEntryData(string entryID, string item, string value)
    {
                // Create form
        WWWForm form = new WWWForm();
        // Set form data
        form.AddField("key", APIKey);        // API Key
        form.AddField("entry", entryID);     // Entry ID
        form.AddField("data", item);      // Key
        form.AddField("value", value);   // Value

        UnityWebRequest www = UnityWebRequest.Post("https://console.echoar.xyz/post", form);
        // Send request
        www.SendWebRequest();
        Update();
        // Check for errors
        // if (www.isNetworkError || www.Result == UnityWebRequest.Result.ProtocolError)
        // {
        //     Debug.Log(www.error);
        // }
        // else
        // {
        //     Debug.Log("Data update complete!");
        // }

    }



}