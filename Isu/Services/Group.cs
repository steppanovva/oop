using System.Collections.Generic;

namespace Isu.Services
{
    public class Group
    {
        public GroupName GroupName { get; set; }
        public List<Student> Students { get; set; }
    }
}