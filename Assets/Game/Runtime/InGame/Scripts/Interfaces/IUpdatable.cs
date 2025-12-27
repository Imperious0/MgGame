namespace Game.Runtime.InGame.Scripts.Interfaces
{
    public interface IUpdatable
    {
        public void Tick(float deltaTime, float unscaledDeltaTime);
    }
}
