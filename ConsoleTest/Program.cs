using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.Sqlite;

public class Room
{
    public int Vnum { get; set; }
    public string Name { get; set; }
    public List<Exit> Exits { get; set; } = new List<Exit>();

    public Room(int vnum, string name)
    {
        Vnum = vnum;
        Name = name;
    }
}

public class Exit
{
    public string Direction { get; set; }
    public int DestinationVnum { get; set; }

    public Exit(string direction, int destinationVnum)
    {
        Direction = direction;
        DestinationVnum = destinationVnum;
    }
}

public class PathNode
{
    public int Vnum { get; set; }
    public List<string> Path { get; set; }
}

public class TextGamePathFinder
{
    private Dictionary<int, Room> rooms;

    public TextGamePathFinder()
    {
        rooms = ParseRooms();
    }

    public List<string> FindPath(int startVnum, int endVnum)
    {
        if (!rooms.ContainsKey(startVnum) || !rooms.ContainsKey(endVnum))
        {
            return null;
        }

        var visited = new HashSet<int>();
        var queue = new Queue<PathNode>();
        queue.Enqueue(new PathNode { Vnum = startVnum, Path = new List<string>() });

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();

            if (node.Vnum == endVnum)
            {
                // Found the destination
                return node.Path;
            }

            if (visited.Contains(node.Vnum))
            {
                continue;
            }

            visited.Add(node.Vnum);

            // var currentRoom = rooms[node.Vnum];
            rooms.TryGetValue(node.Vnum, out Room currentRoom);

            if (currentRoom == null)
            {
                continue;
            }
            foreach (var exit in currentRoom.Exits)
            {
                if (!visited.Contains(exit.DestinationVnum))
                {
                    var newPath = new List<string>(node.Path);
                    newPath.Add(exit.Direction);
                    queue.Enqueue(new PathNode { Vnum = exit.DestinationVnum, Path = newPath });
                }
            }
        }

        // No path found
        return null;
    }

    private Dictionary<int, Room> ParseRooms()
    {
        var rooms = new Dictionary<int, Room>();

        using (var conn = new SqliteConnection(@"Data Source=C:\Users\blake\Dropbox\Documents\AvalonMudClient\dsl-mud.org.db"))
        {
            conn.Open();
            
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    select room.vnum
	                    , room.name
	                    , exits.to_vnum
	                    , exits.exit_name
	                    from room
	                    left join exits on exits.vnum = room.vnum
	                    left join room to_room on to_room.vnum = exits.to_vnum";

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int vnum = dr.GetInt32(0);
                        string name = dr.GetString(1);
                        
                        // Get or create the room
                        if (!rooms.ContainsKey(vnum))
                        {
                            rooms[vnum] = new Room(vnum, name);
                        }

                        var room = rooms[vnum];

                        // If the exist exists, add it in.
                        if (!dr.IsDBNull(2) && !dr.IsDBNull(3))
                        {
                            int toVnum = dr.GetInt32(2);
                            string direction = dr.GetString(3);
                        
                            // Add the exit
                            room.Exits.Add(new Exit(direction, toVnum));
                        }
                    }
                }
            }
        }

        return rooms;
    }
}

// Example usage:
class Program
{
    static void Main(string[] args)
    {
        var pathFinder = new TextGamePathFinder();

        var path1 = pathFinder.FindPath(3001, 72360);
        Console.WriteLine("Path: " + (path1 != null ? string.Join(", ", path1) : "No path found"));

        // // Example: Find path from 8400 to 8403
        // var path1 = pathFinder.FindPath(8400, 8403);
        // Console.WriteLine("Path from 8400 to 8403: " + (path1 != null ? string.Join(", ", path1) : "No path found"));
        //
        // // Example: Find path from 8400 to 8401
        // var path2 = pathFinder.FindPath(8400, 8401);
        // Console.WriteLine("Path from 8400 to 8401: " + (path2 != null ? string.Join(", ", path2) : "No path found"));
        //
        // // Example: Find path from 8400 to 8415
        // var path3 = pathFinder.FindPath(8400, 8415);
        // Console.WriteLine("Path from 8400 to 8415: " + (path3 != null ? string.Join(", ", path3) : "No path found"));
        //
        // // Example: Find path from 8400 to 8470
        // var path4 = pathFinder.FindPath(8400, 8470);
        // Console.WriteLine("Path from 8400 to 8470: " + (path4 != null ? string.Join("; ", path4) : "No path found"));
        
    }
}
