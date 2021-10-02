using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace Isu.Services
{
    public class Isu : IIsuService
    {
        private int _id;
        private int maxStudentPerGroup;
        private List<Group> groups;
        private List<Student> students;

        public Isu()
        {
            _id = 0;
            maxStudentPerGroup = 3;
            groups = new List<Group>();
            students = new List<Student>();
        }

        public Group AddGroup(GroupName name)
        {
            if (groups.Exists(x => x.GroupName.ToString() == name.ToString()))
                throw new IsuException("Group " + name.ToString() + " already added");
            if (!IsGroupNameValid(name))
                throw new IsuException("Entered groupName " + name.ToString() + " is invalid");
            groups.Add(new Group() { GroupName = name, Students = new List<Student>() });
            return groups.Last();
        }

        public Student AddStudent(Group group, string name)
        {
            if (group.Students.Exists(x => x.Name == name))
                throw new IsuException("Student " + name.ToString() + " already added to the group");
            if (groups.Exists(x => x.Students.Exists(y => y.Name == name)))
                throw new Exception("Student " + name.ToString() + " is already in another group");
            if (group.Students.Count > maxStudentPerGroup - 1)
                throw new IsuException("Can not add student " + name.ToString() + ", max number of students reached");
            var newStudent = new Student() { Id = _id, Name = name, Group = group };
            group.Students.Add(newStudent);
            students.Add(newStudent);
            return newStudent;
        }

        public Student GetStudent(int id)
        {
            if (students.Exists(x => x.Id == id))
                return students.Find(x => x.Id == id);
            throw new IsuException("There is no student with " + id + " ID");
        }

        public Student FindStudent(string name)
        {
            return students.Find(x => x.Name == name);
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            if (courseNumber.Number > 0 && courseNumber.Number < 7)
                return students.Where(x => x.Group.GroupName.NumberOfCourse == courseNumber.Number).ToList();
            throw new Exception(courseNumber.Number + " is invalid number");
        }

        public Group FindGroup(GroupName groupName)
        {
            if (groups.Exists(x => x.GroupName.ToString() == groupName.ToString()))
                return groups.Find(x => x.GroupName.ToString() == groupName.ToString());
            throw new IsuException("There is no " + groupName + " group");
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            if (!groups.Exists(x => x.GroupName.ToString() == groupName.ToString()))
                throw new IsuException("There is no " + groupName + " group");
            if (!FindGroup(groupName).Students.Any())
                throw new IsuException("There is no students in " + groupName + " group");
            return FindGroup(groupName).Students;
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return groups.Where(x => x.GroupName.NumberOfCourse == courseNumber.Number).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (!students.Exists(x => x.Name == student.Name))
                throw new IsuException("There is no " + student.Name + " " + student.Id + " in the list of students");
            if (!groups.Exists(x => x.GroupName.ToString() == newGroup.GroupName.ToString()))
                throw new IsuException("There is no " + newGroup.GroupName + " group");
            student.Group.Students.Remove(student);
            student.Group.GroupName = newGroup.GroupName;
            newGroup.Students.Add(student);
        }

        public void RemoveGroup(GroupName groupName)
        {
            if (groups.Exists(x => x.GroupName.ToString() == groupName.ToString()))
                groups.RemoveAll(x => x.GroupName.ToString() == groupName.ToString());
            else throw new IsuException("There is no " + groupName + " group");
        }

        public void PrintGroups()
        {
            Console.WriteLine("All groups");
            foreach (var x in groups)
                Console.WriteLine(x.GroupName.ToString());
            Console.WriteLine("\n");
        }

        public void PrintStudents()
        {
            Console.WriteLine("All students");
            foreach (var x in students)
                Console.WriteLine(x.Name);
            Console.WriteLine("\n");
        }

        private bool IsGroupNameValid(GroupName groupName)
        {
            return groupName.NumberOfCourse > 0 && groupName.NumberOfCourse < 7 && groupName.NumberOfGroup < 16;
        }
    }
}