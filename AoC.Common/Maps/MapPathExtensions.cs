namespace AoC.Common.Maps;

public static class MapPathExtensions
{
    public static int GetShortestPath(this Map<int> map, Point toPoint) =>
        map.GetShortestPath(new Point(0, 0), toPoint);

    public static int GetShortestPath(this Map<int> map, Point fromPoint, Point toPoint)
    {
        Map<int> currentCostPerPoint = new(map.SizeX, map.SizeY);
        HashSet<Point> visitedPoints = [];
        PriorityQueue<Point, int> openPositions = new();
        openPositions.Enqueue(fromPoint, 0);

        while (currentCostPerPoint.GetValue(toPoint) == 0)
        {
            var point = openPositions.Dequeue();
            var cost = currentCostPerPoint.GetValue(point);

            visitedPoints.Add(point);
            var nextPoints = map
                .GetStraightNeighbors(point)
                .Where(p => !visitedPoints.Contains(p));

            foreach (var nextPoint in nextPoints)
            {
                var nextCost = map.GetValue(nextPoint) + cost;
                var currentCost = currentCostPerPoint.GetValue(nextPoint);
                if (currentCost == 0 || nextCost < currentCost)
                {
                    currentCostPerPoint.SetValue(nextPoint, nextCost);
                    openPositions.Enqueue(nextPoint, nextCost);
                }
            }
        }

        return currentCostPerPoint.GetValue(toPoint);
    }

    public static int GetShortestPath<T>(this Map<T> map, Point fromPoint, Point toPoint, Func<Map<T>, Point, Point, bool> canMoveTo) where T : notnull
    {
        Map<int> currentCostPerPoint = new(map.SizeX, map.SizeY);
        HashSet<Point> visitedPoints = [];
        PriorityQueue<Point, int> openPositions = new();
        openPositions.Enqueue(fromPoint, 0);

        while (currentCostPerPoint.GetValue(toPoint) == 0)
        {
            if (openPositions.Count == 0)
            {
                // Dead end, no route possible
                return int.MaxValue;
            }

            var point = openPositions.Dequeue();
            var cost = currentCostPerPoint.GetValue(point);

            visitedPoints.Add(point);
            var nextPoints = map
                .GetStraightNeighbors(point)
                .Where(p => !visitedPoints.Contains(p) && canMoveTo(map, point, p));

            foreach (var nextPoint in nextPoints)
            {
                var nextCost = cost + 1;
                var currentCost = currentCostPerPoint.GetValue(nextPoint);
                if (currentCost == 0 || nextCost < currentCost)
                {
                    currentCostPerPoint.SetValue(nextPoint, nextCost);
                    openPositions.Enqueue(nextPoint, nextCost);
                }
            }
        }

        return currentCostPerPoint.GetValue(toPoint);
    }

    public static int GetLongestPath<T>(this Map<T> map, Point from, Point to, Func<Map<T>, Point, IEnumerable<Point>> getNeighbors) where T : notnull
    {
        List<List<Point>> routes = [[from]];
        var longestPath = 0;

        while (routes.Count > 0)
        {
            List<List<Point>> newRoutes = [];
            foreach (var route in routes)
            {
                var validNeighbors = getNeighbors(map, route[^1]).Where(n => !route.Contains(n));
                foreach (var neighbor in validNeighbors)
                {
                    if (neighbor == to)
                        longestPath = Math.Max(longestPath, route.Count);
                    else
                        newRoutes.Add(new(route) { neighbor });
                }
            }

            routes = newRoutes;
        }

        return longestPath;
    }
}