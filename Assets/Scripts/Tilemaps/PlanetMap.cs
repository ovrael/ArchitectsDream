using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;
using UnityEngine.Tilemaps;

using static UnityEditor.Handles;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.RayTracingAccelerationStructure;

public class PlanetMap : MonoBehaviour
{
    [Tooltip("The Tilemap to draw onto")]
    public Tilemap tilemap;
    [Tooltip("The Tile to draw (use a RuleTile for best results)")]
    public TileBase tile;

    [Tooltip("Width of our map")]
    public int width;
    [Tooltip("Height of our map")]
    public int height;

    [Tooltip("The settings of our map")]
    public MapSetting mapSetting;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            ClearMap();
            GenerateMap();
        }
    }

    [ExecuteInEditMode]
    public void GenerateMap()
    {
        ClearMap();
        int[,] map = new int[width, height];
        float seed;
        if (mapSetting.randomSeed)
        {
            seed = Time.time;
        }
        else
        {
            seed = mapSetting.seed;
        }

        //Generate the map depending omapSen the algorithm selected
        switch (mapSetting.algorithm)
        {
            case Algorithm.Perlin:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, true);
                //Next generate the perlin noise onto the array
                map = MapGenerator.PerlinNoise(map, seed);
                break;
            case Algorithm.PerlinSmoothed:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, true);
                //Next generate the perlin noise onto the array
                map = MapGenerator.PerlinNoiseSmooth(map, seed, mapSetting.interval);
                break;
            case Algorithm.PerlinCave:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, true);
                //Next generate the perlin noise onto the array
                map = MapGenerator.PerlinNoiseCave(map, mapSetting.modifier, mapSetting.edgesAreWalls);
                break;
            case Algorithm.RandomWalkTop:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, true);
                //Next generater the random top
                map = MapGenerator.RandomWalkTop(map, seed);
                break;
            case Algorithm.RandomWalkTopSmoothed:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, true);
                //Next generate the smoothed random top
                map = MapGenerator.RandomWalkTopSmoothed(map, seed, mapSetting.interval);
                break;
            case Algorithm.RandomWalkCave:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, false);
                //Next generate the random walk cave
                map = MapGenerator.RandomWalkCave(map, seed, mapSetting.clearAmount);
                break;
            case Algorithm.RandomWalkCaveCustom:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, false);
                //Next generate the custom random walk cave
                map = MapGenerator.RandomWalkCaveCustom(map, seed, mapSetting.clearAmount);
                break;
            case Algorithm.CellularAutomataVonNeuman:
                //First generate the cellular automata array
                map = MapGenerator.GenerateCellularAutomata(width, height, seed, mapSetting.fillAmount, mapSetting.edgesAreWalls);
                //Next smooth out the array using the von neumann rules
                map = MapGenerator.SmoothVNCellularAutomata(map, mapSetting.edgesAreWalls, mapSetting.smoothAmount);
                break;
            case Algorithm.CellularAutomataMoore:
                //First generate the cellular automata array
                map = MapGenerator.GenerateCellularAutomata(width, height, seed, mapSetting.fillAmount, mapSetting.edgesAreWalls);
                //Next smooth out the array using the Moore rules
                map = MapGenerator.SmoothMooreCellularAutomata(map, mapSetting.edgesAreWalls, mapSetting.smoothAmount);
                break;
            case Algorithm.DirectionalTunnel:
                //First generate our array
                map = MapGenerator.GenerateArray(width, height, false);
                //Next generate the tunnel through the array
                map = MapGenerator.DirectionalTunnel(map, mapSetting.minPathWidth, mapSetting.maxPathWidth, mapSetting.maxPathChange, mapSetting.roughness, mapSetting.windyness);
                break;
        }
        //Render the result
        MapGenerator.RenderMap(map, tilemap, tile);
    }

    public void ClearMap()
    {
        tilemap.ClearAllTiles();
    }
}

[CustomEditor(typeof(PlanetMap))]
public class LevelGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //Reference to our script
        PlanetMap levelGen = (PlanetMap)target;

        //Only show the mapsettings UI if we have a reference set up in the editor
        if (levelGen.mapSetting != null)
        {
            Editor mapSettingEditor = CreateEditor(levelGen.mapSetting);
            mapSettingEditor.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                levelGen.GenerateMap();
            }

            if (GUILayout.Button("Clear"))
            {
                levelGen.ClearMap();
            }
        }
    }
}