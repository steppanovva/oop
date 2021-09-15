namespace Isu.Services
{
    public class GroupName
    {
        public int NumberOfCourse { get; set; }

        public int NumberOfGroup { get; set; }
        public override string ToString()
        {
            if (NumberOfGroup > 9)
                return "M3" + NumberOfCourse.ToString() + NumberOfGroup.ToString();
            else return "M3" + NumberOfCourse.ToString() + "0" + NumberOfGroup.ToString();
        }
    }
}