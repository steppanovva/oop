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

        public Group AddGroup(GroupName groupName)
        {
            if (groupName.NumberOfCourse > 0 && groupName.NumberOfCourse < 7 && groupName.NumberOfGroup < 16)
            {
                if (!groups.Exists(x => x.GroupName.ToString() == groupName.ToString()))
                {
                    groups.Add(new Group() { GroupName = groupName, Students = new List<Student>() });
                    return groups.Last();
                }
                else
                {
                    throw new IsuException("Group " + groupName.ToString() + " already added");
                }
            }
            else
            {
                throw new IsuException("Entered groupName " + groupName.ToString() + " is invalid");
            }
        }

        public Student AddStudent(Group group, string name)
        {
            if (!group.Students.Exists(x => x.Name == name))
            {
                if (!groups.Exists(x => x.Students.Exists(y => y.Name == name)))
                {
                    if (group.Students.Count < maxStudentPerGroup)
                    {
                        _id++;
                        var newStudent = new Student() { Id = _id, Name = name, Group = group };
                        group.Students.Add(newStudent);
                        students.Add(newStudent);
                        return newStudent;
                    }
                    else
                    {
                        throw new IsuException("Can not add student " + name.ToString() +
                                            ", max number of students reached");
                    }
                }
                else
                {
                    throw new Exception("Student " + name.ToString() + " is already in another group");
                }
            }
            else
            {
                throw new IsuException("Student " + name.ToString() + " already added to the group");
            }
        }

        public Student GetStudent(int id)
        {
            if (students.Exists(x => x.Id == id))
                return students.Find(x => x.Id == id);
            else throw new IsuException("There is no student with " + id + " ID");
        }

        public Student FindStudent(string name)
        {
            return students.Find(x => x.Name == name);
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            if (courseNumber.Get() > 0 && courseNumber.Get() < 7)
                return students.Where(x => x.Group.GroupName.NumberOfCourse == courseNumber.Get()).ToList();
            else throw new Exception(courseNumber.Get() + " is invalid number");
        }

        public Group FindGroup(GroupName groupName)
        {
            if (groups.Exists(x => x.GroupName.ToString() == groupName.ToString()))
                return groups.Find(x => x.GroupName.ToString() == groupName.ToString());
            else throw new IsuException("There is no " + groupName + " group");
        }

        public List<Student> FindStudents(GroupName groupName)
        {
            if (groups.Exists(x => x.GroupName.ToString() == groupName.ToString()))
            {
                if (FindGroup(groupName).Students.Any())
                {
                    return FindGroup(groupName).Students;
                }
                else
                {
                    throw new IsuException("There is no students in " + groupName + " group");
                }
            }
            else
            {
                throw new IsuException("There is no " + groupName + " group");
            }
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return groups.Where(x => x.GroupName.NumberOfCourse == courseNumber.Get()).ToList();
        }

        public void ChangeStudentGroup(Student student, Group group)
        {
            if (students.Exists(x => x.Name == student.Name))
            {
                if (groups.Exists(x => x.GroupName.ToString() == group.GroupName.ToString()))
                {
                    student.Group.Students.Remove(student);
                    student.Group.GroupName = group.GroupName;
                    group.Students.Add(student);
                }
                else
                {
                    throw new IsuException("There is no " + group.GroupName + " group");
                }
            }
            else
            {
                throw new IsuException("There is no " + student.Name + " " + student.Id + " in the list of students");
            }
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
    }
}