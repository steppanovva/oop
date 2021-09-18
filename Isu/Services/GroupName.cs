namespace Isu.Services
{
    public class GroupName
    {
        public int NumberOfCourse { get; set; }

        public int NumberOfGroup { get; set; }
        public override string ToString()
        {
            return (NumberOfGroup > 9) ? "M3" + NumberOfCourse.ToString() + NumberOfGroup.ToString()
                : "M3" + NumberOfCourse.ToString() + "0" + NumberOfGroup.ToString();
        }
    }
}