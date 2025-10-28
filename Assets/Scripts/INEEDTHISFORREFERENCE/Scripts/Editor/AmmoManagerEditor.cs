using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AmmoManager))]
public class AmmoManagerEditor : Editor
{
    private bool showReserveAmmo = true;
    private bool showMagazineAmmo = true;
    private bool showWeaponInfo = true;
    private bool showDebugTools = true;
    
    private GUIStyle headerStyle;
    private GUIStyle boxStyle;
    private GUIStyle labelStyle;
    
    private void InitStyles()
    {
        headerStyle ??= new GUIStyle(EditorStyles.boldLabel)
        {
	        fontSize = 14,
	        margin = new RectOffset(0, 0, 10, 5)
        };
        
        boxStyle ??= new GUIStyle(EditorStyles.helpBox)
        {
	        padding = new RectOffset(10, 10, 10, 10)
        };
        
        labelStyle ??= new GUIStyle(EditorStyles.label);
    }

    public override void OnInspectorGUI()
    {
        InitStyles();
        
        AmmoManager ammoManager = (AmmoManager)target;
        
        // Title
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField("Ammo Manager", EditorStyles.whiteLargeLabel);
        EditorGUILayout.Space(10);
        
        // Configuration Section
        EditorGUILayout.BeginVertical(boxStyle);
        EditorGUILayout.LabelField("Configuration", headerStyle);
        EditorGUILayout.Space(5);
        
        SerializedProperty ammoResourcesProp = serializedObject.FindProperty("ammoResources");
        EditorGUILayout.PropertyField(ammoResourcesProp, new GUIContent("Ammo Resources"), true);
        
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.EndVertical();
        
        if (!Application.isPlaying)
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox("Enter Play Mode to see runtime ammo data", MessageType.Info);
            return;
        }
        
        EditorGUILayout.Space(10);
        
        // Reserve Ammo Section
        showReserveAmmo = EditorGUILayout.BeginFoldoutHeaderGroup(showReserveAmmo, "Reserve Ammo (Shared Pools)");
        if (showReserveAmmo)
        {
            EditorGUILayout.BeginVertical(boxStyle);
            DrawReserveAmmo(ammoManager);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space(5);
        
        // Magazine Ammo Section
        showMagazineAmmo = EditorGUILayout.BeginFoldoutHeaderGroup(showMagazineAmmo, "Magazine Ammo (Per Weapon)");
        if (showMagazineAmmo)
        {
            EditorGUILayout.BeginVertical(boxStyle);
            DrawMagazineAmmo(ammoManager);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space(5);
        
        // Current Weapon Info
        showWeaponInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showWeaponInfo, "Current Weapon Info");
        if (showWeaponInfo)
        {
            EditorGUILayout.BeginVertical(boxStyle);
            DrawCurrentWeaponInfo();
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        EditorGUILayout.Space(5);
        
        // Debug Tools
        showDebugTools = EditorGUILayout.BeginFoldoutHeaderGroup(showDebugTools, "Debug Tools");
        if (showDebugTools)
        {
            EditorGUILayout.BeginVertical(boxStyle);
            DrawDebugTools(ammoManager);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        
        // Force repaint during play mode
        if (Application.isPlaying)
        {
            Repaint();
        }
    }
    
    private void DrawReserveAmmo(AmmoManager ammoManager)
    {
        Dictionary<AmmoType, int> allReserve = ammoManager.GetAllReserveAmmoAmounts();
        
        if (allReserve.Count == 0)
        {
            EditorGUILayout.LabelField("No reserve ammo tracked yet", EditorStyles.miniLabel);
            return;
        }
        
        EditorGUILayout.BeginVertical();
        
        // Header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ammo Type", EditorStyles.boldLabel, GUILayout.Width(150));
        EditorGUILayout.LabelField("Amount", EditorStyles.boldLabel, GUILayout.Width(80));
        EditorGUILayout.LabelField("Bar", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(3);
        
        // Data rows
        foreach (var kvp in allReserve)
        {
            EditorGUILayout.BeginHorizontal();
            
            // Ammo type name
            EditorGUILayout.LabelField(kvp.Key.ToString(), GUILayout.Width(150));
            
            // Amount
            Color originalColor = GUI.color;
            if (kvp.Value == 0)
                GUI.color = Color.red;
            else if (kvp.Value < 50)
                GUI.color = Color.yellow;
            else
                GUI.color = Color.green;
            
            EditorGUILayout.LabelField(kvp.Value.ToString(), EditorStyles.boldLabel, GUILayout.Width(80));
            
            // Progress bar
            float maxAmount = 300f; // Adjust based on your game
            float fillAmount = Mathf.Clamp01(kvp.Value / maxAmount);
            Rect barRect = EditorGUILayout.GetControlRect(GUILayout.Height(18));
            EditorGUI.ProgressBar(barRect, fillAmount, "");
            
            GUI.color = originalColor;
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
        }
        
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"Total Types: {allReserve.Count}", EditorStyles.miniLabel);
    }
    
    private void DrawMagazineAmmo(AmmoManager ammoManager)
    {
        // Get all magazine data through reflection or serialized property
        SerializedProperty debugMagazineList = serializedObject.FindProperty("debugMagazineList");
        
        if (debugMagazineList == null || debugMagazineList.arraySize == 0)
        {
            EditorGUILayout.LabelField("No magazines initialized yet", EditorStyles.miniLabel);
            return;
        }
        
        EditorGUILayout.BeginVertical();
        
        // Header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Weapon Type", EditorStyles.boldLabel, GUILayout.Width(150));
        EditorGUILayout.LabelField("Ammo", EditorStyles.boldLabel, GUILayout.Width(80));
        EditorGUILayout.LabelField("Magazine Bar", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(3);
        
        // Data rows
        for (int i = 0; i < debugMagazineList.arraySize; i++)
        {
            SerializedProperty entry = debugMagazineList.GetArrayElementAtIndex(i);
            SerializedProperty weaponTypeProp = entry.FindPropertyRelative("weaponType");
            SerializedProperty currentAmmoProp = entry.FindPropertyRelative("currentAmmo");
            SerializedProperty maxAmmoProp = entry.FindPropertyRelative("maxAmmo");
            
            WeaponType weaponType = (WeaponType)weaponTypeProp.intValue;
            int currentAmmo = currentAmmoProp.intValue;
            int maxAmmo = maxAmmoProp.intValue;
            
            EditorGUILayout.BeginHorizontal();
            
            // Weapon type name
            EditorGUILayout.LabelField(weaponType.ToString(), GUILayout.Width(150));
            
            // Ammo count
            Color originalColor = GUI.color;
            if (currentAmmo == 0)
                GUI.color = Color.red;
            else if (currentAmmo < maxAmmo * 0.3f)
                GUI.color = Color.yellow;
            else
                GUI.color = Color.green;
            
            EditorGUILayout.LabelField($"{currentAmmo}/{maxAmmo}", EditorStyles.boldLabel, GUILayout.Width(80));
            
            // Progress bar
            float fillAmount = maxAmmo > 0 ? (float)currentAmmo / maxAmmo : 0f;
            Rect barRect = EditorGUILayout.GetControlRect(GUILayout.Height(18));
            EditorGUI.ProgressBar(barRect, fillAmount, "");
            
            GUI.color = originalColor;
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(2);
        }
        
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.Space(5);
        EditorGUILayout.LabelField($"Total Weapons: {debugMagazineList.arraySize}", EditorStyles.miniLabel);
    }
    
    private void DrawCurrentWeaponInfo()
    {
        if (!Globals.PlayerOne || !Globals.PlayerOne.Weapon)
        {
            EditorGUILayout.LabelField("No weapon currently equipped", EditorStyles.miniLabel);
            return;
        }
        
        Weapon currentWeapon = Globals.PlayerOne.Weapon;
        WeaponData weaponData = currentWeapon.WeaponData;
        
        EditorGUILayout.BeginVertical();
        
        // Current weapon info
        EditorGUILayout.LabelField("Equipped Weapon", EditorStyles.boldLabel);
        EditorGUILayout.Space(3);
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Name:", GUILayout.Width(120));
        EditorGUILayout.LabelField(weaponData.weaponName, EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Type:", GUILayout.Width(120));
        EditorGUILayout.LabelField(currentWeapon.WeaponType.ToString());
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ammo Type:", GUILayout.Width(120));
        EditorGUILayout.LabelField(weaponData.ammoType.ToString());
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(5);
        
        // Magazine status
        EditorGUILayout.LabelField("Magazine Status", EditorStyles.boldLabel);
        EditorGUILayout.Space(3);
        
        int currentAmmo = currentWeapon.CurrentAmmo;
        int maxAmmo = currentWeapon.MaxAmmo;
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Current / Max:", GUILayout.Width(120));
        
        Color originalColor = GUI.color;
        if (currentAmmo == 0)
            GUI.color = Color.red;
        else if (currentAmmo < maxAmmo * 0.3f)
            GUI.color = Color.yellow;
        
        EditorGUILayout.LabelField($"{currentAmmo} / {maxAmmo}", EditorStyles.boldLabel);
        GUI.color = originalColor;
        EditorGUILayout.EndHorizontal();
        
        // Magazine bar
        float fillAmount = maxAmmo > 0 ? (float)currentAmmo / maxAmmo : 0f;
        Rect barRect = EditorGUILayout.GetControlRect(GUILayout.Height(20));
        EditorGUI.ProgressBar(barRect, fillAmount, $"{fillAmount * 100:F0}%");
        
        EditorGUILayout.Space(5);
        
        // Reserve status
        EditorGUILayout.LabelField("Reserve Ammo", EditorStyles.boldLabel);
        EditorGUILayout.Space(3);
        
        if (weaponData.NewAmmoStruct.HasInfiniteAmmo)
        {
            EditorGUILayout.LabelField("∞ Infinite Ammo", EditorStyles.boldLabel);
        }
        else
        {
            int reserveAmmo = currentWeapon.ReserveAmmo;
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Reserve Amount:", GUILayout.Width(120));
            
            originalColor = GUI.color;
            if (reserveAmmo == 0)
                GUI.color = Color.red;
            else if (reserveAmmo < 50)
                GUI.color = Color.yellow;
            
            EditorGUILayout.LabelField(reserveAmmo.ToString(), EditorStyles.boldLabel);
            GUI.color = originalColor;
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndVertical();
    }
    
    private void DrawDebugTools(AmmoManager ammoManager)
    {
        EditorGUILayout.LabelField("Quick Actions", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Clear All Ammo", GUILayout.Height(25)))
        {
            if (EditorUtility.DisplayDialog("Clear All Ammo", 
                "This will clear all reserve and magazine ammo. Continue?", "Yes", "Cancel"))
            {
                ammoManager.ClearAllAmmo();
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space(10);
        
        // Add ammo tools
        EditorGUILayout.LabelField("Add Reserve Ammo", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);
        
        if (ammoManager.GetAllReserveAmmoAmounts().Count > 0)
        {
            foreach (var kvp in ammoManager.GetAllReserveAmmoAmounts())
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kvp.Key.ToString(), GUILayout.Width(150));
                
                if (GUILayout.Button("+10", GUILayout.Width(50)))
                {
                    ammoManager.AddReserveAmmo(kvp.Key, 10);
                }
                
                if (GUILayout.Button("+50", GUILayout.Width(50)))
                {
                    ammoManager.AddReserveAmmo(kvp.Key, 50);
                }
                
                if (GUILayout.Button("+100", GUILayout.Width(60)))
                {
                    ammoManager.AddReserveAmmo(kvp.Key, 100);
                }
                
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space(2);
            }
        }
        
        EditorGUILayout.Space(10);
        
        // Reload current weapon
        if (Globals.PlayerOne && Globals.PlayerOne.Weapon)
        {
            EditorGUILayout.LabelField("Current Weapon Actions", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Instant Reload", GUILayout.Height(25)))
            {
                ammoManager.InstantReload(Globals.PlayerOne.Weapon.WeaponType);
            }
            
            if (GUILayout.Button("Empty Magazine", GUILayout.Height(25)))
            {
                ammoManager.SetMagazineAmmo(Globals.PlayerOne.Weapon.WeaponType, 0);
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.Space(5);
        EditorGUILayout.HelpBox("Debug tools only work in Play Mode", MessageType.Info);
    }
}