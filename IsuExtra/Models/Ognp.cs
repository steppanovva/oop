using System.Collections.Generic;

namespace IsuExtra.Models
{
    public class Ognp
    {
        public string Faculty { get; init; }
        public Course FirstDiscipline { get; init; }
        public Course SecondDiscipline { get; init; }
        public List<Student> Students { get; } = new ();
    }
}