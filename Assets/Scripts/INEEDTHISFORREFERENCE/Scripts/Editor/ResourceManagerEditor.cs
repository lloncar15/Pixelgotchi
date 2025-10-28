#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    private ResourceManager resourceManager;
    private ResourceConfiguration config;
    private SerializedProperty resourceConfigurationProp;
    
    // Quick add configuration
    private int[] quickAddAmounts = new int[] { 10, 50, 100 };
    private bool showQuickAddConfig = false;
    
    // Per-resource foldout states
    private bool[] resourceFoldouts;
    
    // Scene collection foldout
    private bool showSceneCollection = false;
    
    // Custom add
    private int customAddIndex = 0;
    private int customAddAmount = 0;
    
    // Styling
    private GUIStyle headerStyle;
    private GUIStyle sectionStyle;
    private GUIStyle resourceBoxStyle;
    private bool stylesInitialized = false;

    private void OnEnable()
    {
        resourceManager = (ResourceManager)target;
        resourceConfigurationProp = serializedObject.FindProperty("resourceConfiguration");
        
        RefreshConfiguration();
    }

    private void RefreshConfiguration()
    {
        config = resourceManager.ResourceConfiguration;
        
        if (config)
        {
            ResourceBase[] resources = config.GetAllResources();
            if (resourceFoldouts == null || resourceFoldouts.Length != resources.Length)
            {
                resourceFoldouts = new bool[resources.Length];
                // Start with all expanded
                for (int i = 0; i < resourceFoldouts.Length; i++)
                {
                    resourceFoldouts[i] = true;
                }
            }
        }
    }

    private void InitializeStyles()
    {
        if (stylesInitialized) return;
        
        headerStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleLeft
        };
        
        sectionStyle = new GUIStyle(EditorStyles.helpBox)
        {
            padding = new RectOffset(10, 10, 10, 10)
        };
        
        resourceBoxStyle = new GUIStyle(EditorStyles.helpBox)
        {
            padding = new RectOffset(8, 8, 5, 5),
            margin = new RectOffset(0, 0, 2, 2)
        };
        
        stylesInitialized = true;
    }

    public override void OnInspectorGUI()
    {
        InitializeStyles();
        serializedObject.Update();
        
        // Auto-refresh during play mode
        if (Application.isPlaying)
        {
            Repaint();
        }
        
        DrawHeader();
        EditorGUILayout.Space(5);
        
        // Configuration reference
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(resourceConfigurationProp, new GUIContent("Resource Configuration"));
        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
            RefreshConfiguration();
        }
        
        EditorGUILayout.Space(10);
        
        if (config == null)
        {
            EditorGUILayout.HelpBox("No ResourceConfiguration assigned. Please assign one to manage resources.", MessageType.Warning);
            return;
        }
        
        ResourceBase[] resources = config.GetAllResources();
        
        if (resources == null || resources.Length == 0)
        {
            EditorGUILayout.HelpBox("ResourceConfiguration has no resources defined.", MessageType.Info);
            return;
        }
        
        DrawResourcesSection(resources);
        
        if (Application.isPlaying)
        {
            EditorGUILayout.Space(10);
            DrawDebugToolsSection(resources);
            
            EditorGUILayout.Space(10);
            DrawSceneCollectionSection(resources);
        }
        
        serializedObject.ApplyModifiedProperties();
    }

    private new void DrawHeader()
    {
        EditorGUILayout.BeginVertical(sectionStyle);
        
        string statusText = Application.isPlaying ? "PLAY MODE" : "EDIT MODE";
        Color statusColor = Application.isPlaying ? Color.green : Color.gray;
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Resource Manager", headerStyle);
        
        GUI.color = statusColor;
        GUILayout.Label(statusText, EditorStyles.miniLabel);
        GUI.color = Color.white;
        
        EditorGUILayout.EndHorizontal();
        
        if (config != null)
        {
            ResourceBase[] resources = config.GetAllResources();
            EditorGUILayout.LabelField($"Total Resources: {resources?.Length ?? 0}", EditorStyles.miniLabel);
        }
        
        EditorGUILayout.EndVertical();
    }

    private void DrawResourcesSection(ResourceBase[] resources)
    {
        EditorGUILayout.LabelField("Resources", EditorStyles.boldLabel);
        
        for (int i = 0; i < resources.Length; i++)
        {
            ResourceBase resource = resources[i];
            
            if (resource == null)
            {
                EditorGUILayout.HelpBox($"Resource at index {i} is null", MessageType.Error);
                continue;
            }
            
            DrawResourceItem(resource, i);
        }
    }

    private void DrawResourceItem(ResourceBase resource, int index)
    {
        EditorGUILayout.BeginVertical(resourceBoxStyle);
        
        // HEADER ROW: Foldout + Resource Name + Key Stats
        EditorGUILayout.BeginHorizontal();
        
        resourceFoldouts[index] = EditorGUILayout.Foldout(
            resourceFoldouts[index], 
            resource.GetName(), 
            true, 
            EditorStyles.foldoutHeader
        );
        
        GUILayout.FlexibleSpace();
        
        // Show key info even when minimized
        GUI.enabled = false;
        EditorGUILayout.LabelField("Current:", GUILayout.Width(50));
        EditorGUILayout.IntField(resource.CurrentAmount, GUILayout.Width(60));
        GUI.enabled = true;
        
        EditorGUILayout.EndHorizontal();
        
        // EXPANDED CONTENT
        if (resourceFoldouts[index])
        {
            EditorGUI.indentLevel++;
            
            EditorGUILayout.Space(3);
            
            // Stats display
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            GUI.enabled = false;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Amount:", GUILayout.Width(120));
            EditorGUILayout.IntField(resource.CurrentAmount);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Total Collected:", GUILayout.Width(120));
            EditorGUILayout.IntField(resource.TotalCollectedAmount);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Collectable Type:", GUILayout.Width(120));
            EditorGUILayout.EnumPopup(resource.CollectableType);
            EditorGUILayout.EndHorizontal();
            GUI.enabled = true;
            
            EditorGUILayout.EndVertical();
            
            // Quick action buttons (only in play mode)
            if (Application.isPlaying)
            {
                EditorGUILayout.Space(3);
                DrawQuickActionsForResource(resource);
            }
            
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(2);
    }

    private void DrawQuickActionsForResource(ResourceBase resource)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Quick Actions:", GUILayout.Width(100));
        
        // Quick add buttons
        foreach (int amount in quickAddAmounts)
        {
            if (GUILayout.Button($"+{amount}", GUILayout.Width(50)))
            {
                resourceManager.EditorAddResource(resource, amount);
            }
        }
        
        // Reset button
        GUI.color = new Color(1f, 0.7f, 0.7f);
        if (GUILayout.Button("Reset", GUILayout.Width(60)))
        {
            if (EditorUtility.DisplayDialog(
                "Reset Resource",
                $"Reset {resource.GetName()} to 0?",
                "Reset", "Cancel"))
            {
                resourceManager.EditorResetResource(resource);
            }
        }
        GUI.color = Color.white;
        
        EditorGUILayout.EndHorizontal();
    }

    private void DrawDebugToolsSection(ResourceBase[] resources)
    {
        EditorGUILayout.BeginVertical(sectionStyle);
        EditorGUILayout.LabelField("Debug Tools", EditorStyles.boldLabel);
        
        // Quick add configuration
        showQuickAddConfig = EditorGUILayout.Foldout(showQuickAddConfig, "Quick Add Configuration", true);
        if (showQuickAddConfig)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox("Configure the quick-add button amounts", MessageType.Info);
            
            for (int i = 0; i < quickAddAmounts.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Button {i + 1}:", GUILayout.Width(70));
                quickAddAmounts[i] = EditorGUILayout.IntField(quickAddAmounts[i], GUILayout.Width(100));
                quickAddAmounts[i] = Mathf.Max(1, quickAddAmounts[i]);
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space(5);
        }
        
        // Custom add
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("Custom Add", EditorStyles.miniBoldLabel);
        
        EditorGUILayout.BeginHorizontal();
        
        string[] resourceNames = new string[resources.Length];
        for (int i = 0; i < resources.Length; i++)
        {
            resourceNames[i] = resources[i]?.GetName() ?? $"Resource {i}";
        }
        
        customAddIndex = EditorGUILayout.Popup("Resource:", customAddIndex, resourceNames);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        customAddAmount = EditorGUILayout.IntField("Amount:", customAddAmount);
        
        if (GUILayout.Button("Add", GUILayout.Width(60)))
        {
            if (customAddIndex >= 0 && customAddIndex < resources.Length)
            {
                resourceManager.EditorAddResource(resources[customAddIndex], customAddAmount);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(5);
        
        // Bulk actions
        EditorGUILayout.BeginHorizontal();
        
        GUI.color = new Color(1f, 1f, 0.7f);
        if (GUILayout.Button("Reset Scene Collection", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog(
                "Reset Scene Collection",
                "Reset collection tracking for this scene?",
                "Reset", "Cancel"))
            {
                resourceManager.ResetSceneCollections();
            }
        }
        
        GUI.color = new Color(1f, 0.6f, 0.6f);
        if (GUILayout.Button("Clear All Resources", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog(
                "Clear All Resources",
                "This will reset ALL resources to 0. Continue?",
                "Clear All", "Cancel"))
            {
                resourceManager.ClearAllResources();
            }
        }
        GUI.color = Color.white;
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }

    private void DrawSceneCollectionSection(ResourceBase[] resources)
    {
        EditorGUILayout.BeginVertical(sectionStyle);
        
        showSceneCollection = EditorGUILayout.Foldout(
            showSceneCollection, 
            "Scene Collection Info", 
            true, 
            EditorStyles.foldoutHeader
        );
        
        if (showSceneCollection)
        {
            EditorGUI.indentLevel++;
            
            EditorGUILayout.HelpBox("Resources collected in the current scene", MessageType.Info);
            
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            // Table header
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Resource", EditorStyles.boldLabel, GUILayout.Width(150));
            EditorGUILayout.LabelField("Collected This Scene", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();
            
            // Separator
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(1));
            
            // Resource rows
            foreach (ResourceBase resource in resources)
            {
                if (!resource) continue;
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(resource.GetName(), GUILayout.Width(150));
                
                GUI.enabled = false;
                EditorGUILayout.IntField(resource.CollectedThisScene);
                GUI.enabled = true;
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUI.indentLevel--;
        }
        
        EditorGUILayout.EndVertical();
    }
}
#endif