namespace IsuExtra.Models
{
    public class Student : Isu.Services.Student
    {
        public Course Course { get; set; } = new ();
        public new Group Group { get; init; }
        public string Faculty { get; set; }
    }
}