using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class VoxelLevelEditor : EditorWindow
{
    // Type of Materials.
    enum ObjectMaterialType{
        DefaultMaterial,
        GrassMaterial,
        RockMaterial,
        SandMaterial
    }

    Material grassMaterial, rockMaterial, sandMaterial, defaultMaterial;             // Materials of voxel.
    ObjectMaterialType materialType;                            // Type of materials.
    static List<GameObject> voxelGameObjects = new List<GameObject>();
    int voxelCount;
    string saveFileName = "Save File Name";
    string saveFilePath;

    [MenuItem("Window/Voxel Level Editor")]
    public static void ShowWindow(){
        GetWindow<VoxelLevelEditor>("Voxel Level Editor");
    }
    
    void OnGUI(){
        // Initializing the texture.
        grassMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Editor/Materials/Grass.mat", typeof(Material));
        rockMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Editor/Materials/Rock.mat", typeof(Material));
        sandMaterial = (Material)AssetDatabase.LoadAssetAtPath("Assets/Editor/Materials/Sand.mat", typeof(Material));

        EditorGUILayout.LabelField("Voxel");
        EditorGUILayout.BeginHorizontal();
        // Button to create voxel;
        if(GUILayout.Button("Create")){
            CreateVoxel();
        }

        // Button to delete voxel;
        if(GUILayout.Button("Delete")){
            DeleteVoxel();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Material Type");
        EditorGUILayout.BeginHorizontal();
        // Drop down to select material.
        materialType = (ObjectMaterialType)EditorGUILayout.EnumPopup(materialType);

        // Applying the setting to voxels.
        if(GUILayout.Button("Apply")){
            ObjectTexture();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.LabelField("Level");
        
        // saveFileName = "SaveFileName";
        saveFileName = EditorGUILayout.TextField("File Name", saveFileName);
        
        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("Save")){
            SaveData();
        }

        if(GUILayout.Button("Load Save")){
            LoadData();
        }
        EditorGUILayout.EndHorizontal();

        if(GUILayout.Button("Create Player")){
            CreatePlayer();
        }
    }

    // Create Player from primitive Capsule.
    void CreatePlayer(){
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.tag = "Player";
        player.name = "Player";
        DestroyImmediate(player.GetComponent<CapsuleCollider>());
        player.AddComponent<BoxCollider>();
        player.AddComponent<Rigidbody>();
        player.AddComponent<PlayerMovement>();
    }

    // Create voxel from primitive cube.
    void CreateVoxel(){
        // Create the Voxel from Primitive Cube.
        GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
        voxel.tag = "Voxel";
        Renderer renderer = voxel.GetComponent<Renderer>();

        // Extract the default material.
        defaultMaterial = renderer.sharedMaterial;

        // Increase the voxel count.
        voxelCount++;

        voxel.name = "Voxel " + voxelCount;

        // Enable the voxel as selected gameobject.
        Selection.activeGameObject = voxel;

        // Add voxel in the voxel gameobject list.
        voxelGameObjects.Add(voxel);
    }

    // Delete the selected voxel.
    void DeleteVoxel(){
        // Initialize the active gameobject. 
        GameObject voxel = Selection.activeGameObject;

        // Remove the voxel from list and then delete it.
        voxelGameObjects.Remove(voxel);
        DestroyImmediate(voxel);
    }

    // Apply selected material to the voxel.
    void ObjectTexture(){
        // Check the selected texture option and apply it to the selected gameobject from the gameobjects list.
        foreach(GameObject objectList in Selection.gameObjects){
            Renderer renderer = objectList.GetComponent<Renderer>();
            if(renderer != null){

                // Compare the selected texture with the options.
                switch(materialType){
                    case ObjectMaterialType.DefaultMaterial:
                        renderer.sharedMaterial = defaultMaterial;
                        break;
                    case ObjectMaterialType.GrassMaterial:
                        renderer.sharedMaterial = grassMaterial;
                        break;
                    case ObjectMaterialType.RockMaterial:
                        renderer.sharedMaterial = rockMaterial;
                        break;
                    case ObjectMaterialType.SandMaterial:
                        renderer.sharedMaterial = sandMaterial;
                        break;
                }
            }
        }
    }

    void SaveData(){

        // Check the voxel list and remove the null gameobjects.
        CheckList();

        // Create GameData class which contains the SaveData class in list format.
        GameData gameData = new GameData();
        
        // File save path.
        saveFilePath = Application.persistentDataPath + "/" + saveFileName + ".json";

        // Iterate from each items in voxelGameObjects list.
        foreach(GameObject voxelObject in voxelGameObjects){
            // SaveLevelData class instance.
            SaveLevelData currentData = new SaveLevelData();

            // Initialize voxelObject members to the currentData members.
            currentData.voxelName = voxelObject.name;
            currentData.voxelTag = voxelObject.tag;
            currentData.voxelPosition = voxelObject.transform.position;
            currentData.voxelMaterial = voxelObject.GetComponent<Renderer>().sharedMaterial.name;

            // Add currentData instance to the saveLevelData class List;
            gameData.saveLevelData.Add(currentData);
        }

        // Convert the gameData class instance to Json string format.
        string saveGameData = JsonUtility.ToJson(gameData, true);

        // Write data to the file.
        File.WriteAllText(saveFilePath, saveGameData);
        Debug.Log("File Saved");
    }

    void LoadData(){
        // Delete all the existing voxel and Load the saved level.
        ClearList();

        string loadGameData;

        // File Path.
        saveFilePath = Application.persistentDataPath + "/" + saveFileName + ".json";

        // Check if file exist to the path.
        if(File.Exists(saveFilePath)){
            voxelCount = 0;

            // Read the file
            loadGameData = File.ReadAllText(saveFilePath);

            // Initialize the Json file data to the gameDataJson instance and in the saveLevelData class list.
            GameData gameDataJson = JsonUtility.FromJson<GameData>(loadGameData);

            // Iterate each elements from the saveLevelData list.
            foreach(SaveLevelData savedData in gameDataJson.saveLevelData){
                // Create the Voxel from Primitive Cube.
                GameObject voxel = GameObject.CreatePrimitive(PrimitiveType.Cube);
                Renderer renderer = voxel.GetComponent<Renderer>();
                defaultMaterial = renderer.sharedMaterial;

                voxelCount++;

                // Set the value to the voxel instance.
                voxel.name = savedData.voxelName;
                voxel.tag = savedData.voxelTag;
                voxel.transform.position = savedData.voxelPosition;

                switch(savedData.voxelMaterial){
                    case "Lit":
                        renderer.sharedMaterial = defaultMaterial;
                        break;
                    case "Grass":
                        renderer.sharedMaterial = grassMaterial;
                        break;
                    case "Rock":
                        renderer.sharedMaterial = rockMaterial;
                        break;
                    case "Sand":
                        renderer.sharedMaterial = sandMaterial;
                        break;
                }

                // Add voxel to the voxelGameObjects list.
                voxelGameObjects.Add(voxel);
            }
            Debug.Log("File Loaded");
        }
        else{
            Debug.Log("No file at :" + saveFilePath);
        }
    }

    void CheckList(){
        int i = 0;

        // Check i is less than total numbers of element in list.
        while(i < voxelGameObjects.Count){

            // if find any element with fill value just remove it.
            if(voxelGameObjects[i] == null){
                voxelGameObjects.Remove(voxelGameObjects[i]);
                i = 0;
                continue;
            }
            i++;
        }
    }

    void ClearList(){

        // Iterate from the voxel list and delete all the items.
        foreach(GameObject voxel in voxelGameObjects){
            DestroyImmediate(voxel);
        }
    }
}

// Voxel class to save voxel's properties.
[System.Serializable]
public class SaveLevelData{
    public string voxelName;
    public string voxelTag;
    public Vector3 voxelPosition;
    public string voxelMaterial;
}

// Instantiate voxels class in the form of list.
[System.Serializable]
public class GameData{
    public List<SaveLevelData> saveLevelData = new List<SaveLevelData>();
}
