using NUnit.Framework;

public class GameLogicTests
{
    // Una prueba simple que verifica la inicialización correcta.
    [Test]

    public void GameLogic_Initialization_SetsObjectivesCorrectly()
    {
        // ARRANGE: Preparamos el escenario de la prueba
        GameLogic gameLogic;

        gameLogic = new GameLogic(5);

        // ASSERT: Verificamos que el resultado es el esperado.
        Assert.AreEqual(5, gameLogic.ObjectivesToWin);
        Assert.AreEqual(0, gameLogic.ObjectivesCompleted);
        Assert.IsFalse(gameLogic.IsVictoryConditionMet);
    }


    // Una prueba que verifica el comportamiento de completar un objetivo.
    [Test]
    public void CompleteObjective_IncrementsCompletedCount()
    {
        // ARRANGE
        var gameLogic = new GameLogic(3);

        // ACT
        gameLogic.CompleteObjective();

        // ASSERT
        Assert.AreEqual(1, gameLogic.ObjectivesCompleted);
    }

    // Una prueba que verifica la condición de victoria.
    [Test]
    public void IsVictoryConditionMet_ReturnsTrue_WhenObjectivesCompleted()
    {
        // ARRANGE
        var gameLogic = new GameLogic(2);

        // ACT
        gameLogic.CompleteObjective();
        gameLogic.CompleteObjective();

        // ASSERT
        Assert.IsTrue(gameLogic.IsVictoryConditionMet);
    }
}