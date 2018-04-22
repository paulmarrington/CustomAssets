namespace Events {
  public interface Listener {
    Channel Channel { get; }
    void  OnTriggered(Listener listener);
  }
}