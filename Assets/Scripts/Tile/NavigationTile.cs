using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavNode : IComparable<NavNode>
{
    public ENaveNodeState NodeState = ENaveNodeState.Noen;
    public NavNode Parent;
    public bool IsWall;

    public Vector2 Pos;
    public int IndexX = -1;
    public int IndexY = -1;

    public float Cost = float.MaxValue;  
    public float Dist = float.MaxValue;  
    public float Total = float.MaxValue; 

    public int CompareTo(NavNode other)
    {
        return other.Total.CompareTo(Total);
    }
}

public class PathInfo
{
    public RoomBehavior CurRoom;
    public PathInfo PrevRoom;

    public PathInfo(RoomBehavior cur, PathInfo prev)
    {
        CurRoom = cur;
        PrevRoom = prev;
    }
}

public class NavigationTile
{
    private TileManager tileManager;

    private readonly float _gridHeight = 0.5f;
    private readonly float _gridWidth = 1.0f;
    private readonly float _halfGridHeight = 0.25f;
    private readonly float _halfGridWidth = 0.5f;
    private readonly int _maxGridsPerTile = 6;

    private readonly int[] dx = { 1, -1, 0, 0, 1, 1, -1, -1 };
    private readonly int[] dy = { 0, 0, 1, -1, 1, -1, 1, -1 };

    private List<List<RoomBehavior>> _map;
    private List<List<NavNode>> _nodeList = new List<List<NavNode>>();
    private List<NavNode> _openNodeList = new List<NavNode>();
    private List<NavNode> _useNodeList = new List<NavNode>();

    private List<List<GameObject>> _testGizmo = new List<List<GameObject>>(); // TEST

    public void CreateNavigation(List<List<RoomBehavior>> map)
    {
        tileManager = Main.Get<TileManager>();
        _nodeList.Clear();
        _map = map;

        for (int i = 0; i < map.Count * _maxGridsPerTile; ++i)
        {
            _testGizmo.Add(new List<GameObject>()); // TEST 
            _nodeList.Add(new List<NavNode>());
            for (int j = 0; j < map[0].Count * _maxGridsPerTile; ++j)
            {
                GameObject go = Main.Get<ResourceManager>().Instantiate("Prefabs/Circle");
                go.transform.position = new Vector2(-_halfGridWidth * i + _halfGridWidth * j, 
                    _halfGridHeight * i + _halfGridHeight * j + 0.5f);

                NavNode navNode = CreateNavNode(i, j, go.transform.position);

                if((navNode.IndexX + 1) % _maxGridsPerTile == 0 || (navNode.IndexY + 1) % _maxGridsPerTile == 0) // test
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                }

                _nodeList[i].Add(navNode);
                _testGizmo[i].Add(go); // TEST
            }
        }

        for (int i = 0; i < _map.Count; ++i)
        {
            for (int j = 0; j < _map[i].Count; ++j)
            {
                SetCheckWall(_map[i][j]);
            }
        }
    }

    public void ExpandNodeRow()
    {
        for(int i = 0; i < _maxGridsPerTile; ++i)
        {
            _testGizmo.Add(new List<GameObject>()); // TEST 
            _nodeList.Add(new List<NavNode>());
            for(int j = 0; j < _nodeList[0].Count; ++j)
            {
                GameObject go = Main.Get<ResourceManager>().Instantiate("Prefabs/Circle");
                go.transform.position = new Vector2(-_halfGridWidth * (_nodeList.Count - 1) + _halfGridWidth * j,
                    _halfGridHeight * (_nodeList.Count - 1) + _halfGridHeight * j + 0.5f);

                NavNode navNode = CreateNavNode(_nodeList.Count - 1, j, go.transform.position);

                if ((navNode.IndexX + 1) % _maxGridsPerTile == 0 || (navNode.IndexY + 1) % _maxGridsPerTile == 0) // test
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                }

                _nodeList[_nodeList.Count - 1].Add(navNode);
                _testGizmo[_testGizmo.Count - 1].Add(go); // TEST
            }
        }
    }

    public void ExpandNodeCol()
    {
        for(int i = 0; i < _nodeList.Count; ++i)
        {
            for(int j = 0; j < _maxGridsPerTile; ++j)
            {
                GameObject go = Main.Get<ResourceManager>().Instantiate("Prefabs/Circle");
                go.transform.position = new Vector2(-_halfGridWidth * i + _halfGridWidth * _nodeList[i].Count,
                    _halfGridHeight * i + _halfGridHeight * _nodeList[i].Count + 0.5f);

                NavNode navNode = CreateNavNode(i, _nodeList[i].Count, go.transform.position);

                if ((navNode.IndexX + 1) % _maxGridsPerTile == 0 || (navNode.IndexY + 1) % _maxGridsPerTile == 0) // test
                {
                    go.GetComponent<SpriteRenderer>().color = Color.red;
                }

                _nodeList[i].Add(navNode);
                _testGizmo[i].Add(go); // TEST
            }
        }
    }

    public bool FindPath(Vector2 start, Vector2 end, out Stack<Vector2> stackPath)
    {
        NavNode startNode = GetNavNodeByPos(start);

        if(startNode == null)
        {
            stackPath = null;
            return false;
        }

        NavNode endNode = GetNavNodeByPos(end);

        if(endNode == null)
        {
            stackPath = null;
            return false;
        }

        if (endNode.IsWall)
        {
            stackPath = null;
            return false;
        }

        stackPath = new Stack<Vector2>();
        if (startNode == endNode)
        {
            stackPath.Push(end);
            return true;
        }

        for(int i = 0; i < _useNodeList.Count; ++i)
        {
            _useNodeList[i].Parent = null;
            _useNodeList[i].NodeState = ENaveNodeState.Noen;
            _useNodeList[i].Cost = float.MaxValue;
            _useNodeList[i].Dist = float.MaxValue;
            _useNodeList[i].Total = float.MaxValue;
        }
        _useNodeList.Clear();

        startNode.Parent = null;
        startNode.NodeState = ENaveNodeState.Open;
        startNode.Cost = 0f;
        startNode.Dist = Vector2.Distance(startNode.Pos, endNode.Pos);
        startNode.Total = startNode.Dist;
        _useNodeList.Add(startNode);

        _openNodeList.Add(startNode);
        while (_openNodeList.Count > 0)
        {
            NavNode curNode = _openNodeList[_openNodeList.Count - 1];
            _openNodeList.RemoveAt(_openNodeList.Count - 1);
            curNode.NodeState = ENaveNodeState.Close;

            if (FindNode(curNode, endNode, stackPath))
                break;

            if (_openNodeList.Count > 1)
            {
                _openNodeList.Sort();
            }
        }
        _openNodeList.Clear();

        return stackPath.Count > 0;
    }

    private bool FindNode(NavNode cur, NavNode end, Stack<Vector2> stackPath)
    {
        for (int i = 0; i < 8; ++i)
        {
            NavNode NeighborNode = GetNavNode(cur.IndexX + dx[i], cur.IndexY + dy[i]);

            if(NeighborNode == null)
                continue;

            if (NeighborNode.NodeState == ENaveNodeState.Close)
                continue;

            if (NeighborNode.IsWall)
                continue;

            // 위쪽으로 이동할 경우 왼쪽 위와 오른쪽 위 둘다 벽이 아니여야한다.
            if (i == 4 && CheckUpNode(cur))
                continue;

            // 왼쪽으로 이동할 경우 왼쪽 위와 오른쪽 아래 둘다 벽이 아니여야한다.
            if (i == 5 && CheckLeftNode(cur))
                continue;

            // 오른쪽으로 이동할 경우 오른쪽 위와 오른쪽 아래 둘다 벽이 아니여야한다.
            if (i == 6 && CheckRightNode(cur))
                continue;

            // 아래로 이동할 경우 왼쪽 아래와 오른쪽 아래 둘다 벽이 아니여야한다.
            if (i == 7 && CheckBottomNode(cur))
                continue;

            if (NeighborNode == end)
            {
                NeighborNode.Parent = cur;
                CreatePath(NeighborNode, stackPath);
                return true;
            }

            float Cost = cur.Cost + Vector2.Distance(NeighborNode.Pos, cur.Pos);

            if (NeighborNode.NodeState == ENaveNodeState.Open)
            {
                if (NeighborNode.Cost > Cost)
                {
                    NeighborNode.Parent = cur;
                    NeighborNode.Cost = Cost;
                    NeighborNode.Total = NeighborNode.Cost + NeighborNode.Dist;
                }
            }
            else
            {
                NeighborNode.NodeState = ENaveNodeState.Open;
                NeighborNode.Parent = cur;
                NeighborNode.Cost = Cost;
                NeighborNode.Dist = Vector2.Distance(NeighborNode.Pos, end.Pos);
                NeighborNode.Total = NeighborNode.Cost + NeighborNode.Dist;

                _openNodeList.Add(NeighborNode);
                _useNodeList.Add(NeighborNode);
            }
        }
        return false;
    }

    private bool CheckUpNode(NavNode cur)
    {
        NavNode LTNode = GetNavNode(cur.IndexX + 1, cur.IndexY);
        NavNode RTNode = GetNavNode(cur.IndexX, cur.IndexY + 1);

        if (LTNode != null)
        {
            if (LTNode.IsWall)
            {
                return true;
            }
        }

        if (RTNode != null)
        {
            if (RTNode.IsWall)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckLeftNode(NavNode cur)
    {
        NavNode LTNode = GetNavNode(cur.IndexX + 1, cur.IndexY);
        NavNode LBNode = GetNavNode(cur.IndexX, cur.IndexY - 1);

        if (LTNode != null)
        {
            if (LTNode.IsWall)
            {
                return true;
            }
        }

        if (LBNode != null)
        {
            if (LBNode.IsWall)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckRightNode(NavNode cur)
    {
        NavNode RTNode = GetNavNode(cur.IndexX, cur.IndexY + 1);
        NavNode RBNode = GetNavNode(cur.IndexX - 1, cur.IndexY);

        if (RTNode != null)
        {
            if (RTNode.IsWall)
            {
                return true;
            }
        }

        if (RBNode != null)
        {
            if (RBNode.IsWall)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckBottomNode(NavNode cur)
    {
        NavNode LBNode = GetNavNode(cur.IndexX, cur.IndexY - 1);
        NavNode RBNode = GetNavNode(cur.IndexX - 1, cur.IndexY);

        if (LBNode != null)
        {
            if (LBNode.IsWall)
            {
                return true;
            }
        }

        if (RBNode != null)
        {
            if (RBNode.IsWall)
            {
                return true;
            }
        }

        return false;
    }

    private void CreatePath(NavNode end, Stack<Vector2> stackPath)
    {
        _useNodeList.Add(end);
        NavNode navNode = end;
        while (navNode != null)
        {
            stackPath.Push(navNode.Pos);
            navNode = navNode.Parent;
        }
        stackPath.Pop();
    }

    public (int, int) GetIndex(Vector2 pos)
    {
        return (GetIndexX(pos), GetIndexY(pos));
    }

    public int GetIndexX(Vector2 pos)
    {
        float gridX = -pos.x / _gridWidth + pos.y / _gridHeight;
        return Mathf.RoundToInt(gridX) - 1;
    }

    public int GetIndexY(Vector2 pos)
    {
        float gridY = pos.y / _gridHeight + pos.x / _gridWidth;
        return Mathf.RoundToInt(gridY) - 1;
    }

    public NavNode GetNavNodeByPos(Vector2 pos)
    {
        return GetNavNode(GetIndexX(pos), GetIndexY(pos));
    }

    public NavNode GetNavNode(int x, int y)
    {
        if (x < 0 || y < 0)
            return null;

        if (x >= _nodeList.Count || y >= _nodeList[0].Count)
            return null;

        return _nodeList[x][y];
    }

    public Stack<GameObject> FindUnvisitedRoom(int x, int y, bool[,] visited)
    {
        Queue<PathInfo> roomQ = new Queue<PathInfo>();
        List<RoomBehavior> Neighbors = tileManager.GetNeighbors(x, y);
        bool[,] isVisited = new bool[_map.Count, _map[0].Count];

        foreach (var neighbor in Neighbors)
        {
            roomQ.Enqueue(new PathInfo(neighbor, null));
            isVisited[neighbor.IndexX, neighbor.IndexY] = true;
        }

        PathInfo targetInfo = null;
        bool isfind = false;
        while (true)
        {
            PathInfo info = roomQ.Dequeue();
            RoomBehavior curRoom = info.CurRoom;
            Neighbors = tileManager.GetNeighbors(curRoom.IndexX, curRoom.IndexY);

            foreach (var neighbor in Neighbors)
            {
                if (!visited[neighbor.IndexX, neighbor.IndexY])
                {
                    targetInfo = new PathInfo(neighbor, info);
                    isfind = true;
                    break;
                }
                else if (!isVisited[neighbor.IndexX, neighbor.IndexY])
                {
                    roomQ.Enqueue(new PathInfo(neighbor, info));
                    isVisited[neighbor.IndexX, neighbor.IndexY] = true;
                }
            }

            if (isfind)
                break;

            if (roomQ.Count <= 0)
                return null;
        }

        Stack<GameObject> pathList = new Stack<GameObject>();
        while (targetInfo != null)
        {
            pathList.Push(targetInfo.CurRoom.gameObject); 
            targetInfo = targetInfo.PrevRoom;
        }

        return pathList;
    }

    public void SetCheckWall(RoomBehavior room)
    {
        foreach(ERoomDir dir in Enum.GetValues(typeof(ERoomDir)))
        {
            if(dir > ERoomDir.LeftBottom)
            {
                break;
            }

            SetNodeWall(room, dir, room.IsDoorOpen(dir));
        }
    }

    private void SetNodeWall(RoomBehavior room, ERoomDir dir, bool isOpen)
    {
        Vector2 Pos = new Vector2(room.transform.position.x, room.transform.position.y + 1.5f);
        NavNode Node = null;
        switch (dir)
        {
            case ERoomDir.RightTop:
                Pos.x += (_gridWidth + _halfGridWidth);
                Pos.y += (_gridHeight + _halfGridHeight);
                Node = GetNavNodeByPos(Pos);
                break;
            case ERoomDir.RightBottom:
                Pos.x += (_gridWidth + _halfGridWidth);
                Pos.y -= (_gridHeight + _halfGridHeight);
                Node = GetNavNodeByPos(Pos);
                break;
            case ERoomDir.LeftTop:
                Pos.x -= (_gridWidth + _halfGridWidth);
                Pos.y += (_gridHeight + _halfGridHeight);
                Node = GetNavNodeByPos(Pos);
                break;
            case ERoomDir.LeftBottom:
                Pos.x -= (_gridWidth + _halfGridWidth);
                Pos.y -= (_gridHeight + _halfGridHeight);
                Node = GetNavNodeByPos(Pos);
                break;
        }

        if (Node == null)
        {
            return;
        }

        Node.IsWall = !isOpen;

        //test
        (int resultX, int resultY) = GetIndex(Pos);
        Color co = isOpen ? Color.blue : Color.red;
        _testGizmo[resultX][resultY].GetComponent<SpriteRenderer>().color = co;
    }

    private NavNode CreateNavNode(int x, int y, Vector2 pos)
    {
        NavNode navNode = new NavNode();

        navNode.Pos = pos;
        navNode.IndexX = x;
        navNode.IndexY = y;

        if ((navNode.IndexX + 1) % _maxGridsPerTile == 0 || (navNode.IndexY + 1) % _maxGridsPerTile == 0)
        {
            navNode.IsWall = true;
        }

        return navNode;
    }
}
