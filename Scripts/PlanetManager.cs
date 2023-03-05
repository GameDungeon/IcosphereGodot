using Godot;
using System;

public class PlanetManager : Spatial
{
    public Planet planet;
    public override void _Ready()
    {
        NewPlanet();
    }

    public void NewPlanet() {
        PackedScene planetScene = ResourceLoader.Load<PackedScene>("res://planet.tscn");
        // Edit planet here
        Node planet_node = planetScene.Instance();
        AddChild(planet_node);

        planet = planet_node as Planet;
        planet.UpdateRecursion(new Vector3(0f, 0f, 3f));
    }
}
