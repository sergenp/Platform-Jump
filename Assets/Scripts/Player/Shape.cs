using System;
using UnityEngine;
public enum ShapeType
{
    Cube,
    Sphere
}

[Serializable]
public class Shape
{
    public int rigidbodyMass;
    public ShapeType shape;
    public Mesh meshFilter;
}
