namespace AdventOfCode2023.Day17
{
    using AdventOfCode2023.Utils;
    using AdventOfCode2023.Utils.Graph;
    using Priority_Queue;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // This solution upsets me profusely. I implemented an A* class in advance so that I could be ready
    // for a puzzle like this one. Only it turned out to be useless because of this puzzle's additional
    // rules of straight line sections being lower and upper bound. I've tried to add those checks in
    // my A*'s cost function but without carrying the current direction and straight line length in the
    // state that is used by the algorithm for each "node", it just doesn't seem to be possible. I
    // ended up writing a completely new A* algorithm from scratch and included the whole thing in this
    // file and namespace. But I don't like it. I would really like to fix my A* class to be more
    // generic and able to deal with this scenario.
    public class Solver : ISolver
    {
        public string Part1(string input)
        {
            var grid = new WeightedGrid(input.AsIntGrid());

            var totalLoss = AStar(
                field: grid,
                start: grid.Node("0,0")!,
                finish: grid.Node($"{grid.Width - 1},{grid.Height - 1}")!,
                minLeg: 0,
                maxLeg: 3);

            return totalLoss.ToString();
        }

        public string Part2(string input)
        {
            var grid = new WeightedGrid(input.AsIntGrid());

            var totalLoss = AStar(
                field: grid,
                start: grid.Node("0,0")!,
                finish: grid.Node($"{grid.Width - 1},{grid.Height - 1}")!,
                minLeg: 4,
                maxLeg: 10);

            return totalLoss.ToString();
        }

        private static int AStar(WeightedGrid field, GraphNode start, GraphNode finish, int minLeg, int maxLeg)
        {
            Dictionary<State, int> scoreCache = [];
            SimplePriorityQueue<State, int> frontier = new();

            frontier.Enqueue(
                item: new State(start, Direction.East, 0, 0),
                priority: 0);

            var step = 0;
            while (frontier.Count > 0)
            {
                step++;

                var state = frontier.Dequeue();

                //Console.WriteLine($"step:{step}, state{{node:{state.Node.Name}, dir:{state.Direction}, consec:{state.ConsecutiveDirections}, score:{state.Score}}}");

                if (state.Node.Equals(finish))
                {
                    return state.Score;
                }

                foreach (var node in field.Neighbours(state.Node).Where(node => !node.Coords!.Move(state.Direction).Equals(state.Node.Coords)))
                {
                    var direction = (Direction)state.Node.Coords!.DirectionTo(node.Coords!)!;
                    var isStraight = direction == state.Direction;
                    var straightLineLen = isStraight ? state.StraightLineLen + 1 : 1;

                    if (straightLineLen > maxLeg)
                        continue;

                    if (!isStraight && state.StraightLineLen < minLeg)
                        continue;

                    var newScore = state.Score + node.Value;
                    var newState = new State(node, direction, straightLineLen, newScore);

                    if (!scoreCache.TryGetValue(newState, out var cachedScore))
                        cachedScore = int.MaxValue;

                    if (newScore < cachedScore)
                    {
                        scoreCache[newState] = newScore;
                        if (!frontier.Contains(newState))
                        {
                            frontier.Enqueue(newState, newState.CalcScore(finish));
                        }
                    }
                }
            }

            return int.MaxValue; // No path was found
        }

        private class State(GraphNode node, Direction direction, int straightLineLen, int score) : IEquatable<State>
        {
            public GraphNode Node { get; } = node;
            public Direction Direction { get; } = direction;
            public int StraightLineLen { get; } = straightLineLen;
            public int Score { get; } = score;

            public int CalcScore(GraphNode destination) => Score + Heuristic(destination);
            private int Heuristic(GraphNode destination) => Node.Coords!.ManhattanDistanceTo(destination.Coords!);

            public override int GetHashCode()
            {
                return HashCode.Combine(Node, (int)Direction, StraightLineLen);
            }

            public bool Equals(State? other)
            {
                if (other is null) return false;
                if (ReferenceEquals(this, other)) return true;
                return Node.Equals(other.Node) && Direction == other.Direction &&
                       StraightLineLen == other.StraightLineLen;
            }

            public override bool Equals(object? obj)
            {
                if (obj is null) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((State)obj);
            }
        }
    }
}
