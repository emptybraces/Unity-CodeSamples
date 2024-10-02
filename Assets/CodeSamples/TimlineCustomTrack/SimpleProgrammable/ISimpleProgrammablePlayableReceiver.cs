namespace Emptybraces.Timeline
{
    public interface ISimpleProgrammablePlayableReceiver
    {
        public void OnNotify(float value, bool isEnter, bool isExit);
    }
}
