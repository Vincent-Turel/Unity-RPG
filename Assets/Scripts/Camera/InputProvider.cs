namespace Cameras
{
    public class InputProvider : Cinemachine.CinemachineInputProvider
    {
        public bool InputEnabled; // Enabled when Right click is pressed in CameraMouseControls script.
        public override float GetAxisValue(int axis)
        {
            if(!InputEnabled)
                return 0;
            return axis == 0 ? base.GetAxisValue(axis) : 0;  
        }
    }
}
