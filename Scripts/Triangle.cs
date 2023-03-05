using Godot;
using System;
class Triangle
{
    public int uniqueIndex;

    public Triangle parent = null;

    public int recursiveDepth;

    public Vector3 v1;
    public Vector3 v2;
    public Vector3 v3;

    public Triangle adjacency1;
    public Triangle adjacency2;
    public Triangle adjacency3;

    public bool visible = true;

    public Triangle descendant1;
    public Triangle descendant2;
    public Triangle descendant3;
    public Triangle descendant4;

    public Color vcolor = Color.Color8(230, 230, 230);

    public Triangle[] descendants
    { 
        get 
        {
            return new Triangle[] {descendant1, descendant2, descendant3, descendant4};
        }
    }

    public Triangle[] adjacencies 
    { 
        get 
        {
            return new Triangle[] {adjacency1, adjacency2, adjacency3};
        }
    }

    public Vector3[] vertices
    {
        get
        {
            return new Vector3[] {v1, v2, v3};
        }
    }

    public int descendant_number
    {
        get
        {
            if(parent.descendant1 == this)
                return 1;
            else if(parent.descendant2 == this)
                return 2;
            else if(parent.descendant3 == this)
                return 3;
            else if(parent.descendant4 == this)
                return 4;
            else return -1; // Error
        }
    }

    public Triangle(int uniqueIndex, int recursiveDepth, Vector3 v1, Vector3 v2, Vector3 v3) {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;

        this.uniqueIndex = uniqueIndex;
        this.recursiveDepth = recursiveDepth;
    }

    public Triangle(int uniqueIndex, Triangle parent, int recursiveDepth, Vector3 v1, Vector3 v2, Vector3 v3) {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;

        this.parent = parent;

        this.uniqueIndex = uniqueIndex;
        this.recursiveDepth = recursiveDepth;
    }
}