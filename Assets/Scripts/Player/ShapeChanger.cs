using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshCollider))]
public class ShapeChanger : MonoBehaviour
{

    // rigidbody to change the mass
    public Rigidbody rigidbodyToAffect;
    [Header("Shapes")]
    public List<Shape> shape;
    private MeshFilter objectMesh;
    private MeshCollider meshCollider;

    private void Start()
    {
        objectMesh = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
    }


    public void ChangeShape(ShapeType shapeType)
    {
        var selectedShape = shape.FirstOrDefault(x => x.shape == shapeType); 
        if (selectedShape != null)
        {
            rigidbodyToAffect.mass = selectedShape.rigidbodyMass;
            objectMesh.sharedMesh = selectedShape.meshFilter;
            meshCollider.sharedMesh = selectedShape.meshFilter;
        }
    }
}