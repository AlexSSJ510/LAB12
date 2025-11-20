using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

/// <summary>
/// Pruebas de integración (PlayMode) para LootChestController.
/// Verifican que el cofre se abre una sola vez y que su estado IsOpened
/// refleja correctamente la interacción.
/// </summary>
public class LootChestInteractionTests
{
    /// <summary>
    /// El cofre empieza cerrado, se abre al interactuar,
    /// y no cambia en interacciones posteriores.
    /// </summary>
    [UnityTest]
    public IEnumerator LootChest_Interact_OpensOnce_AndStaysOpened()
    {
        // Arrange: crear un GameObject vacío y agregarle LootChestController
        GameObject chestObject = new GameObject("TestLootChest");
        LootChestController lootChest = chestObject.AddComponent<LootChestController>();

        // Estado inicial esperado
        Assert.IsFalse(lootChest.IsOpened, 
            "El cofre debería estar cerrado (IsOpened = false) al inicializar.");

        // Act: primera interacción → debería abrir el cofre
        lootChest.Interact();

        // Esperar un frame para que Unity procese la lógica
        yield return null;

        // Assert: después de la primera interacción, el cofre debe estar abierto
        Assert.IsTrue(lootChest.IsOpened,
            "El cofre debería estar abierto (IsOpened = true) después de la primera interacción.");

        // Act: segunda interacción → no debería cambiar nada
        lootChest.Interact();
        yield return null;

        // Assert: sigue abierto, sin cambios
        Assert.IsTrue(lootChest.IsOpened,
            "El cofre debería permanecer abierto en interacciones posteriores.");

        // Limpieza
        Object.Destroy(chestObject);
        yield return null;
    }

    /// <summary>
    /// Dos cofres distintos funcionan de forma independiente.
    /// </summary>
    [UnityTest]
    public IEnumerator MultipleLootChests_WorkIndependently()
    {
        // Arrange
        GameObject chestObj1 = new GameObject("TestChest1");
        GameObject chestObj2 = new GameObject("TestChest2");
        LootChestController lootChest1 = chestObj1.AddComponent<LootChestController>();
        LootChestController lootChest2 = chestObj2.AddComponent<LootChestController>();

        // Ambos empiezan cerrados
        Assert.IsFalse(lootChest1.IsOpened, "Cofre 1 debería empezar cerrado.");
        Assert.IsFalse(lootChest2.IsOpened, "Cofre 2 debería empezar cerrado.");

        // Act: solo interactuamos con el primer cofre
        lootChest1.Interact();
        yield return null;

        // Assert: cofre 1 abierto, cofre 2 sigue cerrado
        Assert.IsTrue(lootChest1.IsOpened, "Cofre 1 debería estar abierto.");
        Assert.IsFalse(lootChest2.IsOpened, "Cofre 2 debería seguir cerrado.");

        // Limpieza
        Object.Destroy(chestObj1);
        Object.Destroy(chestObj2);
        yield return null;
    }
}
