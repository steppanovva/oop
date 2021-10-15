using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Services;
using Isu.Tools;
using IsuExtra.Tools;

namespace IsuExtra.Models
{
    public class IsuExtra
    {
        private int _id;
        private int maxStudentPerGroup;
        private List<Group> groups;
        private List<Student> students;

        public IsuExtra()
        {
            _id = 0;
            maxStudentPerGroup = 3;
            groups = new List<Group>();
            students = new List<Student>();
            Ognp = new List<Ognp>();
        }

        public List<Ognp> Ognp { get; }
        public Ognp AddOgnp(string faculty, Course firstDiscipline, Course secondDiscipline)
        {
            var newOgnp = new Ognp
            {
                Faculty = faculty,
                FirstDiscipline = firstDiscipline,
                SecondDiscipline = secondDiscipline,
            };
            Ognp.Add(newOgnp);
            return newOgnp;
        }

        public Student RegisterStudent(Student student, Ognp ognp)
        {
            if (student.Faculty == ognp.Faculty)
                throw new IsuExtraException("You can not enter your faculty course");
            Stream suitableStreamOfFirstDiscipline = null;
            Stream suitableStreamOfSecondDiscipline = null;
            var suitableStreams = new List<Stream>();
            foreach ((string day, List<Lesson> lessons) in student.Group.Lessons)
            {
                foreach (Lesson lesson in lessons)
                {
                    suitableStreamOfFirstDiscipline =
                        ognp.FirstDiscipline.Streams.Find(stream =>
                            stream.Lessons[day].All(x => x.LectureStart != lesson.LectureStart));
                    suitableStreamOfSecondDiscipline = ognp.SecondDiscipline.Streams.Find(stream =>
                        stream.Lessons[day].All(x => x.LectureStart != lesson.LectureStart));
                }
            }

            if (suitableStreamOfFirstDiscipline == null || suitableStreamOfSecondDiscipline == null)
                throw new IsuExtraException("There are intersections in the schedule found");
            if (suitableStreamOfFirstDiscipline.AvailablePlaces == 0 &&
                suitableStreamOfSecondDiscipline.AvailablePlaces == 0)
                throw new IsuExtraException("There are no available places");
            suitableStreamOfFirstDiscipline.AvailablePlaces -= 1;
            suitableStreamOfSecondDiscipline.AvailablePlaces -= 1;
            suitableStreamOfFirstDiscipline.Students.Add(student);
            suitableStreamOfSecondDiscipline.Students.Add(student);
            suitableStreams.Add(suitableStreamOfFirstDiscipline);
            suitableStreams.Add(suitableStreamOfSecondDiscipline);
            student.Course.Streams = suitableStreams;
            ognp.Students.Add(student);
            return student;
        }

        public void RemoveStudent(Student student, Ognp ognp)
        {
            if (!student.Course.Streams.Any())
                throw new IsuExtraException("This student is not registered on course");
            foreach (var i in student.Course.Streams)
            {
                i.AvailablePlaces += 1;
                i.Students.RemoveAll(x => x.Id == student.Id);
            }

            ognp.Students.RemoveAll(x => x.Id == student.Id);
            student.Course = null;
        }

        public List<Stream> GetStreamsOfCourse(Course course)
        {
            return course.Streams;
        }

        public List<Student> GetStudentInOgnp(Ognp ognp)
        {
            return ognp.Students;
        }

        public List<Student> GetStudentsWithoutOgnp(Group group)
        {
            return group.Students.Where(x => x.Course == null).ToList();
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
                throw new IsuExtraException("There is no " + newGroup.GroupName + " group");
            student.Group.Students.Remove(student);
            student.Group.GroupName = newGroup.GroupName;
            newGroup.Students.Add(student);
        }

        public void RemoveGroup(GroupName groupName)
        {
            if (groups.Exists(x => x.GroupName.ToString() == groupName.ToString()))
                groups.RemoveAll(x => x.GroupName.ToString() == groupName.ToString());
            else throw new IsuExtraException("There is no " + groupName + " group");
        }

        private bool IsGroupNameValid(GroupName groupName)
        {
            return groupName.NumberOfCourse > 0 && groupName.NumberOfCourse < 7 && groupName.NumberOfGroup < 16;
        }
    }
}