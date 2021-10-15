using System.Collections.Generic;

namespace IsuExtra.Models
{
    public class Group : Isu.Services.Group
    {
        public new List<Student> Students { get; init; } = new ();
        public Dictionary<string, List<Lesson>> Lessons { get; set; } = new ();
    }
}