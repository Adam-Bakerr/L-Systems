using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Turtle : MonoBehaviour
{
    Vector3 StartPos = Vector3.zero;
    Vector3 SavedPos;
    Quaternion SavedAngle;



    List<(Vector3, Quaternion)> SavedPositionData = new List<(Vector3, Quaternion)>();

    int itterationCount = 1;
    int evalCount = 0;


    List<GameObject> lines = new List<GameObject>();
    public Material defaultLineMaterial;

    private void Start()
    {
        StartPos = transform.position;
        LSYS.Eval += IncrementEvalCount;

        LSYS.Eval += removeLines;
    }

    private void removeLines()
    {
        for(int i = lines.Count -1; i >= 0; i--)
        {
            Destroy(lines[i]);
        }
    }


    public void SpawnObject(GameObject objectToSpawn)
    {
        transform.position += transform.forward;
        var tempObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity,transform);
        tempObject.SetActive(true);
        StartPos = transform.position;
    }

    private void newLine(Vector3 start, Vector3 finish)
    {
        GameObject line = new GameObject();
        lines.Add(line);
        line.transform.parent = this.transform;
        LineRenderer render = line.AddComponent<LineRenderer>();

        //Set up the line renderer
        render.startColor = Color.white;
        render.endColor = Color.white;
        render.startWidth = 0.1f;
        render.endWidth = 0.1f;
        render.positionCount = 2;
        render.useWorldSpace = true;
        render.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        render.receiveShadows = false;
        render.material = defaultLineMaterial;


        render.SetPosition(0, start);
        render.SetPosition(1, finish);


    }

    private void IncrementEvalCount()
    {
        evalCount++;
    }

    public void loadSaved()
    {
        transform.position = SavedPositionData[SavedPositionData.Count-1].Item1;
        transform.rotation = SavedPositionData[SavedPositionData.Count - 1].Item2;
        StartPos = transform.position;
        SavedPositionData.RemoveAt(SavedPositionData.Count - 1);
    }

    public void Save()
    {
        SavedPositionData.Add((transform.position, transform.rotation));
    }

    public void Draw(float distance)
    {

        transform.position += distance * transform.forward;
        newLine(StartPos, transform.position);
        StartPos = transform.position;
    }

    public void EvalDraw(float distance)
    {

        transform.position += evalCount * distance * transform.forward;
        newLine(StartPos, transform.position);
        StartPos = transform.position;
    }

    public void MoveByItterationCount()
    {
        Move(itterationCount);
    }

    public void Move(float distance)
    {

        transform.position += distance * transform.forward;
        StartPos = transform.position;
    }

    public void RotateX(float x)
    {
        transform.Rotate(new Vector3(x, 0, 0)); 
    }
    public void RotateY(float y)
    {
        transform.Rotate(new Vector3(0,y,0));
    }
    public void RotateZ(float z)
    {
        transform.Rotate(new Vector3(0, 0, z));
    }



    public void Update()
    {
        
    }
}
