using System.Collections;
public interface Effect {
    public IEnumerator Activate(Enemy enemy);
    public IEnumerator Deactivate(Enemy enemy);
}

