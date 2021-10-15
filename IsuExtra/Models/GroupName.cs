namespace IsuExtra.Models
{
    public class GroupName : Isu.Services.GroupName
    {
        public string FacultyId { get; set; }
        public override string ToString()
        {
            return (NumberOfGroup > 9) ? FacultyId + NumberOfCourse.ToString() + NumberOfGroup.ToString()
                : FacultyId + NumberOfCourse.ToString() + "0" + NumberOfGroup.ToString();
        }
    }
}