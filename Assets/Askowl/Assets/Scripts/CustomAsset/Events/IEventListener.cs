namespace CustomAsset {
  public interface IEventListener {
    EventActor EventActor { get; }
    void       OnEventRaised(IEventListener listener);
  }
}