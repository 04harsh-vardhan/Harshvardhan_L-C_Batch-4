namespace wqdwqd
{
    public class Solution
    {
        private bool isCycle = false;
        public bool CanFinish(int numCourses, int[][] prerequisites)
        {
            List<Nodes> nodes = new();
            for (int i = 0; i < numCourses; i++)
            {
                nodes.Add(new Nodes(i));
            }
            for (int i = 0; i < prerequisites.Length; i++)
            {
                Nodes node = nodes[prerequisites[i][0]];
                node.AddNeighbor(nodes[prerequisites[i][1]]);
            }
            int[] isVisited = new int[numCourses];
            int[] isTraversed = new int[numCourses];
            // traverse all nodes
            for (int i = 0; i < numCourses; i++)
            {
                if (isTraversed[i] == 0)
                {

                    HasCyclePresent(nodes[i], isVisited, isTraversed);
                }
            }
            if (isCycle)
            {
                isCycle = false;
                return false;
            }
            return true;
        }
        private void HasCyclePresent(Nodes node, int[] isVisited, int[] isTraversed)
        {
            if (isVisited[node.val] == 1)
            {
                isCycle = true;
                return;
            }
            isVisited[node.val] = 1;
            isTraversed[node.val] = 1;
            foreach (Nodes neighbor in node.neighbours)
            {
                if (isTraversed[neighbor.val] == 0)
                    HasCyclePresent(neighbor, isVisited, isTraversed);
            }
            isVisited[node.val] = 0;
        }
    }

    public class Nodes
    {
        public List<Nodes> neighbours = new();
        public int val;
        public Nodes(int node_val)
        {
            val = node_val;
        }
        public void AddNeighbor(Nodes node)
        {
            neighbours.Add(node);
        }
    }


}