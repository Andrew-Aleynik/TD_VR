using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Recipe
{
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private List<MaterialRequirement> requiredMaterials = new List<MaterialRequirement>();

    private Hashtable requirementsTable;
    private bool isInitialized = false;

    [System.Serializable]
    public class MaterialRequirement
    {
        public string materialTag;
        public int requiredCount;
    }

    private void Initialize()
    {
        if (isInitialized) return;

        requirementsTable = new Hashtable();
        foreach (var requirement in requiredMaterials)
        {
            if (!requirementsTable.ContainsKey(requirement.materialTag))
            {
                requirementsTable.Add(requirement.materialTag, requirement.requiredCount);
            }
        }
        isInitialized = true;
    }

    public bool CanBuild(Hashtable availableMaterials)
    {
        Initialize();

        foreach (DictionaryEntry requirement in requirementsTable)
        {
            string materialTag = (string)requirement.Key;
            int requiredCount = (int)requirement.Value;

            if (!availableMaterials.ContainsKey(materialTag))
                return false;

            int availableCount = (int)availableMaterials[materialTag];
            if (availableCount < requiredCount)
            {
                return false;
            }
        }

        return true;
    }

    public GameObject GetTower()
    {
        return towerPrefab;
    }

    public Hashtable GetRequirements()
    {
        Initialize();
        return (Hashtable)requirementsTable.Clone();
    }

    public List<MaterialRequirement> GetRequiredMaterials()
    {
        return requiredMaterials;
    }
}

public class AssemblyManager : MonoBehaviour
{
    public List<Recipe> recipes;
    public Transform towerSpawnPoint;

    private Hashtable materialsCounts = new Hashtable();

    public void AddMaterial(SelectEnterEventArgs args)
    {
        GameObject addedMaterial = args.interactableObject.transform.gameObject;
        string materialTag = addedMaterial.tag;
        Debug.Log("Объект с тегом: " + materialTag + " добавлен");
        ChangeCountOfMaterial(materialTag, 1);
        Build();
    }

    public void RemoveMaterial(SelectExitEventArgs args)
    {
        GameObject removedMaterial = args.interactableObject.transform.gameObject;
        string materialTag = removedMaterial.tag;
        Debug.Log("Объект с тегом: " + materialTag + " удален");
        ChangeCountOfMaterial(materialTag, -1);
    }

    private void ChangeCountOfMaterial(string materialTag, int change)
    {
        if (materialsCounts.ContainsKey(materialTag))
        {
            int currentCount = (int)materialsCounts[materialTag];
            int newCount = currentCount + change;

            if (newCount <= 0)
            {
                materialsCounts.Remove(materialTag);
            }
            else
            {
                materialsCounts[materialTag] = newCount;
            }
        }
        else if (change > 0)
        {
            materialsCounts.Add(materialTag, change);
        }

        DebugMaterialsCount();
    }

    private void Build()
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.CanBuild(materialsCounts))
            {
                Debug.Log("Рецепт выполнен! Создаем башню...");
                ClearMaterialCounts();
                CreateTower(recipe.GetTower());
                break;
            }
        }
    }

    private void CreateTower(GameObject towerPrefab)
    {
        if (towerPrefab == null)
        {
            Debug.LogError("Префаб башни не назначен в рецепте!");
            return;
        }

        if (towerSpawnPoint != null)
        {
            Instantiate(towerPrefab, towerSpawnPoint.position, towerSpawnPoint.rotation);
        }
        else
        {
            Instantiate(towerPrefab, transform.position, transform.rotation);
        }
    }

    private void ClearMaterialCounts()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor[] sockets = GetComponentsInChildren<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        foreach (UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket in sockets)
        {
            if (socket.hasSelection)
            {
                Destroy(socket.interactablesSelected[0].transform.gameObject);
            }
        }

        materialsCounts.Clear();
        Debug.Log("Материалы очищены");
    }

    private void DebugMaterialsCount()
    {
        Debug.Log("=== Текущие материалы ===");
        foreach (DictionaryEntry entry in materialsCounts)
        {
            Debug.Log($"Тег: {entry.Key}, Количество: {entry.Value}");
        }
        Debug.Log("========================");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AssemblyManager))]
public class AssemblyManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        AssemblyManager manager = (AssemblyManager)target;
        
        if (manager.GetComponent<AssemblyManager>() != null && 
            manager.GetComponent<AssemblyManager>().recipes != null)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Рецепты:", EditorStyles.boldLabel);
            
            int index = 1;
            foreach (Recipe recipe in manager.GetComponent<AssemblyManager>().recipes)
            {
                if (recipe != null)
                {
                    EditorGUILayout.LabelField($"Рецепт {index}:");
                    
                    var requirements = recipe.GetRequiredMaterials();
                    if (requirements != null)
                    {
                        foreach (var req in requirements)
                        {
                            EditorGUILayout.LabelField($"  - {req.materialTag}: {req.requiredCount}");
                        }
                    }
                    
                    GameObject tower = recipe.GetTower();
                    if (tower != null)
                    {
                        EditorGUILayout.LabelField($"  Башня: {tower.name}");
                    }
                    
                    EditorGUILayout.Space();
                    index++;
                }
            }
        }
    }
}
#endif