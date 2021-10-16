using System.Collections.Generic;
using NUnit.Framework;
using IsuExtra.Models;
using IsuExtra.Tools;

namespace IsuExtra.Tests
{
    [TestFixture]
    public class IsuExtraTests
    {
        private  Models.IsuExtra _testIsuExtra = new();
        private Stream _streamOfFirstDiscipline = new() { AvailablePlaces = 3 };
        private Stream _streamOfSecondDiscipline = new() { AvailablePlaces = 3 };
        
        [Test]
        public void AddOgnp_OgnpAdded()
        {
             var course1 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
             var course2 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
             Ognp ognp = _testIsuExtra.AddOgnp("Faculty 1", course1, course2);
            Assert.IsTrue(_testIsuExtra.Ognp.Exists(x => x.Faculty == ognp.Faculty));
        }
        
        [Test]
        public void RegisterStudent_StudentIsRegistered()
        {
            var lessons = new List<Lesson>
            {
                new() {LectureStart = "11:40", Lector = "Lector"},
                new() {LectureStart = "13:30", Lector = "Lector"}
            };
            var schedule = new Dictionary<string, List<Lesson>> {{"Monday", lessons}};
            Group group = _testIsuExtra.AddGroup(new GroupName() {NumberOfCourse = 2, NumberOfGroup = 5});
            group.Lessons = schedule;
            Student student = _testIsuExtra.AddStudent(group, "T");
            var course1 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
            var course2 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
            Ognp ognp = _testIsuExtra.AddOgnp("Faculty 1", course1, course2);
            ognp.FirstDiscipline.Streams.ForEach(stream => stream.Lessons = new Dictionary<string, List<Lesson>> {{"Monday", new List<Lesson>()}});
            ognp.SecondDiscipline.Streams.ForEach(stream => stream.Lessons = new Dictionary<string, List<Lesson>> {{"Monday", new List<Lesson>()}});
            student.Faculty = "Faculty 2";
            _testIsuExtra.RegisterStudent(student, ognp);
            Assert.IsTrue(ognp.Students.Exists(x => x.Id == student.Id));
        }
        
        [Test]
        public void CancelEntry_EntryCanceled()
        {
            var lessons = new List<Lesson> { new() {LectureStart = "11:40", Lector = "Lector"}, new() {LectureStart = "13:30", Lector = "Lector"}};
            var schedule = new Dictionary<string, List<Lesson>> {{"Monday", lessons}};
            Group group = _testIsuExtra.AddGroup(new GroupName() {NumberOfCourse = 2, NumberOfGroup = 6});
            group.Lessons = schedule;
            Student student = _testIsuExtra.AddStudent(group, "Van");
            var course1 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
            var course2 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
            Ognp ognp = _testIsuExtra.AddOgnp("Faculty 1", course1, course2);
            ognp.FirstDiscipline.Streams.ForEach(stream => stream.Lessons = new Dictionary<string, List<Lesson>> {{"Monday", new List<Lesson>()}});
            ognp.SecondDiscipline.Streams.ForEach(stream => stream.Lessons = new Dictionary<string, List<Lesson>> {{"Monday", new List<Lesson>()}});
            _testIsuExtra.RegisterStudent(student, ognp);
            _testIsuExtra.RemoveStudent(student, ognp);
            Assert.IsNull(ognp.Students.Find(x => x.Id == student.Id));
            Assert.IsNull(student.Course);
        }

        [Test]
        public void CancelEntry_ThrowExceptionStudentIsNotRegisteredOnCourse()
        {
            Student student = _testIsuExtra.AddStudent(new Group(), "X");
            var course1 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
            var course2 = new Course { Streams = new List<Stream> { _streamOfFirstDiscipline, _streamOfSecondDiscipline} };
            Ognp ognp = _testIsuExtra.AddOgnp("Faculty 1", course1, course2);
            Assert.Catch<IsuExtraException>(() =>
            {
                _testIsuExtra.RemoveStudent(student, ognp);
            });
        }
    }
}