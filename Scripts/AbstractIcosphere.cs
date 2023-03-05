using Godot;
using System;
using System.Collections.Generic;

class AbstractIcosphere
{
    public Dictionary<int, Triangle> faces;

    private int latestIndex = 20;

    public AbstractIcosphere(int recursiveDepth) 
    {
        // Intialize 
        faces = new Dictionary<int, Triangle>();
        
        // Generate the base 20 faces of the Icosphere. (See the last blog post)
        generateUnitIcosahedron();
    
        // Recurse the icosphere.
        for (int i = 0; i < recursiveDepth; i++)
        {
            uniformlyRecurseIcosphere();
        }
    
        //purgeAncestry();
    }

    private void generateUnitIcosahedron()
    {
        // The golden ratio
        float t = (1.0f + (float) Math.Sqrt(5.0)) / 2.0f;
        
        // A collection of the 12 base vertices.
        Dictionary<int, Vector3> vertices = new Dictionary<int, Vector3>();
        
        // Trace the four vertices of a Golden rectangle [R1]
        vertices.Add(0, new Vector3(-1,  0,  t).Normalized());
        vertices.Add(1, new Vector3( 1,  0,  t).Normalized());
        vertices.Add(2, new Vector3(-1,  0, -t).Normalized());
        vertices.Add(3, new Vector3( 1,  0, -t).Normalized());

        // Trace the four verices of a Golden rectangle orthagonal to the last [R2]
        vertices.Add(4, new Vector3( 0,  t, -1).Normalized());
        vertices.Add(5, new Vector3( 0,  t,  1).Normalized());
        vertices.Add(6, new Vector3( 0, -t, -1).Normalized());
        vertices.Add(7, new Vector3( 0, -t,  1).Normalized());
        
        // Trace the four verices of a Golden rectangle orthagonal to the last two [R3]
        vertices.Add(8,  new Vector3( t, -1, 0).Normalized());
        vertices.Add(9,  new Vector3( t,  1, 0).Normalized());
        vertices.Add(10, new Vector3(-t, -1, 0).Normalized());
        vertices.Add(11, new Vector3(-t,  1, 0).Normalized());

        // 5 faces around point 0
        faces.Add(0, new Triangle(0, 0, vertices[0], vertices[11], vertices[5]));
        faces.Add(1, new Triangle(1, 0, vertices[0], vertices[5], vertices[1]));
        faces.Add(2, new Triangle(2, 0, vertices[0], vertices[1], vertices[7]));
        faces.Add(3, new Triangle(3, 0, vertices[0], vertices[7], vertices[10]));
        faces.Add(4, new Triangle(4, 0, vertices[0], vertices[10], vertices[11]));
        
        // 5 adjacent faces
        faces.Add(5, new Triangle(5, 0, vertices[1], vertices[5], vertices[9]));
        faces.Add(6, new Triangle(6, 0, vertices[5], vertices[11], vertices[4]));
        faces.Add(7, new Triangle(7, 0, vertices[11], vertices[10], vertices[2]));
        faces.Add(8, new Triangle(8, 0, vertices[10], vertices[7], vertices[6]));
        faces.Add(9, new Triangle(9, 0, vertices[7], vertices[1], vertices[8]));
        
        // 5 faces around point 3
        faces.Add(10, new Triangle(10, 0, vertices[3], vertices[9], vertices[4]));
        faces.Add(11, new Triangle(11, 0, vertices[3], vertices[4], vertices[2]));
        faces.Add(12, new Triangle(12, 0, vertices[3], vertices[2], vertices[6]));
        faces.Add(13, new Triangle(13, 0, vertices[3], vertices[6], vertices[8]));
        faces.Add(14, new Triangle(14, 0, vertices[3], vertices[8], vertices[9]));
        
        // 5 adjacent faces
        faces.Add(15, new Triangle(15, 0, vertices[4], vertices[9], vertices[5]));
        faces.Add(16, new Triangle(16, 0, vertices[2], vertices[4], vertices[11]));
        faces.Add(17, new Triangle(17, 0, vertices[6], vertices[2], vertices[10]));
        faces.Add(18, new Triangle(18, 0, vertices[8], vertices[6], vertices[7]));
        faces.Add(19, new Triangle(19, 0, vertices[9], vertices[8], vertices[1]));

        // Face 0 adjacencies;
        faces[0].adjacency1 = faces[4];
        faces[0].adjacency2 = faces[6];
        faces[0].adjacency3 = faces[1];
        
        // Face 1 adjacencies
        faces[1].adjacency1 = faces[0];
        faces[1].adjacency2 = faces[5];
        faces[1].adjacency3 = faces[2];
        
        // Face 2 adjacencies
        faces[2].adjacency1 = faces[1];
        faces[2].adjacency2 = faces[9];
        faces[2].adjacency3 = faces[3];
        
        // Face 3 adjacencies 
        faces[3].adjacency1 = faces[2];
        faces[3].adjacency2 = faces[8];
        faces[3].adjacency3 = faces[4];
        
        // Face 4 adjacencies
        faces[4].adjacency1 = faces[3];
        faces[4].adjacency2 = faces[7];
        faces[4].adjacency3 = faces[0];
        
        // Face 5 adjacencies
        faces[5].adjacency1 = faces[1];
        faces[5].adjacency2 = faces[15];
        faces[5].adjacency3 = faces[19];
        
        // Face 6 adjacencies
        faces[6].adjacency1 = faces[0];
        faces[6].adjacency2 = faces[16];
        faces[6].adjacency3 = faces[15];
        
        // Face 7 adjacencies
        faces[7].adjacency1 = faces[4];
        faces[7].adjacency2 = faces[17];
        faces[7].adjacency3 = faces[16];
        
        // Face 8 adjacencies
        faces[8].adjacency1 = faces[3];
        faces[8].adjacency2 = faces[18];
        faces[8].adjacency3 = faces[17];
        
        // Face 9 adjacencies
        faces[9].adjacency1 = faces[2];
        faces[9].adjacency2 = faces[19];
        faces[9].adjacency3 = faces[18];
        
        // Face 10 adjacencies
        faces[10].adjacency1 = faces[14];
        faces[10].adjacency2 = faces[15];
        faces[10].adjacency3 = faces[11];
        
        // Face 11 adjacencies
        faces[11].adjacency1 = faces[10];
        faces[11].adjacency2 = faces[16];
        faces[11].adjacency3 = faces[12];
        
        // Face 12 adjacencies
        faces[12].adjacency1 = faces[11];
        faces[12].adjacency2 = faces[17];
        faces[12].adjacency3 = faces[13];
        
        // Face 13 adjacencies
        faces[13].adjacency1 = faces[12];
        faces[13].adjacency2 = faces[18];
        faces[13].adjacency3 = faces[14];
        
        // Face 14 adjacencies
        faces[14].adjacency1 = faces[13];
        faces[14].adjacency2 = faces[19];
        faces[14].adjacency3 = faces[10];
        
        // Face 15 adjacencies
        faces[15].adjacency1 = faces[10];
        faces[15].adjacency2 = faces[5];
        faces[15].adjacency3 = faces[6];
        
        // Face 16 adjacencies
        faces[16].adjacency1 = faces[11];
        faces[16].adjacency2 = faces[6];
        faces[16].adjacency3 = faces[7];
        
        // Face 17 adjacencies
        faces[17].adjacency1 = faces[12];
        faces[17].adjacency2 = faces[7];
        faces[17].adjacency3 = faces[8];
        
        // Face 18 adjacencies
        faces[18].adjacency1 = faces[13];
        faces[18].adjacency2 = faces[8];
        faces[18].adjacency3 = faces[9];
        
        // Face 19 adjacencies
        faces[19].adjacency1 = faces[14];
        faces[19].adjacency2 = faces[9];
        faces[19].adjacency3 = faces[5];
    }

    // Recurse a single face of the icosphere.
    public void recurseSingleFace(int index)
    {
        // Obtain the face to recurse.
        Triangle faceToRecurse = faces[index];

        if(faceToRecurse.descendant1 != null || faceToRecurse.descendant2 != null || 
           faceToRecurse.descendant3 != null || faceToRecurse.descendant4 != null)
        {
            faceToRecurse.visible = false;
            faceToRecurse.descendant1.visible = true;
            faceToRecurse.descendant2.visible = true;
            faceToRecurse.descendant3.visible = true;
            faceToRecurse.descendant4.visible = true;
        }
    
        // Determine the recursive depth of these triangles.
        int newDepth = faceToRecurse.recursiveDepth + 1;
    
        // Obtain the midpoints of each side of the triangle.
        Vector3 a = obtainMidpoint(faceToRecurse.v1, faceToRecurse.v2).Normalized();
        Vector3 b = obtainMidpoint(faceToRecurse.v2, faceToRecurse.v3).Normalized();
        Vector3 c = obtainMidpoint(faceToRecurse.v3, faceToRecurse.v1).Normalized();
    
        // Construct four triangles.
        // uniqueIndex, parent, recursive depth, vertex 1, vertex 2, vertex 3 
        Triangle t1 = new Triangle(latestIndex++, faceToRecurse, newDepth, faceToRecurse.v1, a, c); 
        Triangle t2 = new Triangle(latestIndex++, faceToRecurse, newDepth, a, faceToRecurse.v2, b);
        Triangle t3 = new Triangle(latestIndex++, faceToRecurse, newDepth, c, b, faceToRecurse.v3);
        Triangle t4 = new Triangle(latestIndex++, faceToRecurse, newDepth, b, c, a);
    
        // Assign these triangles as descendants of the parent.
        faceToRecurse.descendant1 = t1;
        faceToRecurse.descendant2 = t2;
        faceToRecurse.descendant3 = t3;
        faceToRecurse.descendant4 = t4;

        t1.vcolor = faceToRecurse.vcolor;
        t2.vcolor = faceToRecurse.vcolor;
        t3.vcolor = faceToRecurse.vcolor;
        t4.vcolor = faceToRecurse.vcolor;
    
        // Add the new faces to the dictionary.
        faces.Add(t1.uniqueIndex, t1);
        faces.Add(t2.uniqueIndex, t2);
        faces.Add(t3.uniqueIndex, t3);
        faces.Add(t4.uniqueIndex, t4);
    
        // Set the parent to invisible.
        faceToRecurse.visible = false;

        // Determine the adjacencies of each new triangle
        updateDescendant1Adjacencies(t1, true);
        updateDescendant2Adjacencies(t2, true);
        updateDescendant3Adjacencies(t3, true);
        updateDescendant4Adjacencies(t4, true);
    }

    public void recurseSingleFaceAndDescendents(int index, int targetGeneration)
    {
        recurseSingleFaceAndDescendents(index, 0, targetGeneration);  
    }

    public void recurseSingleFaceAndDescendents(int index, int currentGeneration, int targetGeneration)
    {
        if(currentGeneration < targetGeneration)
        {
            recurseSingleFace(index);

            recurseSingleFaceAndDescendents(faces[index].descendant1.uniqueIndex, currentGeneration + 1, targetGeneration);
            recurseSingleFaceAndDescendents(faces[index].descendant2.uniqueIndex, currentGeneration + 1, targetGeneration);
            recurseSingleFaceAndDescendents(faces[index].descendant3.uniqueIndex, currentGeneration + 1, targetGeneration);
            recurseSingleFaceAndDescendents(faces[index].descendant4.uniqueIndex, currentGeneration + 1, targetGeneration);
        }
    }

    public void ascendToLevel(int targetDepth)
    {
        foreach(Triangle tri in faces.Values)
        {
            if(!tri.visible || tri.recursiveDepth <= targetDepth) {continue;}

            tri.visible = false;

            if (tri.adjacency1.recursiveDepth == tri.recursiveDepth) {tri.adjacency1.visible = false;}
            if (tri.adjacency2.recursiveDepth == tri.recursiveDepth) {tri.adjacency2.visible = false;}
            if (tri.adjacency3.recursiveDepth == tri.recursiveDepth) {tri.adjacency3.visible = false;}

            int currentDepth = tri.recursiveDepth;
            Triangle currentTriangle = tri;

            while(currentDepth > targetDepth)
            {
                Triangle parent = currentTriangle.parent;
                currentTriangle = parent;
                currentDepth = parent.recursiveDepth;
            }

            currentTriangle.visible = true;
        }
    }

    // Find the midpoint between two 3D points.
    private Vector3 obtainMidpoint(Vector3 p1, Vector3 p2)
    {
        return (p1 + p2) / 2.0f;
    } 

    private void uniformlyRecurseIcosphere()
    {
        // Create a temporary dictionary of all visible faces.  We will recurse this temporary collection so we don't
        // cross-contaminate our master collection while we are in the process iterating through it.
        Dictionary<int, Triangle> temporaryFaces = new Dictionary<int, Triangle>();
    
        // Pull out the visible faces.
        foreach (Triangle t in faces.Values)
        {
            if (t.visible)
            {
                temporaryFaces.Add(t.uniqueIndex, t);
            }
        }
    
        // Each of these Triangles is a reference.  Thus, updating it in our temporary collection
        // will also update it in our master collection.
        foreach (Triangle t in temporaryFaces.Values)
        {
            recurseSingleFace(t.uniqueIndex);
        }
    }

    // Update the adjacencies of the triangle t, and also update the adjacencies of every neighboring triangle.
    // If recurse is set to true, we need to update neighbors.  If it is false, then this is a neighbor's update; don't update neighbors.
    private void updateDescendant1Adjacencies(Triangle t, bool recurse)
    {
        // Obtain relevant parental adjacencies.
        Triangle parentalNeighbor1 = t.parent.adjacency1;
        Triangle parentalNeighbor3 = t.parent.adjacency3;
    
        // Set adjacency to child 4.
        t.adjacency2 = t.parent.descendant4;
    
        // See how deeply parental neighbor 1 recurses.
        Triangle deepestDescendantOfParentalNeighbor1 = descendToRecursiveLevel(parentalNeighbor1, t.recursiveDepth);
    
        // Either we will reach a matching depth, or we will reach a more shallow depth.
        if (deepestDescendantOfParentalNeighbor1.recursiveDepth < t.recursiveDepth)
        {
            // If we have reached a more shallow depth, this triangle is t's neighbor.
            t.adjacency1 = deepestDescendantOfParentalNeighbor1;
    
            // In this case, the deepest descendant of parental neighbor 1 needs no adjacency update.  
            // It's adjacency is at a level more shallow than t.
        }
        else
        {
            // If we have reached a matching depth, we must scan all co-descendants for an adjacency.
            t.adjacency1 = scanForSharedVertices(t, deepestDescendantOfParentalNeighbor1.parent);
    
            // In this case, the co-descendants of the parental neighbor need an adjacency update; they need knowledge of t and its siblings.
            // TODO: This seems like a heavy handed solution.  Maybe there is a more graceful way to update these co-descendants.
            if (recurse)
            {
                updateDescendant1Adjacencies(deepestDescendantOfParentalNeighbor1.parent.descendant1, false);
                updateDescendant2Adjacencies(deepestDescendantOfParentalNeighbor1.parent.descendant2, false);
                updateDescendant3Adjacencies(deepestDescendantOfParentalNeighbor1.parent.descendant3, false);
            }
        }
    
        // See how deeply parental neighbor 3 recurses. 
        Triangle deepestDescendantOfParentalNeighbor3 = descendToRecursiveLevel(parentalNeighbor3, t.recursiveDepth);
    
        // Either we will reach a matching depth, or we will reach a more shallow depth.
        if (deepestDescendantOfParentalNeighbor3.recursiveDepth < t.recursiveDepth)
        {
            // If we have reached a more shallow depth, this triangle is t's neighbor.
            t.adjacency3 = deepestDescendantOfParentalNeighbor3;
    
            // In this case, the deepest descendant of parental neighbor 3 needs no adjacency update.  
            // It's adjacency is at a level more shallow than t.
        }
        else
        {
            // If we have reached a matching depth, we must scan all co-descendants for an adjacency.
            t.adjacency3 = scanForSharedVertices(t, deepestDescendantOfParentalNeighbor3.parent);
    
            // In this case, the co-descendants of the parental neighbor need an adjacency update; they need knowledge of t and its siblings.
            // TODO: This seems like a heavy handed solution.  Maybe there is a more graceful way to update these co-descendants.
            if (recurse)
            {
                updateDescendant1Adjacencies(deepestDescendantOfParentalNeighbor3.parent.descendant1, false);
                updateDescendant2Adjacencies(deepestDescendantOfParentalNeighbor3.parent.descendant2, false);
                updateDescendant3Adjacencies(deepestDescendantOfParentalNeighbor3.parent.descendant3, false);
            }
        }
    }

    // Update the adjacencies of the triangle t, and also update the adjacencies of every neighboring triangle.
    // If recurse is set to true, we need to update neighbors.  If it is false, then this is a neighbor's update; don't update neighbors.
    private void updateDescendant2Adjacencies(Triangle t, bool recurse)
    {
        // Obtain relevant parental adjacencies.
        Triangle parentalNeighbor1 = t.parent.adjacency1;
        Triangle parentalNeighbor2 = t.parent.adjacency2;
    
        // Set adjacency to child 4.
        t.adjacency3 = t.parent.descendant4;
    
        // See how deeply parental neighbor 1 recurses.
        Triangle deepestDescendantOfParentalNeighbor1 = descendToRecursiveLevel(parentalNeighbor1, t.recursiveDepth);
    
        // Either we will reach a matching depth, or we will reach a more shallow depth.
        if (deepestDescendantOfParentalNeighbor1.recursiveDepth < t.recursiveDepth)
        {
            // If we have reached a more shallow depth, this triangle is t's neighbor.
            t.adjacency1 = deepestDescendantOfParentalNeighbor1;
    
            // In this case, the deepest descendant of parental neighbor 1 needs no adjacency update.  
            // It's adjacency is at a level more shallow than t.
        }
        else
        {
            // If we have reached a matching depth, we must scan all co-descendants for an adjacency.
            t.adjacency1 = scanForSharedVertices(t, deepestDescendantOfParentalNeighbor1.parent);
    
            // In this case, the co-descendants of the parental neighbor need an adjacency update; they need knowledge of t and its siblings.
            // TODO: This seems like a heavy handed solution.  Maybe there is a more graceful way to update these co-descendants.
            if (recurse)
            {
                updateDescendant1Adjacencies(deepestDescendantOfParentalNeighbor1.parent.descendant1, false);
                updateDescendant2Adjacencies(deepestDescendantOfParentalNeighbor1.parent.descendant2, false);
                updateDescendant3Adjacencies(deepestDescendantOfParentalNeighbor1.parent.descendant3, false);
            }
        }
    
        // See how deeply parental neighbor 3 recurses. 
        Triangle deepestDescendantOfParentalNeighbor2 = descendToRecursiveLevel(parentalNeighbor2, t.recursiveDepth);
    
        // Either we will reach a matching depth, or we will reach a more shallow depth.
        if (deepestDescendantOfParentalNeighbor2.recursiveDepth < t.recursiveDepth)
        {
            // If we have reached a more shallow depth, this triangle is t's neighbor.
            t.adjacency2 = deepestDescendantOfParentalNeighbor2;
    
            // In this case, the deepest descendant of parental neighbor 3 needs no adjacency update.  
            // It's adjacency is at a level more shallow than t.
        }
        else
        {
            // If we have reached a matching depth, we must scan all co-descendants for an adjacency.
            t.adjacency2 = scanForSharedVertices(t, deepestDescendantOfParentalNeighbor2.parent);
    
            // In this case, the co-descendants of the parental neighbor need an adjacency update; they need knowledge of t and its siblings.
            // TODO: This seems like a heavy handed solution.  Maybe there is a more graceful way to update these co-descendants.
            if (recurse)
            {
                updateDescendant1Adjacencies(deepestDescendantOfParentalNeighbor2.parent.descendant1, false);
                updateDescendant2Adjacencies(deepestDescendantOfParentalNeighbor2.parent.descendant2, false);
                updateDescendant3Adjacencies(deepestDescendantOfParentalNeighbor2.parent.descendant3, false);
            }
        }
    }

    // Update the adjacencies of the triangle t, and also update the adjacencies of every neighboring triangle.
    // If recurse is set to true, we need to update neighbors.  If it is false, then this is a neighbor's update; don't update neighbors.
    private void updateDescendant3Adjacencies(Triangle t, bool recurse)
    {
        // Obtain relevant parental adjacencies.
        Triangle parentalNeighbor2 = t.parent.adjacency2;
        Triangle parentalNeighbor3 = t.parent.adjacency3;
    
        // Set adjacency to child 4.
        t.adjacency1 = t.parent.descendant4;
    
        // See how deeply parental neighbor 1 recurses.
        Triangle deepestDescendantOfParentalNeighbor2 = descendToRecursiveLevel(parentalNeighbor2, t.recursiveDepth);
    
        // Either we will reach a matching depth, or we will reach a more shallow depth.
        if (deepestDescendantOfParentalNeighbor2.recursiveDepth < t.recursiveDepth)
        {
            // If we have reached a more shallow depth, this triangle is t's neighbor.
            t.adjacency2 = deepestDescendantOfParentalNeighbor2;
    
            // In this case, the deepest descendant of parental neighbor 1 needs no adjacency update.  
            // It's adjacency is at a level more shallow than t.
        }
        else
        {
            // If we have reached a matching depth, we must scan all co-descendants for an adjacency.
            t.adjacency2 = scanForSharedVertices(t, deepestDescendantOfParentalNeighbor2.parent);
    
            // In this case, the co-descendants of the parental neighbor need an adjacency update; they need knowledge of t and its siblings.
            // TODO: This seems like a heavy handed solution.  Maybe there is a more graceful way to update these co-descendants.
            if (recurse)
            {
                updateDescendant1Adjacencies(deepestDescendantOfParentalNeighbor2.parent.descendant1, false);
                updateDescendant2Adjacencies(deepestDescendantOfParentalNeighbor2.parent.descendant2, false);
                updateDescendant3Adjacencies(deepestDescendantOfParentalNeighbor2.parent.descendant3, false);
            }
        }
    
        // See how deeply parental neighbor 3 recurses. 
        Triangle deepestDescendantOfParentalNeighbor3 = descendToRecursiveLevel(parentalNeighbor3, t.recursiveDepth);
    
        // Either we will reach a matching depth, or we will reach a more shallow depth.
        if (deepestDescendantOfParentalNeighbor3.recursiveDepth < t.recursiveDepth)
        {
            // If we have reached a more shallow depth, this triangle is t's neighbor.
            t.adjacency3 = deepestDescendantOfParentalNeighbor3;
    
            // In this case, the deepest descendant of parental neighbor 3 needs no adjacency update.  
            // It's adjacency is at a level more shallow than t.
        }
        else
        {
            // If we have reached a matching depth, we must scan all co-descendants for an adjacency.
            t.adjacency3 = scanForSharedVertices(t, deepestDescendantOfParentalNeighbor3.parent);
    
            // In this case, the co-descendants of the parental neighbor need an adjacency update; they need knowledge of t and its siblings.
            // TODO: This seems like a heavy handed solution.  Maybe there is a more graceful way to update these co-descendants.
            if (recurse)
            {
                updateDescendant1Adjacencies(deepestDescendantOfParentalNeighbor3.parent.descendant1, false);
                updateDescendant2Adjacencies(deepestDescendantOfParentalNeighbor3.parent.descendant2, false);
                updateDescendant3Adjacencies(deepestDescendantOfParentalNeighbor3.parent.descendant3, false);
            }
        }
    }

    // Update the adjacencies of the triangle t, and also update the adjacencies of every neighboring triangle.
    // If recurse is set to true, we need to update neighbors.  If it is false, then this is a neighbor's update; don't update neighbors.
    private void updateDescendant4Adjacencies(Triangle t, bool recurse)
    {
        t.adjacency1 = t.parent.descendant3;
        t.adjacency2 = t.parent.descendant1;
        t.adjacency3 = t.parent.descendant2;
    }

    // Probe from Triangle t to the specified recursive depth.
    // If the level of depth does not exist, return the deepest available depth.
    private Triangle descendToRecursiveLevel(Triangle t, int targetDepth)
    {
        // If we are at the target depth, return this triangle.
        if (t.recursiveDepth == targetDepth)
        {
            return t;
        }
    
        // If this triangle has no descendants, return this triangle.
        if (t.descendant1 == null || t.descendant2 == null || t.descendant3 == null || t.descendant4 == null)
        {
            return t;
        }
    
        // Otherwise, scan for descendants.
        int currentDepth = t.recursiveDepth;
    
        // Get the deepest level available of each descendant.
        Triangle deepestDescendant1 = descendToRecursiveLevel(t.descendant1, targetDepth);
        Triangle deepestDescendant2 = descendToRecursiveLevel(t.descendant2, targetDepth);
        Triangle deepestDescendant3 = descendToRecursiveLevel(t.descendant3, targetDepth);
        Triangle deepestDescendant4 = descendToRecursiveLevel(t.descendant4, targetDepth);
    
        // Find the deepest descendant of the deepest descendants.
        Triangle deepest = deepestDescendant1;
        deepest = (deepestDescendant2.recursiveDepth > deepest.recursiveDepth) ? deepestDescendant2 : deepest;
        deepest = (deepestDescendant3.recursiveDepth > deepest.recursiveDepth) ? deepestDescendant3 : deepest;
        deepest = (deepestDescendant4.recursiveDepth > deepest.recursiveDepth) ? deepestDescendant4 : deepest;
    
        return deepest;
    }

    private Triangle scanForSharedVertices(Triangle scanningFrom, Triangle scanDescendantsOf)
    {
        // For each codescendant,
        Triangle adjacency = null;
        foreach (Triangle potentialAdjacency in scanDescendantsOf.descendants)
        {
            // If we have not yet found an adjacency,
            if (adjacency != null)
            {
                break;
            }
    
            // For each vertex of this codescendant,
            int sharedVertices = 0;
            foreach (Vector3 vertex in potentialAdjacency.vertices)
            {
                // If the vertex matches one of the vertices of the triangle we are scanning from, log it
                if (vertex.Equals(scanningFrom.v1) || vertex.Equals(scanningFrom.v2) || vertex.Equals(scanningFrom.v3))
                {
                    sharedVertices++;
                }
                // If we have found two vertex matches, this is our adjacency; break
                if (sharedVertices >= 2)
                {
                    adjacency = potentialAdjacency;
                    break;
                }
            }
        }
        if(adjacency == null)
        {
            GD.Print(adjacency);
        }
        return adjacency;
    }

    public HashSet<Triangle> coupledRingSearch(Triangle center, int radius)
    {
        // Initialize Collections
        HashSet<Triangle> adjacencies = new HashSet<Triangle>();
        HashSet<Triangle> ring1 = new HashSet<Triangle>();
        HashSet<Triangle> ring2 = new HashSet<Triangle>();
    
        // Add the origin to the collection of all adjacencies and make it ring 1
        adjacencies.Add(center);
        ring1.Add(center);
    
        // Begin coupled ring search
        ring1Search(adjacencies, ring1, ring2, radius, 0);
    
        return adjacencies;
    }
    
    // Search ring 1 for adjacencies.
    private void ring1Search(HashSet<Triangle> allAdjacencies, HashSet<Triangle> ring1, HashSet<Triangle> ring2, int radius, int currentBreadth)
    {
        // If we have not yet reached the search radius,
        if (currentBreadth < radius)
        {
            // Set up a buffer
            HashSet<Triangle> buffer =  new HashSet<Triangle>();
    
            // For each triangle in ring 1,
            foreach (Triangle t in ring1)
            {
                // Add adjacencies (duplicates will be skipped).
                if (t.adjacency1.visible)
                {
                    // Add adjacency 1 to all adjacencies.
                    allAdjacencies.Add(t.adjacency1);
                    
                    // If adjacency 1 is not present in ring 2, save it to be added to the next ring 2.
                    if (!ring2.Contains(t.adjacency1))
                    {
                        // Save adjacency 1 to be added to Ring 2.
                        buffer.Add(t.adjacency1);
                    }
                }
                if (t.adjacency2.visible)
                {
                    // Add adjacency 2 to all adjacencies.
                    allAdjacencies.Add(t.adjacency2);
    
                    // If adjacency 2 is not present in ring 2, save it to be added to the next ring 2.
                    if (!ring2.Contains(t.adjacency2))
                    {
                        // Save adjacency 2 to be added to Ring 2.
                        buffer.Add(t.adjacency2);
                    }
                }
                if (t.adjacency3.visible)
                {
                    // Add adjacency 3 to all adjacencies.
                    allAdjacencies.Add(t.adjacency3);
    
                    // If adjacency 3 is not present in ring 2, save it to be added to the next ring 2.
                    if (!ring2.Contains(t.adjacency3))
                    {
                        // Save adjacency 3 to be added to Ring 2.
                        buffer.Add(t.adjacency3);
                    }
                }
            }
    
            // Now that we are done with the old Ring 2, we can clear it and reset it to the new ring 2.
            ring2.Clear();
            ring2 = buffer;
    
            // Initiate Ring 2 Search.
            ring2Search(allAdjacencies, ring1, ring2, radius, currentBreadth + 1);
        }
    }
    
    // Search Ring 2 for adjacencies.
    private void ring2Search(HashSet<Triangle> allAdjacencies, HashSet<Triangle> ring1, HashSet<Triangle> ring2, int radius, int currentBreadth)
    {
        // If we have not yet reached the search radius,
        if (currentBreadth < radius)
        {
            // Set up a buffer
            HashSet<Triangle> buffer =  new HashSet<Triangle>();
    
            // For each triangle in ring 2,
            foreach (Triangle t in ring2)
            {
                // Add adjacencies (duplicates will be skipped).
                if (t.adjacency1.visible)
                {
                    // Add adjacency 1 to all adjacencies.
                    allAdjacencies.Add(t.adjacency1);
    
                    // If adjacency 1 is not present in ring 1, save it to be added to the next ring 1.
                    if (!ring1.Contains(t.adjacency1))
                    {
                        buffer.Add(t.adjacency1);
                    }
                }
                if (t.adjacency2.visible)
                {
                    // Add adjacency 2 to all adjacencies.
                    allAdjacencies.Add(t.adjacency2);
    
                    // If adjacency 2 is not present in ring 1, save it to be added to the next ring 1.
                    if (!ring1.Contains(t.adjacency2))
                    {
                        buffer.Add(t.adjacency2);
                    }
                }
                if (t.adjacency3.visible)
                {
                    // Add adjacency 3 to all adjacencies.
                    allAdjacencies.Add(t.adjacency3);
                    
                    // If adjacency 3 is not present in ring 1, save it to be added to the next ring 1.
                    if (!ring1.Contains(t.adjacency3))
                    {
                        buffer.Add(t.adjacency3);
                    }
                }
            }
    
            // Now that we are done with the old ring 1, we can clear it and reset it to the new ring 1.
            ring1.Clear();
            ring1 = buffer;
    
            // Initialize Ring 1 search
            ring1Search(allAdjacencies, ring1, ring2, radius, currentBreadth + 1);
        }
    }
}