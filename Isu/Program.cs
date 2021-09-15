using Isu.Services;

namespace Isu
{
    internal class Program
    {
        public static void Main()
        {
            var isu = new Services.Isu();

            var group3304 = isu.AddGroup(new GroupName() { NumberOfCourse = 3, NumberOfGroup = 4 });
            var group3204 = isu.AddGroup(new GroupName() { NumberOfCourse = 2, NumberOfGroup = 4 });
            var group3515 = isu.AddGroup(new GroupName() { NumberOfCourse = 5, NumberOfGroup = 15 });

            isu.AddStudent(group3304, "Oksana Oksanova");
            isu.AddStudent(group3304, "Igor Igorev");
            isu.AddStudent(group3304, "Kirill Kirillov");

            isu.AddStudent(group3204, "Denis Denisov");
            isu.AddStudent(group3204, "Ekaterina Ekaterinova");
            isu.AddStudent(group3204, "Alexandra Alexandrova");

            isu.AddStudent(group3515, "Kazimir Kazimirov");
            isu.AddStudent(group3515, "Nikolai Nikolaev");
            isu.AddStudent(group3515, "Artem Artemov");

            isu.PrintGroups();

            isu.PrintStudents();

            isu.RemoveGroup(group3515.GroupName);

            isu.PrintGroups();
        }
    }
}