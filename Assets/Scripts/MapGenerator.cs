using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Collections;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

public class MapGenerator : MonoBehaviour
{
    public int X_DIST = 30;
    public int Y_DIST = 25;
    public int PLACEMENT_RANDOMNESS = 5;
    public int FLOOR = 10;
    public int WIDTH = 7;
    int PATHS = 6;
    float ENEMY_ROOM_WEIGHT = 7f;
    float SHOP_ROOM_WEIGHT = 3f;
    float ENCOUNTER_ROOM_WEIGHT = 6.0f;

    float CARD_ROOM_WEIGHT = 3f;

    //public Encounter.Type[][] MAP = new Encounter.Type[8][];
    public List<List<Encounter>> Map_Data;
    
    
    void Start()
    {
        //MAP[4][1] = Encounter.Type.BOSS;
        //GenerateMap();
    }
    public List<List<Encounter>> GenerateMap()
    {
        Map_Data = GenerateInitialGrid(); 
        var startingPoints = GetRandomStartingPoint();
        /*Debug.Log(Map_Data + " Count: " + Map_Data.Count);

        for(int i = 0; i < Map_Data.Count; i++)
        {
        Debug.Log("Floor: " + i + "      " + Map_Data[i]);
        for(int j = 0; j< Map_Data[i].Count; j++)
            {
                Debug.Log("Room: " + j + "    " + Map_Data[i][j]);
            }
        }
        Map_Data.ForEach(x => Debug.Log(x));
        
         for(int i = 0; i < startingPoints.Count; i++)
        {
            print(startingPoints[i]);
        } 
        
        */
        int current_j = 0;
        for(int j= 0; j < startingPoints.Count; j++)
        {
            

            print("Current J is: " + current_j + " ; J normally is: " + j);
            for(int i = 0; i < FLOOR - 1; i++)
            {
                current_j = SetupConnections(i, current_j);
                
            }   
            
            SetUpBossRoom();
            SetUpRandomRoom();
            SetUpRoomTypes();         
        }  
        /*var m = 0; 
        for(int i = 0; i < Map_Data.Count; i++)
        {
            print($" {Map_Data[i]} : ");
            print(Map_Data[i][m].next_rooms);
        } */
        return Map_Data;
    }
    List<int> GetRandomStartingPoint()
    {
        List<int> y_coordinates = new List<int>();
        int unique_points = 0;
        while (unique_points < 2)
        {
            y_coordinates = new List<int>();
            unique_points = 0;
            for(int i = 0; i < PATHS; i++)
            {
                int starting_point = UnityEngine.Random.Range(0, WIDTH-1);
                //checks through Y-list if randomly selected starting point is actually unique or already taken
                bool ycheck= false;
                for(int j = 0; j < y_coordinates.Count; j++)
                {
                    if(starting_point == y_coordinates[j])
                    {
                        ycheck = true;
                    }
                }
                
                if(ycheck != true)
                {
                    unique_points += 1;
                }
                else
                {
                    ycheck = false;
                }
                y_coordinates.Add(starting_point);
            }
        }
        return y_coordinates;
    }
    public List<List<Encounter>> GenerateInitialGrid()
    {
        List<List<Encounter>> result = new List<List<Encounter>>();
        
        for(int i = 0; i < FLOOR; i++)
        {
            List<Encounter> adjacent_rooms = new List<Encounter>();
         
            for(int j=0; j < WIDTH; j++) 
            {
                //Debug.Log("JLoop: " + j);
                Encounter current_room = ScriptableObject.CreateInstance<Encounter>();
                Vector2 offset;
                offset = new Vector2(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f)) * PLACEMENT_RANDOMNESS;
                current_room.position = new Vector2(j * X_DIST, i * Y_DIST) + offset;
                current_room.row = i;
                current_room.column = j;
                print(j);
                current_room.next_rooms= new List<Encounter>();

                //non-random Y on boss encounters
                if(i == FLOOR - 1) 
                {
                    current_room.position.Y = (i+1) * Y_DIST;
                }
                adjacent_rooms.Add(current_room);
                
            }
            result.Add(adjacent_rooms);
        }
        return result;
    }
    int SetupConnections(int i, int j)
    {
        Encounter next_room = null;
        Encounter current_room = Map_Data[i][j];
        //print(WouldCrossExistingPath(i, j, next_room));
        while(next_room == null | WouldCrossExistingPath(i, j, next_room))
        {
            int random_j = Mathf.Clamp(UnityEngine.Random.Range(j-1, j+2),0, WIDTH-1);
            next_room = Map_Data[i + 1][random_j];
            //print("Next Room: " + next_room);
        }
        current_room.next_rooms.Add(next_room);
        return next_room.column;
    }
    bool WouldCrossExistingPath(int i, int j, Encounter room)
    {
        if(room == null)
        {
            return true;
        }
        Encounter leftNeighbour = null;
        Encounter rightNeighbour = null;
        if (j > 0)
        {
            leftNeighbour = Map_Data[i][j-1];
        }
        if (j < WIDTH-1)
        {
            rightNeighbour = Map_Data[i][j+1];
        }
        //print("Room Column: " + room.column);
        if(rightNeighbour && room.column > j)
        {
            for(int m = 0; m < rightNeighbour.next_rooms.Count; m++)
            {
                
                if(rightNeighbour.next_rooms[m].column < room.column)
                {
                    return true;
                }
            }
        }
        if(leftNeighbour && room.column < j)
        {
                for(int m = 0; m < leftNeighbour.next_rooms.Count; m++)
            {
                if(leftNeighbour.next_rooms[m].column > room.column)
                {
                    return true;
                }
            }
        }

        return false;
    }
    void SetUpBossRoom()
    {
        int middle = (int)(WIDTH * 0.5f);
        Encounter boss_room = Map_Data[FLOOR-1][middle];
        for(int j=0; j< WIDTH; j++)
        {
            Encounter current_room = Map_Data[FLOOR-2][j];
            if(current_room.next_rooms.Count > 0)
            {
                current_room.next_rooms.Clear();
                current_room.next_rooms.Add(boss_room);
            }
        }
        boss_room.type = Encounter.Type.BOSS;
    }
    void SetUpRandomRoom()
    {
        
    }
    void SetUpRoomTypes()
    {
        //first floor is always battle
        for(int i = 0; i < Map_Data[0].Count; i++)
        { 
            if(Map_Data[0][i].next_rooms.Count() > 0)
            {
                Map_Data[0][i].type = Encounter.Type.ENEMY;

            }
        }
        for(int i=0;i<Map_Data[3].Count;i++)
        {
            if(Map_Data[3][i].next_rooms.Count() > 0)
            {
                Map_Data[3][i].type = Encounter.Type.CARDS;
            }
        }
        for(int i=0;i< Map_Data[FLOOR-2].Count; i++)
        {
            if(Map_Data[FLOOR-2][i].next_rooms.Count() > 0)
            {
                Map_Data[FLOOR-2][i].type = Encounter.Type.SHOP;

            }
        }
        for(int currentfloor = 0; currentfloor < Map_Data.Count; currentfloor++)
        {
            for(int room = 0; room < Map_Data[currentfloor].Count; room++)
            {
                for(int i = 0; i < Map_Data[currentfloor][room].next_rooms.Count; i++)
                {
                    if(Map_Data[currentfloor][room].next_rooms[i].type == Encounter.Type.NOT_ASSIGNED)
                    {
                        SetUpRandomly(Map_Data[currentfloor][room].next_rooms[i]);
                    }
                }
            }
        }
    }
    void SetUpRandomly(Encounter room)
    {
        bool shop_below = true;
        bool consecutive_shop = true;
        bool consecutive_Cards = true;
        bool shop_on_final = true;
        bool CardsOnThree = true;

        Encounter.Type type_candidate = Encounter.Type.NOT_ASSIGNED;
        while(shop_below | consecutive_shop | consecutive_Cards | shop_on_final | CardsOnThree)
        {
            type_candidate = GetRandomRoomTypeByWeight();
            print("Type Candidate =  " + type_candidate);

            bool is_shop = (type_candidate == Encounter.Type.SHOP);
            bool has_shop_parent = RoomHasParentOfType(room, Encounter.Type.SHOP);
            bool is_cards = (type_candidate == Encounter.Type.CARDS);
            bool has_cards_parent= RoomHasParentOfType(room, Encounter.Type.CARDS);

            shop_below = is_shop && room.row < 2;
            consecutive_shop = is_shop && has_shop_parent;
            consecutive_Cards = is_cards && has_cards_parent;
            shop_on_final = is_shop && room.row == FLOOR-3;
            CardsOnThree = is_cards && room.row == 2;


        }
        if(type_candidate == Encounter.Type.NOT_ASSIGNED) {print("ERROR inSetUpRandomly Function, Room's Type Candidate has returned as NOT_ASSIGNED");}
        room.type = type_candidate;
    }
    bool RoomHasParentOfType(Encounter room, Encounter.Type type)
    {
        List<Encounter> parents = new List<Encounter>();
        if (room.column > 0 && room.row > 0)
        {
            Encounter parent_candidate = Map_Data[room.row-1][room.column-1];
            if (parent_candidate.next_rooms.Contains(room))
            {
                parents.Add(parent_candidate);
            }
        }
        if(room.row > 0)
        {
            Encounter parent_candidate = Map_Data[room.row-1][room.column];
            if (parent_candidate.next_rooms.Contains(room))
            {
                parents.Add(parent_candidate);
            }
        }
        if (room.column < WIDTH-1 && room.row >0)
        {
            Encounter parent_candidate = Map_Data[room.row-1][room.column+1];
            if (parent_candidate.next_rooms.Contains(room))
            {
                parents.Add(parent_candidate);
            }
        }
        for(int i=0; i < parents.Count; i++)
        {
            if (parents[i].type == type)
            {
                return true;
            }
        }

        return false;
    }
    Encounter.Type GetRandomRoomTypeByWeight()
    {
        float fullWeight = ENEMY_ROOM_WEIGHT + SHOP_ROOM_WEIGHT + ENCOUNTER_ROOM_WEIGHT+CARD_ROOM_WEIGHT;
        float halfWeight = ENEMY_ROOM_WEIGHT + SHOP_ROOM_WEIGHT + CARD_ROOM_WEIGHT;
        float minor_weight = ENEMY_ROOM_WEIGHT + CARD_ROOM_WEIGHT;
        float roll= UnityEngine.Random.Range(0.0f, fullWeight);

        if(roll < ENEMY_ROOM_WEIGHT)
        {
            //ENEMYROOM
            return Encounter.Type.ENEMY;
        }
        else if(roll < minor_weight)
        {
            return Encounter.Type.CARDS;
        }
        else if(roll < halfWeight)
        {
            //SHOPROOM
            return Encounter.Type.SHOP;
        }
        else
        {
            //EncounterROOM
            return Encounter.Type.ENCOUNTER;
        }
    }
}
