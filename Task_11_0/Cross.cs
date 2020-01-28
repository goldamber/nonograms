using System;
using System.Collections.Generic;

namespace Task_11_0
{
    public enum CrossGroup { Animals, Nature, People, Different, Buildings, Science }

    public class Cross
    {
        public string Name { get; set; }
        public bool Sloved { get; set; } = false;
        public CrossGroup CGroup { get; set; }
        public List<List<int>> hArr { get; set; } = new List<List<int>>();
        public List<List<int>> vArr { get; set; } = new List<List<int>>();

        public override bool Equals(object obj)
        {
            if (obj is Cross)
                return (obj as Cross).Name == Name;

            throw new Exception();
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }
}