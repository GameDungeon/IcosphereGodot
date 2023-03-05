using Godot;
using System.Collections.Generic;
using System;
using System.Linq;

public class Planet : MeshInstance
{
    int startingRecursiveDepth = 4;
    int radius = 1;

    AbstractIcosphere icosphere;
    HashSet<int> observationGroup;

    List<Vector3> vertices;
    List<int> triangles;
    List<Vector2> UV;

    Dictionary<int, int> meshToAbstractIcosphereMap;
    Dictionary<int, int> abstractIcosphereToUvMap;

    int observerRecursiveDepth = 4;
    int AdjacencySearchRadius = 30;

    // TODO: Find a good way to make this dynamic
    private float r5Threshold  = 0.5336f;
    private float r6Threshold  = 0.3610f;
    private float r7Threshold  = 0.1726f;
    private float r8Threshold  = 0.0941f;
    private float r9Threshold  = 0.0313f;
    private float r10Threshold = 0.0117f;

    private int r4AdjacencySearchRadius = 30;
    private int r5AdjacencySearchRadius = 20;
    private int r6AdjacencySearchRadius = 25;
    private int r7AdjacencySearchRadius = 30;
    private int r8AdjacencySearchRadius = 30;
    private int r9AdjacencySearchRadius = 30;
    private int r10AdjacencySearchRadius = 30;

    public override void _Ready()
    {
        // Generate Icosphere
        icosphere = new AbstractIcosphere(startingRecursiveDepth);
    
        // Initialize Observation Ring
        observationGroup = new HashSet<int>();
    
        generateMesh();

        ShaderMaterial mat = new ShaderMaterial();
        mat.Shader = ResourceLoader.Load<Shader>("res://Shader/planet_shader.tres");
        SetSurfaceMaterial(1, mat);
    }

    public void UpdateRecursion(Vector3 pos)
    {
        foreach(Triangle tri in icosphere.faces.Values)
        {
            tri.vcolor = Color.Color8(230, 230, 230);
        }

        int priorDepth = observerRecursiveDepth;

        (int face_int, float dist) = closest_triangle_index(pos);
        face_int = meshToAbstractIcosphereMap[face_int];

        if(dist >= r5Threshold)
        {
            observerRecursiveDepth = 4;
            AdjacencySearchRadius = r4AdjacencySearchRadius;
        }
        else if(dist < r5Threshold && dist >= r6Threshold)
        {
            observerRecursiveDepth = 5;
            AdjacencySearchRadius = r5AdjacencySearchRadius;
        }
        else if(dist < r6Threshold && dist >= r7Threshold)
        {
            observerRecursiveDepth = 6;
            AdjacencySearchRadius = r6AdjacencySearchRadius;
        }
        else if(dist < r7Threshold && dist >= r8Threshold)
        {
            observerRecursiveDepth = 7;
            AdjacencySearchRadius = r7AdjacencySearchRadius;
        }
        else if(dist < r8Threshold && dist >= r9Threshold)
        {
            observerRecursiveDepth = 8;
            AdjacencySearchRadius = r8AdjacencySearchRadius;
        }
        else if(dist < r9Threshold && dist >= r10Threshold)
        {
            observerRecursiveDepth = 9;
            AdjacencySearchRadius = r9AdjacencySearchRadius;
        }

        observationGroup.Clear();
 
        foreach (Triangle t in icosphere.coupledRingSearch(icosphere.faces[face_int], 10))
        {
            observationGroup.Add(t.uniqueIndex);
        }
 
        // HighlightObservationGroup();

        foreach(int tri_face in observationGroup)
        {
            icosphere.faces[tri_face].vcolor = Color.Color8(100, 100, 100);
            if(icosphere.faces[tri_face].recursiveDepth < observerRecursiveDepth)
            {
                int differential = observerRecursiveDepth - icosphere.faces[tri_face].recursiveDepth;

                icosphere.recurseSingleFaceAndDescendents(tri_face, differential);
            }

            if(priorDepth > observerRecursiveDepth)
            {
                icosphere.ascendToLevel(observerRecursiveDepth);
            }
        }

        icosphere.faces[face_int].vcolor = Color.Color8(255, 255, 0);

        generateMesh();
    }
   
    private void generateMesh()
    {
        List<Vector3> mesh_vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Color> vertex_colors = new List<Color>();

        meshToAbstractIcosphereMap = new Dictionary<int, int>();

        int currentMeshTriange = 0;
        foreach (Triangle abstractTriangle in icosphere.faces.Values)
        {
            if(!abstractTriangle.visible)
            {
                continue;
            }

            // Add the vertices to the mesh.  
            mesh_vertices.Add(radius * abstractTriangle.v1);
            mesh_vertices.Add(radius * abstractTriangle.v2);
            mesh_vertices.Add(radius * abstractTriangle.v3);

            // Create the mesh triangle.
            indices.Add((currentMeshTriange * 3));
            indices.Add((currentMeshTriange * 3) + 1);
            indices.Add((currentMeshTriange * 3) + 2);

            meshToAbstractIcosphereMap.Add(currentMeshTriange, abstractTriangle.uniqueIndex);

            vertex_colors.Add(abstractTriangle.vcolor);
            vertex_colors.Add(new Color(0,0,0));
            vertex_colors.Add(abstractTriangle.vcolor);

            currentMeshTriange++;
        }

        var array_mesh = new ArrayMesh();
        var arrays = new Godot.Collections.Array();

        arrays.Resize((int)ArrayMesh.ArrayType.Max);
        arrays[(int)ArrayMesh.ArrayType.Vertex] = mesh_vertices.ToArray();
        arrays[(int)ArrayMesh.ArrayType.Color] = vertex_colors.ToArray();
        arrays[(int)ArrayMesh.ArrayType.Index] = indices.ToArray();

        array_mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

        // MeshDataTool

        var mdt = new MeshDataTool();
        mdt.CreateFromSurface(array_mesh, 0);


        for (int i = 0; i < mdt.GetFaceCount(); i++)
        {
            var a = mdt.GetFaceVertex(i, 0);
            var b = mdt.GetFaceVertex(i, 1);
            var c = mdt.GetFaceVertex(i, 2);

            var ap = mdt.GetVertex(a);
            var bp = mdt.GetVertex(b);
            var cp = mdt.GetVertex(c);

            var n = (bp - cp).Cross(ap - bp).Normalized();

            mdt.SetVertexNormal(a, n + mdt.GetVertexNormal(a));
            mdt.SetVertexNormal(b, n + mdt.GetVertexNormal(b));
            mdt.SetVertexNormal(c, n + mdt.GetVertexNormal(c));
        }

        for (int i = 0; i < mdt.GetVertexCount(); i++)
        {
            var v = mdt.GetVertexNormal(i).Normalized();
            mdt.SetVertexNormal(i, v);
        }

        //array_mesh.SurfaceRemove(0);
        mdt.CommitToSurface(array_mesh);

        Mesh = array_mesh;
    }

    // TODO: This really needs to be changed for improved proformance,
    // Godot dosn't have some of the methods unity has for dealing with this.
    // I refuse to belive the iterating over every face is the best way.
    private (int, float) closest_triangle_index(Vector3 global_ray_from)
    {
        List<Vector3> intersections = new List<Vector3>();
        List<int> faces = new List<int>();
        Vector3 direction = Translation - global_ray_from;
	    for(int surface_index = 0; surface_index < Mesh.GetSurfaceCount(); surface_index++)
        {
	    	var surface_arrays = Mesh.SurfaceGetArrays(surface_index);
            
	    	var vertex_array = ((Vector3[])surface_arrays[(int)Mesh.ArrayType.Vertex]).Select(v => GlobalTransform.Xform(v)).ToArray(); 
	    	var vertex_array_length = vertex_array.Length;

	    	var face_array = (int[])surface_arrays[(int)Mesh.ArrayType.Index];
	    	var face_array_length = face_array.Length;

	    	for(int face_index = 0; face_index * 3 < face_array_length; face_index++)
            {
	    		var intersection = Geometry.RayIntersectsTriangle(
	    			global_ray_from,
	    			direction,
	    			vertex_array[face_array[face_index * 3 + 2]],
	    			vertex_array[face_array[face_index * 3 + 1]],
	    			vertex_array[face_array[face_index * 3 + 0]]
	    		);
	    		if(intersection != null)
                {
                    intersections.Add((Vector3)intersection);
                    faces.Add(face_index);
                }
            }
        }

        float distance = Mathf.Inf;
        int out_face_index = -1;
        foreach(Tuple<Godot.Vector3, int> intersection_and_face in intersections.Zip(faces, Tuple.Create))
        {
            float new_dist = global_ray_from.DistanceTo(intersection_and_face.Item1);
            if(distance > new_dist)
            {
                distance = new_dist;
                out_face_index = intersection_and_face.Item2;
            }

        }

        return (out_face_index, distance);
    }
}