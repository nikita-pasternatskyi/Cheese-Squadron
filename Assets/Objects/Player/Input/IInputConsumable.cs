namespace Assets.Objects.Player.Input
{
    public interface IInputConsumable
    {
        public bool IsConsumed { get; }
        public void Consume();
        public void Release();
    }
}
