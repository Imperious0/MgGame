namespace Game.Runtime.InGame.Models.Level
{
    public readonly struct CameraData
    {
        public readonly GameItemData CameraTRS;
        public readonly float CameraSize;

        public CameraData(GameItemData cameraTRS, float cameraSize)
        {
            this.CameraTRS = cameraTRS;
            this.CameraSize = cameraSize;
        }
    }
}
