using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor.Build.Content;
using UnityEngine;
using Yarn;
using Vector3 = UnityEngine.Vector3;

public class Map : MonoBehaviour
{
    [SerializeField]
    MapGenerator MapGenerator;
    [SerializeField]
    GameObject Camera;
    [SerializeField]
    GameObject Lines;
    [SerializeField]
    GameObject Visuals; 
    [SerializeField]
    Encounter LastRoom;
    [SerializeField]
    GameObject Rooms;

    [SerializeField]
    GameObject MAP_ROOM_PREFAB;
    [SerializeField]
    GameObject MAP_LINE_PREFAB;

    
    List<List<Encounter>> Map_Data = new List<List<Encounter>>();
    float camera_edge_y;
    int scrollspeed = 10000;
    int floorsClimbed = 0;
    void Start()
    {
        StartUp();
    }
    void Awake()
    {
        
    }
    void StartUp()
    {
        camera_edge_y = MapGenerator.Y_DIST * (Map_Data.Count - 1);

        GenerateNewMap();
        UnlockFloor(0);
    }
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            Camera.transform.localPosition = new UnityEngine.Vector3 (Camera.transform.localPosition.x, Mathf.Clamp(Camera.transform.localPosition.y + Input.mouseScrollDelta.y * scrollspeed * Time.deltaTime, -camera_edge_y, camera_edge_y), Camera.transform.localPosition.z);
            
        }
        
    }
    void GenerateNewMap()
    {
        floorsClimbed = 0;
        Map_Data = MapGenerator.GenerateMap();
        //print(Map_Data /*+ " : Floors :  " + Map_Data.Count + " : 1 floor-rooms : " + Map_Data[0].Count*/);
        CreateMap();
    }
    void CreateMap()
    {
        for(int currentFloor = 0; currentFloor < Map_Data.Count; currentFloor++)
        {
            for(int room = 0; room < Map_Data[currentFloor].Count; room++)
            {
                if(Map_Data[currentFloor][room].next_rooms.Count > 0)
                {
                    SpawnRoom(Map_Data[currentFloor][room]);
                }

            }
        }
        int middle = (int)(MapGenerator.WIDTH * 0.5);
        SpawnRoom(Map_Data[MapGenerator.FLOOR-1][middle]);

        var map_width_pixels = MapGenerator.X_DIST * (MapGenerator.WIDTH -1);
        Visuals.transform.position = new Vector3((Camera.GetComponent<Camera>().rect.x - map_width_pixels) /2, Camera.GetComponent<Camera>().rect.y /2 ,Visuals.transform.position.z);
        
    }
    void UnlockFloor(int whichfloor = 0)
    {
        for(int k = 0; k < Rooms.transform.childCount; k++)
        {
            //MapRoom map_room = Map_Data[whichfloor][k].GetComponent<MapRoom>();
            MapRoom map_room = Rooms.transform.GetChild(k).GetComponent<MapRoom>();
            print("map_room.room.row: " + map_room.room.row);
            if(map_room.room.row == whichfloor)
            {
                map_room.available = true;
            }
            
        }
    }
    void UnlockNextRoom()
    {
        for(int k = 0; k < Rooms.transform.childCount; k++)
        {
            MapRoom map_room = Rooms.transform.GetChild(k).GetComponent<MapRoom>();
            if(LastRoom.next_rooms.Contains(map_room.room))
            {
                map_room.available = true;
            }
        }
    }

    void SpawnRoom(Encounter room)
    {
        GameObject new_room = Instantiate(MAP_ROOM_PREFAB, Rooms.transform);
        MapRoom new_map_room = new_room.GetComponent<MapRoom>();
        new_map_room.room = room;
        new_room.transform.localPosition = new Vector3(new_map_room.room.position.X, new_map_room.room.position.Y, 0);
        //new_map_room.Selected.Connect()
        _connect_lines(room);

        if (room.selected && room.row < floorsClimbed)
        {
            new_map_room.ShowSelected();
        }
    }
    void _connect_lines(Encounter room)
    {
        if(room.next_rooms.Count == 0)
        {
            return;
        }
        for(int m = 0; m < room.next_rooms.Count; m++)
        {
            var new_map_line = Instantiate(MAP_LINE_PREFAB, Lines.transform);
            //figure out how to make the line connect itself from node 1 room.position to node 2 room.next_rooms[m].position
            //new_map_line.transform.position = new Vector3();
        }
    }
    void _on_map_room_selected(Encounter room)
    {
       for(int k = 0; k < Rooms.transform.childCount; k++)
        {
            MapRoom map_room = Rooms.transform.GetChild(k).GetComponent<MapRoom>();
            if(map_room.room.row == room.row)
            {
                map_room.available = false; 
            }
            LastRoom = room;
            floorsClimbed += 1;
            //Events.map_exited.emit(room);
        }
    }
}
