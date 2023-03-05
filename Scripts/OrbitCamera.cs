using Godot;
using System;

public class OrbitCamera : Spatial
{
    [Export(PropertyHint.Range, "0.1,1.0")]
    float mouse_sensitivity = 0.3f;

    private Camera camera;
    private PlanetManager manager;
    private bool mouseDown = false;
    public override void _Ready()
    {
        camera = GetNode<Camera>("Camera");

        manager = GetParent() as PlanetManager;
    }

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if(@event is InputEventMouseMotion motionEvent && Input.IsMouseButtonPressed(((int)ButtonList.Left)))
        {
            Vector3 rotDeg = RotationDegrees;
            rotDeg.y -= motionEvent.Relative.x * mouse_sensitivity;
            rotDeg.x -= motionEvent.Relative.y * mouse_sensitivity;
            rotDeg.x = Mathf.Clamp(rotDeg.x, -90, 90);
            RotationDegrees = rotDeg;

            manager.planet.UpdateRecursion(camera.GlobalTranslation);
        } 
        else if (@event is InputEventMouseButton){
            InputEventMouseButton emb = (InputEventMouseButton)@event;
            if (emb.IsPressed()) {
                if (emb.ButtonIndex == (int)ButtonList.WheelUp){
                    var oldTranslaton = camera.Translation;
                    oldTranslaton -= new Vector3(0f, 0f, 0.04f);
                    oldTranslaton.z = Mathf.Max(oldTranslaton.z, 1.05f);
                    camera.Translation = oldTranslaton;
                }
                if (emb.ButtonIndex == (int)ButtonList.WheelDown){
                    camera.Translation += new Vector3(0f, 0f, 0.04f);
                }

                manager.planet.UpdateRecursion(camera.GlobalTranslation);
            }
        }
    }
}


