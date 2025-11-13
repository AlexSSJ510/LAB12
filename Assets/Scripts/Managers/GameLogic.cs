/// <summary>
/// Clase de lógica pura que gestiona el estado de los objetivos
/// No hereda de MonoBehaviour, por lo que puede ser testeada fácilmente
/// </summary>
public class GameLogic
{
    public int ObjectivesToWin { get; }
    public int ObjectivesCompleted { get; private set; }
    public bool IsVictoryConditionMet => ObjectivesCompleted >= ObjectivesToWin;

    public GameLogic(int objectivesToWin)
    {
        ObjectivesToWin = objectivesToWin > 0 ? objectivesToWin : 1;
        ObjectivesCompleted = 0;
    }

    public void CompleteObjective()
    {
        if (!IsVictoryConditionMet)
        {
            ObjectivesCompleted++;
        }
    }
}